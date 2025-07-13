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
    /// 会話シーンの進行を処理するクラス
    /// </summary>
    public class SkitSceneManager : MonoBehaviour
    {
        [SerializeField] private SkitSceneDataContainer _skitSceneDataContainer;
        
        private readonly Queue<SkitSceneDataAbstractBase> _skitContextQueue = new();
        private readonly List<SkitSceneExecutorBase> _skitContextHandlers = new();
        
        public CancellationTokenSource CurrentCancellationToken { get; private set; }
        public event Func<UniTask> OnSkitEnd;

        /// <summary>
        /// 実際に処理を回す際に使うデータを設定する。現在のフラグ状態に応じて、スキットシーンデータを取得する。
        /// </summary>
        public void OnStartSkitScene()
        {
            string currentFlag = null;
            if (_skitSceneDataContainer.SkitSceneData.TryGetValue(nameof(FlagData), out var flagDataList))
            {
                var flags = flagDataList
                    .OfType<FlagData>()
                    .ToList();
                
                currentFlag = flags
                    .FirstOrDefault(flag => flag.Flags.ContainsValue(true))?.CurrentFlag;
            }
            
            if (_skitSceneDataContainer.SkitSceneData.TryGetValue(nameof(ConversationGroupData), out var conversationGroupData))
            {
                var conversationData = conversationGroupData.FirstOrDefault(x => x is ConversationGroupData currentFlagConversationGroupData &&
                                                                                 currentFlagConversationGroupData.Flag == currentFlag);
                
                if (conversationData != null)
                {
                    _skitContextQueue.Enqueue(conversationData);
                }
            }
        }

        public async UniTask DoSkitSequence()
        {
            CancelSkitSequence();
            while (_skitContextQueue.Count > 0)
            {
                var currentSkitContext = _skitContextQueue.Peek();
                if (currentSkitContext == null)
                {
                    Debug.LogError("SkitContextがnullです");
                    _skitContextQueue.Dequeue(); // null要素を削除してスキップ
                    continue;
                }

                var handleSkitContextType = nameof(currentSkitContext);
                foreach (var skitContextHandler in _skitContextHandlers.Where(skitContextHandler =>
                             skitContextHandler.HandleSkitContextType == handleSkitContextType))
                {
                    // 現在のコンテキストを処理し、デキュー
                    await skitContextHandler.HandleSkitSceneData(_skitContextQueue.Dequeue(),
                        CurrentCancellationToken.Token);

                    // 次のスキットコンテキストがある場合、エンキュー
                    if (skitContextHandler.TrtGetNextSkitSceneData(out var nextSkitContextQueue))
                    {
                        _skitContextQueue.Enqueue(nextSkitContextQueue);
                    }
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
            _skitContextHandlers.ToList().ForEach(handler => handler.Dispose());
            CurrentCancellationToken = new CancellationTokenSource();
        }
    }
}