# SkitProject - 会話システム / Conversation System

Unity向けの高機能な会話・対話システムです。ビジュアルノベルやストーリー重視のゲームでの使用を想定しています。

A sophisticated conversation and dialogue system for Unity, designed for visual novels and story-driven games.

## セットアップ / Setup

詳細なセットアップ手順については以下をご覧ください：

For detailed setup instructions, please see:

**[📋 セットアップガイド / Setup Guide](SETUP.md)**

## 使用方法 / Usage

詳細な使用方法とAPIリファレンスについては以下をご覧ください：

For detailed usage instructions and API reference, please see:

**[📚 使用方法ガイド / Usage Guide](USAGE.md)**

## 主な機能 / Key Features

### 🌐 多言語対応 / Multi-Language Support
- 日本語、英語に対応
- 言語別の会話データ管理
- Support for Japanese and English
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
- **NuGet for Unity**: .NETパッケージ統合 / .NET package integration


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

