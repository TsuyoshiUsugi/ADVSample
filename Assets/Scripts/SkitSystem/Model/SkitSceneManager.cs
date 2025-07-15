using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using SkitSystem.Common;
using SkitSystem.Model.SkitSceneData;
using UnityEngine;

namespace SkitSystem.Model
{
    /// <summary>
    ///     会話シーンの進行を処理するクラス
    /// </summary>
    public class SkitSceneManager : MonoBehaviour
    {
        [SerializeField] private SkitSceneDataContainer _skitSceneDataContainer;

        private readonly Queue<SkitSceneDataAbstractBase> _skitContextQueue = new();
        private CompositeDisposable _cancellationDisposables = new();

        private CancellationTokenSource _masterCancellationTokenSource;

        
        public List<SkitSceneExecutorBase> SkitContextHandlers { get; } = new();
        public CancellationTokenSource CurrentCancellationToken { get; private set; }

        private void Start()
        {
            _masterCancellationTokenSource = new CancellationTokenSource();
            CurrentCancellationToken = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            CancelAllSkitOperations();
            _masterCancellationTokenSource?.Dispose();
            CurrentCancellationToken?.Dispose();
        }

        public event Func<UniTask> OnSkitEnd;

        /// <summary>
        ///     実際に処理を回す際に使うデータを設定する。現在のフラグ状態に応じて、スキットシーンデータを取得する。
        /// </summary>
        public void Initialize()
        {
            //ハンドラーの準備
            SkitContextHandlers.Add(new ConversationExecutor());

            //フラグから最初の会話データを取得
            string currentFlag = null;
            if (_skitSceneDataContainer.SkitSceneData.TryGetValue(nameof(FlagData), out var flagDataList))
            {
                var flags = flagDataList
                    .OfType<FlagData>()
                    .ToList();

                currentFlag = flags
                    .FirstOrDefault(flag => flag.Flags.ContainsValue(true))?.CurrentFlag;
            }

            if (_skitSceneDataContainer.SkitSceneData.TryGetValue(nameof(ConversationGroupData),
                    out var conversationGroupData))
            {
                var conversationData = conversationGroupData.FirstOrDefault(x =>
                                           x is ConversationGroupData currentFlagConversationGroupData &&
                                           // フラグに対応する会話データがない場合、最初の会話データを使用
                                           currentFlagConversationGroupData.Flag == currentFlag) ??
                                       conversationGroupData.FirstOrDefault(x => x is ConversationGroupData);

                if (conversationData != null) _skitContextQueue.Enqueue(conversationData);
            }
        }

        public async UniTask DoSkitSequence()
        {
            CancelSkitSequence();

            try
            {
                // マスターキャンセルトークンを使用
                var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(
                    _masterCancellationTokenSource.Token,
                    CurrentCancellationToken.Token
                );

                await ExecuteSkitSequence(linkedToken.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("スキットシーケンスがキャンセルされました");
            }
        }

        static bool debug = false;
        private async UniTask ExecuteSkitSequence(CancellationToken token)
        {
            if (debug)
            {
                Debug.Log("スキットシーケンスを開始します");
            }
            debug = true;
            
            while (_skitContextQueue.Count > 0)
            {
                token.ThrowIfCancellationRequested();

                var currentCount = _skitContextQueue.Count;
                var currentSkitContext = _skitContextQueue.Peek();
                if (currentSkitContext == null)
                {
                    Debug.LogError("SkitContextがnullです");
                    _skitContextQueue.Dequeue(); // null要素を削除してスキップ
                    continue;
                }

                var handleSkitContextType = currentSkitContext.GetType().Name;
                foreach (var skitContextHandler in SkitContextHandlers.Where(skitContextHandler =>
                             skitContextHandler.HandleSkitContextType == handleSkitContextType))
                {
                    // 現在のコンテキストを処理し、デキュー
                    await skitContextHandler.HandleSkitSceneData(_skitContextQueue.Dequeue(), token);

                    // 次のスキットコンテキストがある場合、エンキュー
                    if (skitContextHandler.TrtGetNextSkitSceneData(out var nextSkitContextQueue))
                        _skitContextQueue.Enqueue(nextSkitContextQueue);
                }

                if (currentCount == _skitContextQueue.Count)
                {
                    Debug.LogWarning("スキットコンテキストの数が変わらないため、無限ループの可能性があります。処理を中断します。");
                    break; // 無限ループを防ぐために中断
                }
            }

            if (OnSkitEnd != null) await OnSkitEnd.Invoke();
        }

        /// <summary>
        ///     一括キャンセル処理（全体停止）
        /// </summary>
        private void CancelAllSkitOperations()
        {
            // 1. 全ての進行中の処理をキャンセル
            _masterCancellationTokenSource?.Cancel();
            CurrentCancellationToken?.Cancel();

            // 2. 全ての購読を解除
            _cancellationDisposables?.Dispose();
            SkitContextHandlers?.ForEach(handler => handler.Dispose());
            SkitContextHandlers?.Clear();

            // 3. 新しいトークンソースを作成
            _masterCancellationTokenSource = new CancellationTokenSource();
            CurrentCancellationToken = new CancellationTokenSource();
            _cancellationDisposables = new CompositeDisposable();
        }

        private void CancelSkitSequence()
        {
            CurrentCancellationToken?.Cancel();
            SkitContextHandlers.ToList().ForEach(handler => handler.Dispose());
            CurrentCancellationToken = new CancellationTokenSource();
        }
    }
}