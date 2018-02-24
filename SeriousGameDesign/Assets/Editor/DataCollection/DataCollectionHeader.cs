using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataCollectionHeader : Editor
{
    public void DoWindow(int _unusedWindowID)
    {
        EditorGUILayout.BeginVertical();
        {
            if (RetensionTracker.Instance != null)
            {
                if (GUILayout.Button("Save"))
                {
                    RetensionTracker.Instance.SaveData();
                    AssetDatabase.Refresh();
                }

                if (GUILayout.Button("Load"))
                {
                    RetensionTracker.Instance.LoadData("");
                }
            }
        }
        EditorGUILayout.EndVertical();
    }
}
