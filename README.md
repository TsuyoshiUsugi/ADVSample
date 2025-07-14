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
- Exclusive flag system (only one flag active at a time)
- Branching dialogue based on game state

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
- Character-by-character text display
- Tap to instantly show full text

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
- `SkitSceneManager`: 会話シーケンスの統括管理
- `ConversationExecutor`: 会話データの実行処理
- `SkitSceneExecutorBase`: 実行処理の基底クラス

#### プレゼンテーション / Presentation
- `SkitScenePresenter`: メインプレゼンター
- `ConversationDialogView`: 会話UI表示
- `ConversationCharaImageAndBackgroundView`: キャラクター・背景表示
- `SkitSceneFader`: 画面フェード処理
- `SkitSceneStarter`: システム初期化

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
2. 必要なパッケージをインストール / Install required packages
3. `SkitSceneStarter`プレハブをシーンに配置 / Place `SkitSceneStarter` prefab in scene
4. 会話データCSVを準備 / Prepare conversation data CSV
5. データローダーを設定 / Configure data loader

## 使用方法 / Usage

### 基本的な使用 / Basic Usage

```csharp
// システム初期化
var skitSceneStarter = FindObjectOfType<SkitSceneStarter>();
await skitSceneStarter.StartAsync();

// 会話開始
var manager = FindObjectOfType<SkitSceneManager>();
await manager.StartConversationAsync(conversationId);
```

### フラグ管理 / Flag Management

```csharp
// フラグ設定
flagData.SetFlag("flag_name", true);

// フラグに基づく会話選択
var conversation = manager.GetConversationByFlag("flag_name");
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
│       │   └── SkitSceneData/    # 会話データ構造
│       ├── Presenter/            # プレゼンター
│       └── View/                 # UI表示・フェード処理
├── Prefab/                       # プレハブ
├── Scenes/                       # サンプルシーン
└── SkitScenData/                 # 会話データ
```

## 開発メモ / Development Notes

### 最近の更新 / Recent Updates
- 画面遷移時のFade処理を実装
- キャラクター画像の動的読み込み機能を追加
- 背景画像の自動切り替え機能を実装
- 画像アドレス付与機能を追加
- 会話再生機能の実装
- タップ進行処理の改善

### 今後の課題 / Future Tasks
- 終了時のFade処理の実装
- ログ機能の追加
- オート再生機能の実装
- パフォーマンス最適化
- エラーハンドリングの強化

