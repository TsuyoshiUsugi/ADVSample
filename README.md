# SkitProject - 会話システム / Conversation System

Unity向けの高機能な会話・対話システムです。ビジュアルノベルやストーリー重視のゲームでの使用を想定しています。

A sophisticated conversation and dialogue system for Unity, designed for visual novels and story-driven games.

## 主な機能 / Key Features

### 🌐 多言語対応 / Multi-Language Support
- 日本語、英語、中国語、韓国語に対応
- 言語別の会話データ管理
- Support for Japanese, English, Chinese, and Korean
- Language-specific dialogue data management

### 🎯 フラグベースの会話制御 / Flag-Based Dialogue Flow
- 排他的フラグシステム（一度に1つのフラグのみアクティブ）
- ゲーム状態に基づく分岐会話
- タグ処理システムでフラグを動的変更
- Exclusive flag system (only one flag active at a time)
- Branching dialogue based on game state
- Dynamic flag modification via tag processing system

### 📊 柔軟なデータ読み込み / Flexible Data Loading
- Googleスプレッドシート（開発用）
- Addressable Assets（本番用）
- Google Sheets (development)
- Addressable Assets (production)

### ⚡ 非同期処理 / Asynchronous Operations
- UniTaskを使用したスムーズな処理
- キャンセル可能な操作
- Smooth operations using UniTask
- Cancellable operations

### 🎨 文字送り演出 / Text Animation
- 文字ごとの段階的表示
- タップで即座に全文表示
- ログ機能で過去の会話を管理
- Character-by-character text display
- Tap to instantly show full text
- Log system for managing conversation history

### 🎭 キャラクター・背景表示 / Character & Background Display
- キャラクター画像の動的読み込み
- 背景画像の自動切り替え
- 画面遷移時のフェード処理
- Dynamic character image loading
- Automatic background switching
- Fade transitions between scenes

## システム構成 / System Architecture

### MVP パターン / MVP Pattern
- **Model**: データ管理とビジネスロジック
- **View**: UI表示とユーザー入力
- **Presenter**: ModelとViewの橋渡し

### 主要コンポーネント / Core Components

#### データ管理 / Data Management
- `SkitSceneDataContainer`: 会話データの中央管理
- `CsvLoader`: CSV形式のデータ読み込み
- `SkitDataLoader`: ScriptableObjectベースのデータローダー

#### 実行システム / Execution System
- `SkitSceneManager`: 会話シーケンスの統括管理とキューベース実行
- `ConversationExecutor`: 会話データの実行処理
- `SkitSceneExecutorBase`: 実行処理の基底クラス
- `SkitSceneExiter`: シーン終了処理管理

#### プレゼンテーション / Presentation
- `SkitScenePresenter`: メインプレゼンター
- `ConversationDialogView`: 会話UI表示
- `ConversationCharaImageAndBackgroundView`: キャラクター・背景表示
- `SkitSceneFader`: 画面フェード処理
- `SkitSceneStarter`: システム初期化とデータロード
- `SkitSceneLogViewer`: ログ表示機能
- `LogPrefab`: ログエントリプレハブ

### データ構造 / Data Structure

#### 会話データ / Conversation Data
```csharp
ConversationData
├── Background Image
├── Speaker Name
├── Dialogue Text
└── Character Display Data
    ├── Name
    ├── Emotion
    └── Position
```

#### フラグデータ / Flag Data
```csharp
FlagData
├── Flag ID
├── Flag Name
└── Active State (exclusive)
```

## 必要な依存関係 / Dependencies

- **UniTask**: 非同期処理 / Async operations
- **R3**: リアクティブプログラミング / Reactive programming
- **Unity Addressables**: アセット管理 / Asset management
- **Unity Input System**: 入力処理 / Input handling

## セットアップ / Setup

1. Unityプロジェクトを開く / Open Unity project
2. 必要なパッケージをインストール / Install required packages:
   - UniTask (2.5.10)
   - R3 (1.3.0)
   - Unity Addressables (2.5.0)
   - Unity Input System (1.14.0)
3. `SkitSceneStarter`プレハブをシーンに配置 / Place `SkitSceneStarter` prefab in scene
4. 会話データCSVを準備 / Prepare conversation data CSV
5. `SkitDataLoader`でデータソースを設定 / Configure data sources with `SkitDataLoader`
6. タグハンドラーを登録（必要に応じて） / Register tag handlers (if needed)

## 使用方法 / Usage

### 基本的な使用 / Basic Usage

```csharp
// システム初期化
var skitSceneStarter = FindObjectOfType<SkitSceneStarter>();
await skitSceneStarter.InitializeSkitSceneData();

// 会話システムの初期化
var manager = FindObjectOfType<SkitSceneManager>();
manager.Initialize();

// 会話実行
await manager.ExecuteAsync(cancellationToken);
```

### フラグ管理 / Flag Management

```csharp
// フラグデータの取得
var flagData = skitSceneDataContainer.GetSkitSceneData<FlagData>();

// 排他的フラグ設定（他のフラグは自動で無効化）
flagData.SetActiveFlag("flag_name");

// 現在のアクティブフラグを取得
var activeFlag = flagData.GetActiveFlag();
```

### タグ処理システム / Tag Processing System

```csharp
// タグハンドラーの登録
var setTagHandler = new SetFlagTagHandler();
tagProcessor.RegisterHandler(setTagHandler);

// タグの処理
tagProcessor.ProcessTag("flag", "flag_value");
```

### シーン終了処理 / Scene Exit Processing

```csharp
// シーン終了処理
var skitSceneExiter = new SkitSceneExiter();
skitSceneExiter.FinalizeSkitScene(skitSceneDataContainer);
```

### ログ機能 / Log System

```csharp
// ログビューアーの取得
var logViewer = FindObjectOfType<SkitSceneLogViewer>();

// ログ表示の切り替え
logViewer.ToggleLogDisplay();
```

## エディター拡張 / Editor Extensions

### カスタムスクリプト作成ツール / Custom Script Creator
- `Assets/Create/Custom Script`メニューから利用可能
- 自動的な名前空間生成
- MonoBehaviourと通常クラスのテンプレート

## ファイル構成 / File Structure

```
Assets/
├── Scripts/
│   ├── Editor/                    # エディター拡張
│   │   └── CustomScriptCreator.cs
│   └── SkitSystem/               # メインシステム
│       ├── Common/               # 共通コンポーネント
│       ├── Model/                # データモデル
│       │   ├── RawSkitDataConverter/ # データ変換処理
│       │   ├── SkitDataTagHandler/   # タグ処理システム
│       │   ├── SkitSceneData/        # 会話データ構造
│       │   └── SkitSceneExecutor/    # 実行システム
│       ├── Presenter/            # プレゼンター
│       └── View/                 # UI表示・フェード処理・ログ
├── Prefab/                       # プレハブ
├── Scenes/                       # サンプルシーン
└── SkitScenData/                 # 会話データ
```

## 開発メモ / Development Notes

### 最近の更新 / Recent Updates
- タグ処理システムの追加（フラグ操作等）
- シーン終了処理クラス（SkitSceneExiter）の実装
- ログ表示機能の実装
- キューベースの実行システムで順序制御を改善
- キャラクター画像の動的読み込み機能
- 背景画像の自動切り替え機能
- 画面遷移時のFade処理

### 今後の課題 / Future Tasks
- タグ処理システムの機能拡充（フラグ以外のタグタイプ）
- ログ表示のUI改善と操作性向上
- オート再生機能の実装
- セーブ/ロード機能の追加
- パフォーマンス最適化とメモリ管理改善
- エラーハンドリングの強化

