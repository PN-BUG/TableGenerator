#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// ═══════════════════════════════════════════════════════════════
///  表格生成器（网页版启动器）
/// ═══════════════════════════════════════════════════════════════
///
///  轻量包装窗口，本身不绘制 UI，仅用于：
///  1. 携带 [ToolInfo] 特性，让 UnityToolsHub 自动发现并归类到「数据工具」
///  2. 打开时用系统默认浏览器启动 index.html
///
///  HTML 文件位置：跟随脚本目录 Editor/TableGenerator/index.html
///
/// ═══════════════════════════════════════════════════════════════
/// </summary>
[ToolInfo("表格生成器", "数据工具",
    Description = "网页版表格生成器，通过 Copilot AI 将文本解析为结构化表格。\n\n• 支持剧本对话、选项配置、战斗节点等多种预设\n• 内置脚本解析引擎\n• 多 Sheet 支持\n• 枚举校验与命令参数提示\n• 导出 CSV / JSON\n• 从剪贴板粘贴 JSON 数据",
    Icon = "📊",
    Tags = new[] { "表格", "数据", "配表", "对话", "剧本", "copilot", "AI" },
    Shortcut = "Ctrl+Shift+Alt+T",
    Priority = 20)]
public class TableGeneratorLauncher : EditorWindow
{
    [MenuItem("UnityToolsHub/表格生成器", priority = 120)]
    public static void ShowWindow()
    {
        OpenInBrowser();
    }

    /// <summary>Hub 点击「打开」按钮时调用：直接启动浏览器</summary>
    private void CreateGUI()
    {
        // 包装窗口本身不显示 UI，直接打开浏览器并关闭自身
        OpenInBrowser();
        Close();
    }

    /// <summary>获取 HTML 文件的路径（基于脚本自身路径动态定位）</summary>
    private static string GetHtmlFilePath()
    {
        // 通过 MonoScript 反查脚本在 Assets 中的路径，
        // 再拼接同目录下的 HTML 文件
        var monoScript = MonoScript.FromScriptableObject(CreateInstance<TableGeneratorLauncher>());
        if (monoScript != null)
        {
            string scriptPath = AssetDatabase.GetAssetPath(monoScript);
            if (!string.IsNullOrEmpty(scriptPath))
            {
                string scriptDir = Path.GetDirectoryName(scriptPath);
                string htmlPath = Path.Combine(scriptDir, "index.html");
                if (File.Exists(htmlPath))
                    return Path.GetFullPath(htmlPath);
            }
        }

        // 回退：搜索项目中所有同名 HTML 文件
        string[] guids = AssetDatabase.FindAssets("index t:TextAsset", new[] { "Assets/Editor/TableGenerator" });
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (assetPath.EndsWith("index.html") && assetPath.Contains("TableGenerator"))
                return Path.GetFullPath(assetPath);
        }

        return string.Empty;
    }

    /// <summary>用系统默认浏览器打开 HTML 文件</summary>
    private static void OpenInBrowser()
    {
        var fullPath = GetHtmlFilePath();
        if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath))
        {
            EditorUtility.DisplayDialog("错误",
                "找不到表格生成器 HTML 文件（index.html）。\n\n" +
                "请确认 index.html 与 TableGeneratorLauncher.cs 在同一目录下。", "确定");
            Debug.LogError("[TableGenerator] 找不到 index.html，请确认它与启动器脚本在同一目录。");
            return;
        }

        // 转为 file:// URI（正斜杠，URL 编码）
        var uri = "file:///" + fullPath.Replace('\\', '/');

        try
        {
            // 用系统默认程序打开（Windows 上会调用默认浏览器）
            Process.Start(new ProcessStartInfo
            {
                FileName = uri,
                UseShellExecute = true,
            });
            Debug.Log($"[TableGenerator] 已在浏览器中打开：{uri}");
        }
        catch (System.Exception e)
        {
            // 回退方案：用 Application.OpenURL
            Application.OpenURL(uri);
            Debug.LogWarning($"[TableGenerator] Process.Start 失败，已回退到 Application.OpenURL：{e.Message}");
        }
    }
}
#endif
