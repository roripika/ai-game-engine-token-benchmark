using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace TsumTsumu.Editor
{
    public static class BuildScript
    {
        public static void PerformBuild()
        {
            string[] scenes = new[] { "Assets/Scenes/MainScene.unity" };
            string buildPath = "Builds/Mac/UnityTsumTsumu.app";

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = buildPath,
                target = BuildTarget.StandaloneOSX,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded: {summary.totalSize} bytes");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError($"Build failed with {summary.totalErrors} errors");
            }
        }
    }
}
