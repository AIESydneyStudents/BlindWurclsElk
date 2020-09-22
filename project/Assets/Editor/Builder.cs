using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using System.Threading;

public class Builder
{
    public static void Build()
    {
        string assetFolderPath = Application.dataPath;
        string pcFileName = assetFolderPath + "/../builds/pc/MyGame.exe";
        string webFileName = assetFolderPath + "/../builds/web/";

        var scenes = EditorBuildSettings.scenes;
        var levels = scenes.Select(z => z.path).ToArray();

        BuildPipeline.BuildPlayer(levels, webFileName, BuildTarget.WebGL, BuildOptions.None);
        BuildPipeline.BuildPlayer(levels, pcFileName, BuildTarget.StandaloneWindows, BuildOptions.None);
        
        Thread.Sleep(2000);
    }
}
