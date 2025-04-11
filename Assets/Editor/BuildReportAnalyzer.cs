using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Linq;

public class BuildDllAnalyzer
{
    [MenuItem("Tools/Analyze DLLs in WebGL Build")]
    public static void AnalyzeDllsInBuild()
    {
        string pathToBuild = "Build/WebGL"; // Измени путь, если нужно
        string outputPath = $"{pathToBuild}/Build";

        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray(),
            locationPathName = pathToBuild,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        });

        if (report == null || report.summary.result != BuildResult.Succeeded)
        {
            Debug.LogError("❌ Build failed or report not available.");
            return;
        }

        var dlls = report.GetFiles()
            .Where(f => f.path.EndsWith(".dll"))
            .OrderByDescending(f => f.size)
            .ToList();

        Debug.Log($"📦 Found {dlls.Count} DLLs in build:");

        foreach (var dll in dlls)
        {
            Debug.Log($"{dll.path} — {(dll.size / 1024f):F1} KB");
        }
    }
}