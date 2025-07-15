using SkitSystem.Common;
using UnityEngine.SceneManagement;

namespace SkitSystem.Model
{
    public class SkitSceneExiter
    {
        private const string DefaultLoadNextSceneName = "SkitSceneSample";
        
        /// <summary>
        ///     スキットシーンの終了処理を行う。
        /// </summary>
        /// <param name="skitSceneData">スキットシーンデータ</param>
        public void FinalizeSkitScene(SkitSceneDataContainer container)
        {
            SceneManager.LoadScene(DefaultLoadNextSceneName);
        }
    }
}
