using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorCore : Editor
{
    private static string ProjectPath = "";

    public static string GetProjectPath()
    {        
        if(ProjectPath.Length == 0)
        {
            ProjectPath = Application.dataPath;
            ProjectPath = ProjectPath.Replace("Assets", "");
        }

        return ProjectPath;
    }
}
