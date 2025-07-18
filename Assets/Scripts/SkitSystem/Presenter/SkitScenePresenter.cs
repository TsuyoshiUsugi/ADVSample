using Cysharp.Threading.Tasks;
using R3;
using SkitSystem.Common;
using SkitSystem.Model;
using SkitSystem.Model.SkitSceneExecutor;
using SkitSystem.View;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SkitSystem
{
    public class SkitScenePresenter : MonoBehaviour
    {
        [Header("Skitシーンのモデル")]
        [SerializeField] private SkitSceneStarter _skitSceneStarter;
        [SerializeField] private SkitSceneManager _skitSceneManager;
        [Header("Skitシーンのビュー")]
        [SerializeField] private ConversationDialogView _conversationDialogView;
        [SerializeField] private ConversationCharaImageAndBackgroundView _conversationCharaImageAndBackgroundView;
        [SerializeField] private SkitSceneFader _skitSceneFader;
        [SerializeField] private SkitSceneLogViewer _skitSceneLogViewer;
        
        private SkitSceneInput _skitSceneInput;
        private SkitSceneExiter _skitSceneExiter;

        private void Start()
        {
            InitializeAsync().Forget();
        }

        private void OnEnable()
        {
            _skitSceneInput = new SkitSceneInput();
            _skitSceneInput.Enable();
        }

        private void OnDisable()
        {
            _skitSceneInput?.Disable();
            _skitSceneInput?.Dispose();
            _skitSceneInput = null;
        }

        /// <summary>
        ///     Skitシーンを動かす
        ///     1. SkitSceneStarterを初期化して、スキットデータをロード。SkitSceneDataContainerにスキットデータを格納する。（シーン遷移2回目以降は行われない）
        ///     2．Executorの初期化、処理を非同期で開始する。
        ///     処理の流れ
        ///     1．現在のフラグ状態を見る。
        ///     2．スキットシーンデータコンテナから、現在のフラグ状態に対応するスキットシーンデータを取得する。
        ///     ある場合
        ///     複数ある場合はその旨をログに出しつつ、先に見つかったほうを使用する。
        ///     ない場合
        ///     ConversationDataの1番目のデータを使用する。
        /// </summary>
        private async UniTask InitializeAsync()
        {
            _conversationCharaImageAndBackgroundView.ResetImages();
            _skitSceneFader.ForceShowFade();
            // modelの初期化
            _skitSceneExiter = new SkitSceneExiter();
            await _skitSceneStarter.InitializeSkitSceneData();
            _skitSceneManager.Initialize();

            //会話シーンの画面表示処理の関連づけ
            foreach (var handler in _skitSceneManager.SkitContextHandlers)
            {
                if (handler is ConversationExecutor conversationExecutor)
                {
                    conversationExecutor.CurrentConversationData.Subscribe(async conversationData =>
                    {
                        if (conversationData == null) return;
                        _conversationCharaImageAndBackgroundView.ResetImages();

                        _conversationCharaImageAndBackgroundView.ShowBackground(
                            SkitSceneDataContainer.Instance.GetSpriteByFileName(conversationData.BackgroundImageName));
                        foreach (var showCharaData in conversationData.ShowCharaDataList)
                        {
                            _conversationCharaImageAndBackgroundView.ShowCharacter(
                                SkitSceneDataContainer.Instance.GetCharaSpriteByEmotion(showCharaData.CharaName,
                                    showCharaData.CharaEmote),
                                showCharaData.StandPos);
                        }

                        if (_skitSceneFader.IsFadeImageActive)
                        {
                            await _skitSceneFader.FadeInAsync(_skitSceneManager.CurrentCancellationToken.Token);
                        }
                        //文字送り
                        await _conversationDialogView.ShowConversation(conversationData.TalkerName,
                            conversationData.Dialogue,
                            _skitSceneManager.CurrentCancellationToken.Token);
                    });
                    
                    //会話シーンのログビューアーの設定
                    conversationExecutor.CurrentConversationData.Subscribe( conversationData =>
                    {
                        if (conversationData == null) return;
                        _skitSceneLogViewer.SetLog(conversationData.TalkerName, conversationData.Dialogue);
                    });
                }
            }
            

            //マウスクリック等されたとき
            _skitSceneInput.SkitSceneInputMap.Tap.performed += _ =>
            {
                
                if (_skitSceneFader.IsFadeImageActive)
                {
                    return;
                }
                
                if (EventSystem.current && EventSystem.current.currentSelectedGameObject)
                {
                    return;
                }
                if (_conversationDialogView.IsDisplaying)
                    _conversationDialogView.ForceShowText();
                else
                    foreach (var handler in _skitSceneManager.SkitContextHandlers)
                        if (handler is ConversationExecutor conversationExecutor)
                            conversationExecutor.AwaitForInput.TrySetResult("Input received");
            };
            
            //終了時の処理
            _skitSceneManager.OnSkitEnd += async () =>
            {
                // スキットシーンが終了したらフェードアウト
                await _skitSceneFader.FadeOutAsync(_skitSceneManager.CurrentCancellationToken.Token);
                // スキットシーンのビューを非表示にする
                _skitSceneExiter.FinalizeSkitScene(SkitSceneDataContainer.Instance);
            };
            
            // スキットシーンの実行を開始
            await _skitSceneManager.DoSkitSequence();
        }
    }
}