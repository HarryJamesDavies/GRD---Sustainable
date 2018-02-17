using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GWHeaderBar : Editor
{
    public int toolbarInt = 0;
    public string[] toolbarStrings = new string[] { "Toolbar1", "Toolbar2", "Toolbar3" };

    private TPTabBar tab;

    public void Initialise()
    {
        tab = CreateInstance<TPTabBar>().Initialise();
        List<string> names = new List<string>();
        names.Add("James");
        names.Add("Tom");
        names.Add("Clair");
        names.Add("Matthew");
        tab.AddTabRange(names);
        tab.AddTab("Paul");
    }

    public void DoWindow(int unusedWindowID)
    {
        GUILayout.BeginHorizontal("Box");
        {
            //toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);
            //if (GUILayout.Button("+"))
            //{
            //    Debug.Log("Add");
            //}
            tab.RenderGUI();
        }
        GUILayout.EndHorizontal();
    }
}
