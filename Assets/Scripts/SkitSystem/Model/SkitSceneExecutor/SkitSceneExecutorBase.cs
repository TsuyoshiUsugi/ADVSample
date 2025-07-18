using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace SkitSystem.Model
{
    /// <summary>
    ///     会話シーンの処理の流れを書くところ
    ///     会話シーンは以下のフローで行われる。
    ///     1．StarterがSkitDataLoaderContainerに登録されている全Loaderのロード処理を走らせる。
    ///     ロードするとLoaderのConvertedDataに変換後のデータが格納される。以降はこれにアクセスすればいい。
    ///     2．
    /// </summary>
    public abstract class SkitSceneExecutorBase
    {
        public abstract string HandleSkitContextType { get; }
        public UniTaskCompletionSource<string> AwaitForInput { get; protected set; } = new();

        public abstract UniTask HandleSkitSceneData(SkitSceneDataAbstractBase skitSceneData, CancellationToken token);

        public abstract bool TrtGetNextSkitSceneData(out SkitSceneDataAbstractBase nextSkitContextQueue);
    }
}