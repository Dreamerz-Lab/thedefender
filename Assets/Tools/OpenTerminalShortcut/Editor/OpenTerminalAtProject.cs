using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public static class OpenTerminalAtProject
{
    [MenuItem("Tools/Headshot Games/Open Terminal at Project %#.")]
    public static void OpenTerminal()
    {
        string projectPath = Application.dataPath;
        projectPath = projectPath.Substring(0, projectPath.Length - 6);
        Process.Start("open", "-a Terminal " + projectPath + " -n");
    }
}
