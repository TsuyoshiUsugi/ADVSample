# ADVゲームシステム

# 概要

ADVゲームの処理のサンプルです。ADVゲームの基本的なシステム（会話再生、画像読みこみ＆表示）を実装しております。

また会話データは共有ドライブ上のスプレッドシートorアドレッサブルで取得できるようになっており、複数人やUnity,Gitに慣れていない人でも編集しやすい仕組みになっています。


https://github.com/user-attachments/assets/df958452-95bb-4166-9bfc-e790f718021a

<details>
<summary><small>お借りした素材</small></summary>

<small>

**会話ウィンドウ、UI**: 空想曲線 | https://kopacurve.blog.fc2.com/  
**背景**: みんちりえ | https://min-chi.material.jp/  
**キャラ**: わたおきば | https://wataokiba.net/  
**フォント**: aosagi | https://ymnk-design.com/aosagi/

</small>

</details>

# 実装済み機能

ゲーム内処理

- 会話再生機能（タップで次の会話再生、会話再生中にタップで一気に表示）
- シーン遷移前後のフェード
- ログの表示
- フラグ変更タグ

エディタ

- スクリプト作成時に名前空間に合わせてコードを生成する
- スプレッドシート（CSV形式）を共有ドライブ上から取得する
- スプレッドシート（CSV形式）をアドレッサブルで取得する

# プロジェクトのセットアップ

1. Unity6000.0.51を準備して開く
2. 依存関係を自動解決（プロジェクトオープン時に自動実行）

## 依存関係
- UniTask
- R3
- Unity Addressables
- Unity Input System
- NuGet for Unity

# データの設定

## 必要なシート構成

SkitSystemでは以下の2つのシートを作成・設定する必要があります：

## 1. 会話データシート（ConversationSkitDataLoader,ConversationFlagDataLoader用）
<img width="3449" height="845" alt="d69e0032bff8198f7be5f80803b6fc72" src="https://github.com/user-attachments/assets/d7dc03d0-9c9a-4669-ad77-b1c041c719be" />

会話の内容とフラグ設定を管理するシートです。

### 必須列（Required Columns）

- **id**: 会話ID / Conversation ID
- **flag**: フラグ名 / Flag name
- **background**: 背景画像名 / Background image name
- **speaker**: 話者名 / Speaker name
- **text_ja**: 日本語テキスト / Japanese text
- **text_en**: 英語テキスト / English text
- **chara_name**: キャラクター名 / Character name
- **chara_emotion**: 感情 / Emotion
- **chara_position**: 位置 / Position

## 2. 全般設定データシート（SkitSceneGeneralSettingLoader用）
<img width="1993" height="389" alt="3e4aeac5e8c4f06245dccc788e8e98f9" src="https://github.com/user-attachments/assets/f4f81948-7a7f-435d-8f17-4a87e2a0d821" />

キャラクター画像設定や名前設定を管理するシートです。

### データ構造

**表示キャラ画像設定欄**
- 表示するキャラ名
- 各感情キーで読みこむファイル名（アドレッサブルで取得します）

**名前設定**
- 表示するキャラ名
- 英語の時の名前

# 設定したデータをUnity上で読みこめるようにする

会話シーンでは主にフラグデータ、会話データ、会話シーン設定データが必要になります。

それぞれ、スクリプタブルオブジェクトの

- ConversationFlagDataLoader
- ConversationSkitDataLoader
- SkitSceneGeneralSettingLoader

で読みこんでいます。

これらの

- スプシのアドレス
- ローカルのアドレッサブルのアドレス

を設定することでデータを読みこめるようになります。

スプシのアドレスは該当のシートを開き、左上の

ファイル＞共有＞Webに公開  
<img width="726" height="857" alt="82a333c2375858544a03e98f101d1383" src="https://github.com/user-attachments/assets/4e65951b-c8fd-4068-8c33-6eace1ea644e" />  

から取得できます。

アドレッサブルアセットは該当のCSVファイルをAddressable Groups ウィンドウで登録することで可能です。

# 独自のCSVデータをロードし、それに合わせた処理をする

まずCSVデータを変換する処理を作成します。
`IRawSkitDataConverter`を継承し、各カラムを読みこみ、`List<SkitSceneDataAbstractBase>`を返す処理を作成します。

```csharp
// IRawSkitDataConverter.cs
namespace SkitSystem.Model.RawSkitDataConverter
{
    [Serializable]
    public abstract class IRawSkitDataConverter
    {
        public abstract string ConvertDataType { get; }

        /// <summary>
        ///     スキットデータを変換するインターフェース。
        /// </summary>
        /// <param name="rawData">生データ</param>
        /// <returns>変換後のデータ</returns>
        public abstract List<SkitSceneDataAbstractBase> Convert(List<string[]> rawData);
    }
}
```

`SkitSceneDataAbstractBase`は返すデータを定義するクラスです。

```csharp
// SkitSceneDataAbstractBase.cs
namespace SkitSystem.Model
{
    /// <summary>
    ///     スキットシーンで使われる会話データの基底
    /// </summary>
    public class SkitSceneDataAbstractBase
    {
    }
}
```

次に実際の会話シーン内で定義したデータを処理するクラスを作成します。
`SkitSceneExecutorBase`を継承し、`HandleSkitSceneData`で処理する仕組みを定義してください。描画や入力処理が絡む場合は現在処理しているデータをリアクティブで公開し、Presenterでつなぎこむことをお勧めします。

```csharp
// SkitSceneExecutorBase.cs
namespace SkitSystem.Model
{
    public abstract class SkitSceneExecutorBase
    {
        public abstract string HandleSkitContextType { get; }
        public UniTaskCompletionSource<string> AwaitForInput { get; protected set; } = new();

        public abstract UniTask HandleSkitSceneData(SkitSceneDataAbstractBase skitSceneData, CancellationToken token);

        public abstract bool TrtGetNextSkitSceneData(out SkitSceneDataAbstractBase nextSkitContextQueue);
    }
}
```

また入力を待つ際は待機用の`UniTaskCompletionSource` を公開するのもおすすめです。

詳しくは`ConversationExecutor`などを参考にしてください。

```csharp
// ConversationExecutor.cs（抜粋）
public class ConversationExecutor : SkitSceneExecutorBase
{
    private readonly ReactiveProperty<ConversationData> _currentConversationData = new();
    
    public override string HandleSkitContextType => nameof(ConversationGroupData);
    public ReadOnlyReactiveProperty<ConversationData> CurrentConversationData => _currentConversationData;

    public override async UniTask HandleSkitSceneData(SkitSceneDataAbstractBase skitSceneData, CancellationToken token)
    {
        if (skitSceneData is not ConversationGroupData currentConversationGroup) return;

        foreach (var conversation in currentConversationGroup.ConversationData)
        {
            _currentConversationData.Value = conversation;
            await AwaitForInput.Task;
            AwaitForInput = new UniTaskCompletionSource<string>();
        }
    }
}
```
