using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEditor;
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
        UnityEngine.Debug.Log("Building Local Client");

        CleanDefineSymbols();
        AddDefineSymbols(new[] { "CLIENT_BUILD" });

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
        CleanDefineSymbols();
        UnityEngine.Debug.Log("WTF");
        OpenInWindowsExplorer(Path.GetDirectoryName(outputLocation));
    }

    [MenuItem("Run/Local Client #&c")]
    public static void RunClient()
    {
        var proc = new Process();
        proc.StartInfo.FileName = $@"{BuildsDirectory}\{localClientName}\{localClientName}.exe";
        proc.Start();
    }

    [MenuItem("Build/Local Server %&s")]
    public static void BuildServer()
    {
        UnityEngine.Debug.Log("Building local Server");

        CleanDefineSymbols();
        AddDefineSymbols(new[] { "SERVER_BUILD" });

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
        CleanDefineSymbols();
        OpenInWindowsExplorer(Path.GetDirectoryName(outputLocation));
    }

    [MenuItem("Run/Local Server #&s")]
    public static void RunServer()
    {
        var proc = new Process();
        proc.StartInfo.FileName = $@"{BuildsDirectory}\{localServerName}\{localServerName}.exe";
        proc.Start();
    }

    [MenuItem("Build/Headless Server %&h")]
    public static void BuildHeadlessServer()
    {
        UnityEngine.Debug.Log("Building Headless Server");

        CleanDefineSymbols();
        AddDefineSymbols(new[] { "SERVER_BUILD" });

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
        CleanDefineSymbols();
        OpenInWindowsExplorer(Path.GetDirectoryName(HeadlessServerLocationPathName));
    }

    [MenuItem("Run/Headless Server #&h")]
    public static void RunHeadlessServer()
    {
        var proc = new Process();
        proc.StartInfo.FileName = "CMD.EXE";
        proc.StartInfo.WorkingDirectory = $@"{ProjectParentDirectory}\MockVmAgent";
        proc.StartInfo.UseShellExecute = true;
        UnityEngine.Debug.Log($@"running agentlocated at: { ProjectParentDirectory}\MockVmAgent");
        proc.StartInfo.Arguments = $@"/K {ProjectParentDirectory}\MockVmAgent\MockVmAgent.exe";
        proc.Start();
    }

    private static void LogReport(BuildReport report)
    {
        UnityEngine.Debug.Log(report.summary);
        if (report.summary.result == BuildResult.Succeeded)
        {
            UnityEngine.Debug.Log("Build succeeded: " + report.summary.totalSize * .000001 + " MB");
        }

        if (report.summary.result == BuildResult.Failed)
        {
            UnityEngine.Debug.Log("Build failed");
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
        UnityEngine.Debug.Log($@"Headless server zipped to: {HeadlessServerZipPath}");
    }

    private static void CleanDefineSymbols()
    {
        var buildTargetGroup = BuildTargetGroup.Standalone;
        var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        var allDefines = definesString.Split(';').ToList();
        allDefines.RemoveAll(a => a == "SERVER_BUILD" || a == "CLIENT_BUILD");
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", allDefines.ToArray()));
    }

    private static void AddDefineSymbols(string[] symbols)
    {
        var buildTargetGroup = BuildTargetGroup.Standalone;
        var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        var allDefines = definesString.Split(';').ToList();
        allDefines.AddRange(symbols.Except(allDefines));
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", allDefines.ToArray()));
    }

    private static void OpenInWindowsExplorer(string path)
    {
        UnityEngine.Debug.Log($"Trying to build to {path}");
        if (Directory.Exists(path))
        {
            var startInfo = new ProcessStartInfo
            {
                Arguments = path,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }
    }
}