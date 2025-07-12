using UnityEditor;
using UnityEngine;
using System.IO;

public static class CustomScriptCreator
{
    private const string MonoTemplatePath = "Assets/Scripts/Editor/Templates/MonoBehaviourTemplate.txt";
    private const string EmptyTemplatePath = "Assets/Scripts/Editor/Templates/EmptyClassTemplate.txt";

    [MenuItem("Assets/Create/Clean MonoBehaviour Script", false, -11111)]
    public static void CreateMonoBehaviourScript()
    {
        CreateScriptFromTemplate("NewMonoBehaviourScript.cs", MonoTemplatePath);
    }

    [MenuItem("Assets/Create/Empty Class", false, -1111)]
    public static void CreateEmptyClass()
    {
        CreateScriptFromTemplate("NewClass.cs", EmptyTemplatePath);
    }

    private static void CreateScriptFromTemplate(string fileName, string templatePath)
    {
        string path = GetSelectedPathOrFallback();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            0,
            ScriptableObject.CreateInstance<DoCreateCustomScriptAsset>(),
            Path.Combine(path, fileName),
            null,
            templatePath
        );
    }

    private static string GetSelectedPathOrFallback()
    {
        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(assetPath))
                return Path.GetDirectoryName(assetPath);
            if (Directory.Exists(assetPath))
                return assetPath;
        }
        return "Assets";
    }
}

internal class DoCreateCustomScriptAsset : UnityEditor.ProjectWindowCallback.EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        string className = Path.GetFileNameWithoutExtension(pathName);
        string template = File.ReadAllText(resourceFile);

        string relativeDir = Path.GetDirectoryName(pathName).Replace("\\", "/");
        string namespaceName = GenerateNamespace(relativeDir);

        string content = template
            .Replace("#SCRIPTNAME#", className)
            .Replace("#NAMESPACE#", namespaceName);

        File.WriteAllText(pathName, content);
        AssetDatabase.Refresh();

        Object asset = AssetDatabase.LoadAssetAtPath<Object>(pathName);
        ProjectWindowUtil.ShowCreatedAsset(asset);
    }

    private string GenerateNamespace(string assetPath)
    {
        if (assetPath.StartsWith("Assets/"))
            assetPath = assetPath.Substring("Assets/".Length);

        // Scripts/ を削除
        if (assetPath.StartsWith("Scripts/"))
            assetPath = assetPath.Substring("Scripts/".Length);

        return string.IsNullOrEmpty(assetPath) ? "GlobalNamespace" : assetPath.Replace("/", ".");
    }
}
