using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SkitSystem.Common;
using SkitSystem.Model;
using SkitSystem.View;
using UnityEngine;
using R3;
using UnityEngine.InputSystem;

namespace SkitSystem
{
    public class SkitScenePresenter : MonoBehaviour
    {
        [SerializeField] private SkitSceneStarter _skitSceneStarter;
        [SerializeField] private ConversationView _conversationView; 
        [SerializeField] private SkitSceneManager _skitSceneManager;
        private SkitSceneInput _skitSceneInput;

        private void OnEnable()
        {
            _skitSceneInput = new SkitSceneInput();
            _skitSceneInput.Enable();
        }

        private void Start()
        {
            InitializeAsync().Forget();
        }

        /// <summary>
        /// Skitシーンを動かす
        /// 1. SkitSceneStarterを初期化して、スキットデータをロード。SkitSceneDataContainerにスキットデータを格納する。（シーン遷移2回目以降は行われない）
        /// 2．Executorの初期化、処理を非同期で開始する。
        /// 処理の流れ
        /// 1．現在のフラグ状態を見る。
        /// 2．スキットシーンデータコンテナから、現在のフラグ状態に対応するスキットシーンデータを取得する。
        /// ある場合
        /// 複数ある場合はその旨をログに出しつつ、先に見つかったほうを使用する。
        /// ない場合
        /// ConversationDataの1番目のデータを使用する。
        /// </summary>
        private async UniTask InitializeAsync()
        {
            // modelの初期化
            await _skitSceneStarter.InitializeSkitSceneData();
            _skitSceneManager.Initialize();
            
            //viewの初期化
            foreach (var handler in _skitSceneManager.SkitContextHandlers)
            {
                if (handler is ConversationExecutor conversationExecutor)
                {
                    conversationExecutor.CurrentConversationData.Subscribe(async x =>
                    {
                        if (x == null) return;
                        await _conversationView.ShowConversation(x.TalkerName, x.Dialogue,
                            _skitSceneManager.CurrentCancellationToken.Token);
                    });
                }
            }

            _skitSceneInput.SkitSceneInputMap.Tap.performed += _ =>
            {
                if (_conversationView.IsDisplaying)
                {
                    _conversationView.ForceShowText();
                }
                else
                {
                    foreach (var handler in _skitSceneManager.SkitContextHandlers)
                    {
                        if (handler is ConversationExecutor conversationExecutor)
                        {
                            conversationExecutor.AwaitForInput.TrySetResult("Input received");
                        }
                    }
                }
                
                Debug.Log("タップ入力");
            };
            
            // スキットシーンの実行を開始
            await _skitSceneManager.DoSkitSequence();
        }
        
        private void OnDisable()
        {
            _skitSceneInput?.Dispose();
            _skitSceneInput = null;
        }
    }
}