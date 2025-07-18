# 使用方法ガイド / Usage Guide

SkitSystemの詳細な使用方法とAPIリファレンスです。

This is a detailed usage guide and API reference for SkitSystem.

## 前提条件 / Prerequisites

このガイドを使用する前に、[セットアップガイド](SETUP.md)を完了してください。

Before using this guide, please complete the [Setup Guide](SETUP.md).

## 基本的な使用 / Basic Usage

### システムの初期化 / System Initialization

```csharp
using SkitSystem.Model;
using SkitSystem.Presenter;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    async void Start()
    {
        // システム初期化
        var skitSceneStarter = FindObjectOfType<SkitSceneStarter>();
        await skitSceneStarter.InitializeSkitSceneData();

        // 会話システムの初期化
        var manager = FindObjectOfType<SkitSceneManager>();
        manager.Initialize();

        // 会話実行
        var cancellationToken = this.GetCancellationTokenOnDestroy();
        await manager.ExecuteAsync(cancellationToken);
    }
}
```

### 会話の開始と制御 / Starting and Controlling Conversations

```csharp
public class ConversationController : MonoBehaviour
{
    [SerializeField] private SkitSceneManager skitSceneManager;
    
    // 会話を開始
    public async UniTask StartConversation()
    {
        await skitSceneManager.ExecuteAsync(this.GetCancellationTokenOnDestroy());
    }
    
    // 会話を一時停止
    public void PauseConversation()
    {
        // 現在の実装では自動的にタップ待ちで一時停止
    }
    
    // 会話をスキップ
    public void SkipConversation()
    {
        // InputSystemによるタップでスキップ可能
    }
}
```

## フラグ管理 / Flag Management

### 基本的なフラグ操作 / Basic Flag Operations

```csharp
using SkitSystem.Model.SkitSceneData;

public class FlagController : MonoBehaviour
{
    private SkitSceneDataContainer dataContainer;
    private FlagData flagData;
    
    void Start()
    {
        dataContainer = FindObjectOfType<SkitSceneDataContainer>();
        flagData = dataContainer.GetSkitSceneData<FlagData>();
    }
    
    // 排他的フラグ設定（他のフラグは自動で無効化）
    public void SetFlag(string flagName)
    {
        flagData.SetActiveFlag(flagName);
        Debug.Log($"フラグ '{flagName}' をアクティブに設定");
    }
    
    // 現在のアクティブフラグを取得
    public string GetActiveFlag()
    {
        var activeFlag = flagData.GetActiveFlag();
        Debug.Log($"現在のアクティブフラグ: {activeFlag}");
        return activeFlag;
    }
    
    // 特定のフラグがアクティブかチェック
    public bool IsFlagActive(string flagName)
    {
        return flagData.GetActiveFlag() == flagName;
    }
}
```

### フラグによる会話分岐 / Conversation Branching with Flags

```csharp
public class ConversationBranching : MonoBehaviour
{
    [SerializeField] private FlagController flagController;
    [SerializeField] private SkitSceneManager skitSceneManager;
    
    public async UniTask StartBranchedConversation()
    {
        // ゲーム状態に応じてフラグを設定
        if (PlayerData.HasCompletedQuest)
        {
            flagController.SetFlag("quest_completed");
        }
        else
        {
            flagController.SetFlag("quest_in_progress");
        }
        
        // フラグに基づいた会話を実行
        await skitSceneManager.ExecuteAsync(this.GetCancellationTokenOnDestroy());
    }
}
```

## タグ処理システム / Tag Processing System

### タグハンドラーの登録 / Registering Tag Handlers

```csharp
using SkitSystem.Model.SkitDataTagHandler;

public class TagSystemSetup : MonoBehaviour
{
    void Start()
    {
        var dataContainer = FindObjectOfType<SkitSceneDataContainer>();
        var tagProcessor = dataContainer.GetSkitSceneData<SkitDataTagProcessor>();
        
        // フラグ設定用のタグハンドラーを登録
        var setFlagHandler = new SetFlagTagHandler();
        tagProcessor.RegisterHandler(setFlagHandler);
        
        Debug.Log("タグハンドラーを登録しました");
    }
}
```

### カスタムタグハンドラーの作成 / Creating Custom Tag Handlers

```csharp
using SkitSystem.Model.SkitDataTagHandler;

public class CustomEffectTagHandler : ISkitDataTagHandler
{
    public string TagName => "effect";
    
    public void HandleTag(string tagValue, SkitSceneDataContainer dataContainer)
    {
        switch (tagValue)
        {
            case "shake":
                StartCameraShake();
                break;
            case "flash":
                StartScreenFlash();
                break;
            default:
                Debug.LogWarning($"未知のエフェクト: {tagValue}");
                break;
        }
    }
    
    private void StartCameraShake()
    {
        // カメラシェイク処理
        Debug.Log("カメラシェイクを開始");
    }
    
    private void StartScreenFlash()
    {
        // 画面フラッシュ処理
        Debug.Log("画面フラッシュを開始");
    }
}
```

### タグの動的処理 / Dynamic Tag Processing

```csharp
public class DynamicTagProcessor : MonoBehaviour
{
    private SkitDataTagProcessor tagProcessor;
    
    void Start()
    {
        var dataContainer = FindObjectOfType<SkitSceneDataContainer>();
        tagProcessor = dataContainer.GetSkitSceneData<SkitDataTagProcessor>();
    }
    
    // ゲームイベントに応じてタグを処理
    public void ProcessGameEvent(string eventType, string eventValue)
    {
        tagProcessor.ProcessTag(eventType, eventValue);
        Debug.Log($"イベント処理: {eventType} = {eventValue}");
    }
}
```

## シーン終了処理 / Scene Exit Processing

### 基本的なシーン終了 / Basic Scene Exit

```csharp
using SkitSystem.Model.SkitSceneExecutor;

public class SceneExitController : MonoBehaviour
{
    public void ExitSkitScene()
    {
        var dataContainer = FindObjectOfType<SkitSceneDataContainer>();
        var skitSceneExiter = new SkitSceneExiter();
        
        // シーン終了処理を実行
        skitSceneExiter.FinalizeSkitScene(dataContainer);
        
        Debug.Log("SkitSceneを終了しました");
    }
}
```

### カスタム終了処理 / Custom Exit Processing

```csharp
public class CustomSceneExit : MonoBehaviour
{
    public async UniTask ExitWithSave()
    {
        // セーブ処理
        await SaveGameData();
        
        // SkitScene終了処理
        var dataContainer = FindObjectOfType<SkitSceneDataContainer>();
        var skitSceneExiter = new SkitSceneExiter();
        skitSceneExiter.FinalizeSkitScene(dataContainer);
        
        // 次のシーンに遷移
        await LoadNextScene();
    }
    
    private async UniTask SaveGameData()
    {
        // セーブ処理の実装
        await UniTask.Delay(1000); // 例：1秒の処理時間
        Debug.Log("ゲームデータを保存しました");
    }
    
    private async UniTask LoadNextScene()
    {
        // シーン遷移の実装
        await UniTask.Delay(500);
        Debug.Log("次のシーンに遷移しました");
    }
}
```

## ログ機能 / Log System

### ログ表示の制御 / Log Display Control

```csharp
using SkitSystem.View;

public class LogController : MonoBehaviour
{
    private SkitSceneLogViewer logViewer;
    
    void Start()
    {
        logViewer = FindObjectOfType<SkitSceneLogViewer>();
    }
    
    // ログ表示の切り替え
    public void ToggleLog()
    {
        logViewer.ToggleLogDisplay();
    }
    
    // ログを強制表示
    public void ShowLog()
    {
        // logViewer.ShowLog(); // 必要に応じて実装
    }
    
    // ログを強制非表示
    public void HideLog()
    {
        // logViewer.HideLog(); // 必要に応じて実装
    }
}
```

### ログデータの管理 / Log Data Management

```csharp
public class LogDataManager : MonoBehaviour
{
    // ログの最大保持数を設定
    public void SetMaxLogCount(int maxCount)
    {
        // ログの最大保持数を設定する処理
        Debug.Log($"ログの最大保持数を {maxCount} に設定");
    }
    
    // ログをクリア
    public void ClearLog()
    {
        // ログクリア処理
        Debug.Log("ログをクリアしました");
    }
}
```

## データローディング / Data Loading

### CSV データの動的ロード / Dynamic CSV Data Loading

```csharp
using SkitSystem.Common;

public class DynamicDataLoader : MonoBehaviour
{
    public async UniTask LoadConversationData(string csvPath)
    {
        var dataContainer = FindObjectOfType<SkitSceneDataContainer>();
        var csvLoader = new CsvLoader();
        
        // CSVデータを動的にロード
        await csvLoader.LoadCsvDataAsync(csvPath, dataContainer);
        
        Debug.Log($"CSVデータを読み込みました: {csvPath}");
    }
}
```

### Addressable アセットの動的ロード / Dynamic Addressable Asset Loading

```csharp
using UnityEngine.AddressableAssets;

public class AddressableDataLoader : MonoBehaviour
{
    public async UniTask LoadAddressableData(string addressableKey)
    {
        try
        {
            // Addressableアセットをロード
            var handle = Addressables.LoadAssetsAsync<ScriptableObject>(
                addressableKey, 
                null
            );
            
            var assets = await handle.ToUniTask();
            
            // ロードしたデータを処理
            foreach (var asset in assets)
            {
                Debug.Log($"Addressableアセットを読み込み: {asset.name}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Addressableロードエラー: {e.Message}");
        }
    }
}
```

## 多言語対応 / Multi-Language Support

### 言語切り替え / Language Switching

```csharp
public class LanguageController : MonoBehaviour
{
    public enum Language
    {
        Japanese,
        English
    }
    
    [SerializeField] private Language currentLanguage = Language.Japanese;
    
    public void SwitchLanguage(Language newLanguage)
    {
        currentLanguage = newLanguage;
        
        // 言語設定を保存
        PlayerPrefs.SetInt("Language", (int)newLanguage);
        
        // UIの更新など必要な処理
        UpdateUIForLanguage(newLanguage);
        
        Debug.Log($"言語を切り替えました: {newLanguage}");
    }
    
    private void UpdateUIForLanguage(Language language)
    {
        // 言語に応じたUI更新処理
        switch (language)
        {
            case Language.Japanese:
                // 日本語UI設定
                break;
            case Language.English:
                // 英語UI設定
                break;
        }
    }
}
```

## エラーハンドリング / Error Handling

### 基本的なエラーハンドリング / Basic Error Handling

```csharp
public class ErrorHandler : MonoBehaviour
{
    public async UniTask SafeExecuteConversation()
    {
        try
        {
            var skitSceneManager = FindObjectOfType<SkitSceneManager>();
            await skitSceneManager.ExecuteAsync(this.GetCancellationTokenOnDestroy());
        }
        catch (System.OperationCanceledException)
        {
            Debug.Log("会話がキャンセルされました");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"会話実行中にエラーが発生: {e.Message}");
            // エラー時の回復処理
            await HandleConversationError();
        }
    }
    
    private async UniTask HandleConversationError()
    {
        // エラー回復処理
        Debug.Log("エラー回復処理を実行中...");
        await UniTask.Delay(1000);
    }
}
```

## パフォーマンス最適化 / Performance Optimization

### メモリ管理 / Memory Management

```csharp
public class MemoryManager : MonoBehaviour
{
    void OnDestroy()
    {
        // リソースの適切な解放
        ClearCachedData();
    }
    
    private void ClearCachedData()
    {
        // キャッシュされたデータをクリア
        Debug.Log("キャッシュデータをクリアしました");
    }
    
    // 定期的なガベージコレクション
    public void ForceGarbageCollection()
    {
        System.GC.Collect();
        Debug.Log("ガベージコレクションを実行しました");
    }
}
```

## 高度な使用例 / Advanced Usage Examples

### カスタム会話システム / Custom Conversation System

```csharp
public class CustomConversationSystem : MonoBehaviour
{
    [SerializeField] private SkitSceneManager skitSceneManager;
    [SerializeField] private FlagController flagController;
    
    public async UniTask StartCustomConversation(string conversationId)
    {
        // カスタムロジックを実行
        await ExecutePreConversationLogic(conversationId);
        
        // 会話を実行
        await skitSceneManager.ExecuteAsync(this.GetCancellationTokenOnDestroy());
        
        // ポスト会話処理
        await ExecutePostConversationLogic(conversationId);
    }
    
    private async UniTask ExecutePreConversationLogic(string conversationId)
    {
        // 会話前の処理
        Debug.Log($"会話前処理を実行: {conversationId}");
        await UniTask.Delay(100);
    }
    
    private async UniTask ExecutePostConversationLogic(string conversationId)
    {
        // 会話後の処理
        Debug.Log($"会話後処理を実行: {conversationId}");
        await UniTask.Delay(100);
    }
}
```

## デバッグとテスト / Debugging and Testing

### デバッグ用ヘルパー / Debug Helpers

```csharp
public class DebugHelper : MonoBehaviour
{
    [SerializeField] private bool enableDebugMode = false;
    
    void Update()
    {
        if (!enableDebugMode) return;
        
        // デバッグ用のキーボード入力
        if (Input.GetKeyDown(KeyCode.F1))
        {
            DebugLogCurrentState();
        }
        
        if (Input.GetKeyDown(KeyCode.F2))
        {
            DebugSkipConversation();
        }
    }
    
    private void DebugLogCurrentState()
    {
        var dataContainer = FindObjectOfType<SkitSceneDataContainer>();
        var flagData = dataContainer.GetSkitSceneData<FlagData>();
        Debug.Log($"現在のフラグ: {flagData.GetActiveFlag()}");
    }
    
    private void DebugSkipConversation()
    {
        // 会話スキップ処理
        Debug.Log("デバッグ: 会話をスキップ");
    }
}
```

---

[← README に戻る / Back to README](README.md)
[🔧 セットアップガイド / Setup Guide](SETUP.md)