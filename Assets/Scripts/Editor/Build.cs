using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class MenuBuildOptions
{
    private const string buildsName = "Builds";
    private const string headlessServerName = "HeadlessServer";
    private const string localServerName = "LocalServer";
    private const string localClientName = "LocalClient";

    private static string ProjectName => Path.GetFileName(Path.GetDirectoryName(Application.dataPath));
    private static string ProjectDirectory => Path.GetDirectoryName(Application.dataPath);
    private static string ProjectParentDirectory => Path.GetDirectoryName(ProjectDirectory);
    private static string BuildsDirectory => $@"{ProjectParentDirectory}\{ProjectName}-{buildsName}";
    private static string HeadlessServerLocationPathName => $@"{BuildsDirectory}\{headlessServerName}\{headlessServerName}.exe";
    private static string HeadlessServerZipPath => $@"{ProjectParentDirectory}\{headlessServerName}.zip";

    [MenuItem("Build/Local Client %&c")]
    public static void BuildClient()
    {
        Debug.Log("Building Local Client");

        var outputLocation = $@"{BuildsDirectory}\{localClientName}\{localClientName}.exe";
        var options = new BuildPlayerOptions()
        {
            locationPathName = outputLocation,
            scenes = GetAllScenes(),
            options = BuildOptions.AllowDebugging,
            target = BuildTarget.StandaloneWindows64
        };

        var report = BuildPipeline.BuildPlayer(options);
        LogReport(report);
        OpenInWindowsExplorer(Path.GetDirectoryName(outputLocation));
    }

    [MenuItem("Build/Local Server %&s")]
    public static void BuildServer()
    {
        Debug.Log("Building local Server");

        var outputLocation = $@"{BuildsDirectory}\{localServerName}\{localServerName}.exe";
        var options = new BuildPlayerOptions()
        {
            locationPathName = outputLocation,
            options = BuildOptions.AllowDebugging,
            scenes = GetAllScenes(),
            target = BuildTarget.StandaloneWindows64,
            targetGroup = BuildTargetGroup.Standalone
        };

        var report = BuildPipeline.BuildPlayer(options);

        LogReport(report);
        OpenInWindowsExplorer(Path.GetDirectoryName(outputLocation));
    }

    [MenuItem("Build/Headless Server %&h")]
    public static void BuildHeadlessServer()
    {
        Debug.Log("Building Headless Server");

        EditorUserBuildSettings.SwitchActiveBuildTarget(NamedBuildTarget.Server, BuildTarget.StandaloneWindows64);
        PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Server, "SERVER_BUILD");
        AssetDatabase.Refresh();

        Debug.Log(HeadlessServerLocationPathName);

        var options = new BuildPlayerOptions()
        {
            locationPathName = HeadlessServerLocationPathName,
            subtarget = (int)StandaloneBuildSubtarget.Server,
            scenes = GetAllScenes(),
            target = BuildTarget.StandaloneWindows64,
            targetGroup = BuildTargetGroup.Standalone,
        };

        var report = BuildPipeline.BuildPlayer(options);

        LogReport(report);
        CreateZip();
        OpenInWindowsExplorer(Path.GetDirectoryName(HeadlessServerLocationPathName));

        EditorUserBuildSettings.SwitchActiveBuildTarget(NamedBuildTarget.Standalone, BuildTarget.StandaloneWindows64);
        PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Standalone, "CLIENT_BUILD");
        AssetDatabase.Refresh();
    }

    private static void LogReport(BuildReport report)
    {
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + report.summary.totalSize * .000001 + " MB");
        }
        if (report.summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }

    private static string[] GetAllScenes()
    {
        return EditorBuildSettings.scenes.Select(i => i.path).ToArray();
    }

    private static void CreateZip()
    {
        FileUtil.DeleteFileOrDirectory(HeadlessServerZipPath);
        ZipFile.CreateFromDirectory($@"{BuildsDirectory}\{headlessServerName}", HeadlessServerZipPath);
        Debug.Log($@"Headless server zipped to: {HeadlessServerZipPath}");
    }

    private static void OpenInWindowsExplorer(string path)
    {
        if (Directory.Exists(path))
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                Arguments = path,
                FileName = "explorer.exe"
            };

            System.Diagnostics.Process.Start(startInfo);
        }
    }
}