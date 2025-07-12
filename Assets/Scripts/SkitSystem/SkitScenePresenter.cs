using System.Threading;
using Cysharp.Threading.Tasks;
using SkitSystem.Common;
using SkitSystem.Model;
using UnityEngine;

namespace SkitSystem
{
    public class SkitScenePresenter : MonoBehaviour
    {
        [SerializeField] private SkitSceneStarter _skitSceneStarter;
        [SerializeField] private SkitSceneDataContainer _skitSceneDataContainer;
        private SkitSceneExecutor _skitSceneExecutor;
        private CancellationToken _cancellationToken;

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
            _cancellationToken = this.GetCancellationTokenOnDestroy();
            await _skitSceneStarter.InitializeSkitSceneData();
            _skitSceneExecutor = new SkitSceneExecutor();
            //await _skitSceneExecutor.HandleSkitSceneData(_skitSceneDataContainer, _cancellationToken);
        }
    }
}