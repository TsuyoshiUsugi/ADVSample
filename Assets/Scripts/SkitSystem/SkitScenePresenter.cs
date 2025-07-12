using Cysharp.Threading.Tasks;
using SkitSystem.Model;
using UnityEngine;

namespace SkitSystem
{
    public class SkitScenePresenter : MonoBehaviour
    {
        private void Start()
        {
            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            await UniTask.WaitUntil(() => SkitSceneStarter.Instance.IsInitialized, cancellationToken: destroyCancellationToken);
            
            // SkitDataFactoryManagerからスキットデータファクトリを取得,次の順番でロードするデータを決める
            // 現在のフラグを見てマッチするものがないか探す。
            // マッチするものがなければ、SkitDataFactoryManagerのSkitDataFactoriesの”会話データ”のSkitDataListから最初のものを取得
            // マッチするものがあれば、SkitSceneStarterのSkitDataFactoriesのマッチしたもののSkitDataListから最初のものを取得
        }
    }
}
