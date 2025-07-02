
using UnityEditor;
using UnityEngine;

public class BuildCreator
{
    [MenuItem("Build/Build Windows")]
    public static void PerformBuild()
    {
        string[] scenes = { "Assets/Scenes/MainScene.unity" }; // <-- zmieñ œcie¿kê, jeœli inna
        string buildPath = "Builds/Windows/MyGame.exe";   // mo¿esz zmieniæ nazwê

        BuildPipeline.BuildPlayer(
            scenes,
            buildPath,
            BuildTarget.StandaloneWindows64,
            BuildOptions.None
        );

        Debug.Log("Build complete: " + buildPath);
    }
}
