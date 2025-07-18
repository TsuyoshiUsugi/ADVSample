# セットアップガイド / Setup Guide

SkitSystemを使用するための詳細なセットアップ手順です。

This is a detailed setup guide for using SkitSystem.

## 前提条件 / Prerequisites

- Unity 6000.0.51f1 以上
- Git（パッケージ管理用）
- Unity 6000.0.51f1 or higher
- Git (for package management)

## ステップ1: Unityプロジェクトの準備 / Step 1: Unity Project Setup

1. 新しいUnityプロジェクトを作成するか、既存のプロジェクトを開きます
2. プロジェクトのテンプレートは「3D」または「2D」を選択

1. Create a new Unity project or open an existing one
2. Select "3D" or "2D" project template

## ステップ2: 必要なパッケージのインストール / Step 2: Install Required Packages

### Unity Package Managerからインストール / Install from Unity Package Manager

1. Window > Package Manager を開く
2. 以下のパッケージを検索してインストール：

1. Open Window > Package Manager
2. Search and install the following packages:

#### Unity公式パッケージ / Unity Official Packages
- **Unity Addressables** (2.5.0)
  - Package Manager > Unity Registry で検索
  - Search in Package Manager > Unity Registry
- **Unity Input System** (1.14.0)
  - Package Manager > Unity Registry で検索
  - Search in Package Manager > Unity Registry

### Git URLからインストール / Install from Git URL

Package Managerの「+」ボタン > "Add package from git URL" を選択：

Select "+" button in Package Manager > "Add package from git URL":

#### UniTask
```
https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask
```

#### R3
```
https://github.com/Cysharp/R3.git?path=src/R3.Unity/Assets/R3.Unity
```

#### NuGet for Unity
```
https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity
```

## ステップ3: SkitSystemの導入 / Step 3: Import SkitSystem
SkitSceneSampleを開く  
Open SkitSceneSample

## ステップ4: 会話データの準備 / Step 4: Prepare Conversation Data

### CSV形式のデータ / CSV Format Data

以下の列を含むCSVファイルを作成：

Create a CSV file with the following columns:

```csv
id,speaker,text_ja,text_en,background,chara_name,chara_emotion,chara_position,flag
1,ナレーター,こんにちは,Hello,bg_room,character1,happy,center,default
2,キャラ1,よろしく,Nice to meet you,bg_room,character1,smile,left,default
```

#### 必須列 / Required Columns
- **id**: 会話ID / Conversation ID
  **flag**: フラグ名 / Flag name
- **speaker**: 話者名 / Speaker name
- **text_ja**: 日本語テキスト / Japanese text
- **text_en**: 英語テキスト / English text
- **background**: 背景画像名 / Background image name
- **chara_name**: キャラクター名 / Character name
- **chara_emotion**: 感情 / Emotion
- **chara_position**: 位置 / Position

参考
<img width="3449" height="845" alt="d69e0032bff8198f7be5f80803b6fc72" src="https://github.com/user-attachments/assets/5825c77b-28c4-4197-afcd-e5e86ed21a0f" />


### Addressable Assets（推奨）/ Addressable Assets (Recommended)

1. Window > Asset Management > Addressables > Groups を開く
2. 会話データアセットを作成
3. Addressableグループに追加
4. 適切なキーを設定

1. Open Window > Asset Management > Addressables > Groups
2. Create conversation data assets
3. Add to Addressable groups
4. Set appropriate keys

### タグハンドラーの登録（オプション）/ Register Tag Handlers (Optional)

フラグ操作などの高度な機能を使用する場合：

For advanced features like flag operations:

```csharp
// SkitSceneStarterのInitialize後に実行
var tagProcessor = skitSceneDataContainer.GetSkitSceneData<SkitDataTagProcessor>();
var setFlagHandler = new SetFlagTagHandler();
tagProcessor.RegisterHandler(setFlagHandler);
```

## ステップ7: テスト実行 / Step 7: Test Run

1. Playボタンを押してシーンを実行
2. 会話が正常に表示されることを確認
3. タップで次の会話に進むことを確認

1. Press Play button to run the scene
2. Confirm conversations display correctly
3. Confirm tapping advances to next conversation

---

[← README に戻る / Back to README](README.md)
