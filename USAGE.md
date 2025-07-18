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
        await manager.DoSkitSequence();
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

[← README に戻る / Back to README](README.md)
[🔧 セットアップガイド / Setup Guide](SETUP.md)
