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

1. このリポジトリをクローンまたはダウンロード
2. `Assets/Scripts/SkitSystem` フォルダをUnityプロジェクトにコピー
3. `Assets/Prefab` フォルダもコピー

1. Clone or download this repository
2. Copy `Assets/Scripts/SkitSystem` folder to your Unity project
3. Also copy `Assets/Prefab` folder

## ステップ4: プレハブの配置 / Step 4: Place Prefabs

### SkitSceneStarterの配置 / Place SkitSceneStarter

1. `Assets/Prefab/SkitSceneStarter.prefab` をシーンにドラッグ
2. インスペクターで以下を設定：
   - **Data Source Type**: CSV または Addressable を選択
   - **CSV Path**: CSVファイルのパス（CSV選択時）
   - **Addressable Key**: アドレサブルキー（Addressable選択時）

1. Drag `Assets/Prefab/SkitSceneStarter.prefab` to your scene
2. Configure in Inspector:
   - **Data Source Type**: Select CSV or Addressable
   - **CSV Path**: Path to CSV file (when CSV selected)
   - **Addressable Key**: Addressable key (when Addressable selected)

### 会話UIの配置 / Place Conversation UI

1. `Assets/Prefab/ConversationCanvas.prefab` をシーンに配置
2. 必要に応じてUIレイアウトを調整

1. Place `Assets/Prefab/ConversationCanvas.prefab` in scene
2. Adjust UI layout as needed

## ステップ5: 会話データの準備 / Step 5: Prepare Conversation Data

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
- **speaker**: 話者名 / Speaker name
- **text_ja**: 日本語テキスト / Japanese text
- **text_en**: 英語テキスト / English text
- **background**: 背景画像名 / Background image name
- **chara_name**: キャラクター名 / Character name
- **chara_emotion**: 感情 / Emotion
- **chara_position**: 位置 / Position
- **flag**: フラグ名 / Flag name

### Addressable Assets（推奨）/ Addressable Assets (Recommended)

1. Window > Asset Management > Addressables > Groups を開く
2. 会話データアセットを作成
3. Addressableグループに追加
4. 適切なキーを設定

1. Open Window > Asset Management > Addressables > Groups
2. Create conversation data assets
3. Add to Addressable groups
4. Set appropriate keys

## ステップ6: 基本設定 / Step 6: Basic Configuration

### Input Systemの設定 / Input System Configuration

1. Edit > Project Settings > XR Plug-in Management > Input System Package
2. 「Both」または「Input System Package (New)」を選択
3. Unityを再起動

1. Edit > Project Settings > XR Plug-in Management > Input System Package
2. Select "Both" or "Input System Package (New)"
3. Restart Unity

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

## トラブルシューティング / Troubleshooting

### よくある問題 / Common Issues

#### パッケージのインポートエラー / Package Import Error
- Unity バージョンが要件を満たしているか確認
- Git URLが正しいか確認
- Check Unity version meets requirements
- Verify Git URLs are correct

#### 会話データが読み込まれない / Conversation Data Not Loading
- CSVファイルのパスが正しいか確認
- CSVファイルの形式が正しいか確認
- Addressableのキーが正しく設定されているか確認
- Verify CSV file path is correct
- Check CSV file format is correct
- Confirm Addressable keys are properly set

#### UIが表示されない / UI Not Displaying
- ConversationCanvasプレハブが配置されているか確認
- Canvasの表示順序を確認
- Check ConversationCanvas prefab is placed
- Verify Canvas sorting order

#### 入力が反応しない / Input Not Responding
- Input Systemの設定を確認
- EventSystemがシーンに存在するか確認
- Check Input System configuration
- Verify EventSystem exists in scene

### サポート / Support

問題が解決しない場合は、以下の情報を含めてIssueを作成してください：

If issues persist, create an Issue with the following information:

- Unity バージョン / Unity version
- エラーメッセージ / Error messages
- 実行環境 / Runtime environment
- 再現手順 / Steps to reproduce

---

[← README に戻る / Back to README](README.md)