using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using SkitSystem.Common;
using SkitSystem.Model.SkitSceneData;
using SkitSystem.Model.SkitSceneExecutor;
using UnityEngine;

namespace SkitSystem.Model
{
    /// <summary>
    ///     会話シーンの進行を処理するクラス
    /// </summary>
    public class SkitSceneManager : MonoBehaviour
    {
        private readonly Queue<SkitSceneDataAbstractBase> _skitContextQueue = new();
        public List<SkitSceneExecutorBase> SkitContextHandlers { get; } = new();
        public CancellationTokenSource CurrentCancellationToken { get; private set; }

        private void Start()
        {
            CurrentCancellationToken = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            CancelSkitSequence();
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
            if (SkitSceneDataContainer.Instance.SkitSceneData.TryGetValue(nameof(FlagData), out var flagDataList))
            {
                var flags = flagDataList
                    .OfType<FlagData>()
                    .ToList();

                currentFlag = flags
                    .FirstOrDefault(flag => flag.Flags.ContainsValue(true))?.CurrentFlag;
            }

            if (SkitSceneDataContainer.Instance.SkitSceneData.TryGetValue(nameof(ConversationGroupData),
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
                await ExecuteSkitSequence(CurrentCancellationToken.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("スキットシーケンスがキャンセルされました");
            }
        }

        private async UniTask ExecuteSkitSequence(CancellationToken token)
        {
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
                    break; // 無限ループを防ぐために中断
                }
            }

            if (OnSkitEnd != null)
            {
                await OnSkitEnd.Invoke();
            }
        }

        private void CancelSkitSequence()
        {
            CurrentCancellationToken?.Cancel();
            CurrentCancellationToken = new CancellationTokenSource();
        }
    }
}