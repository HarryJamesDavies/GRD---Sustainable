using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class KCInspector : Editor
{
    public float m_inspectorMinimumWidth = 40.0f;
    public bool m_inspectorToggle = false;

    private Vector2 m_titlePercentage = new Vector2(0.94f, 0.075f);
    private Vector2 m_sectionPercentage = new Vector2(0.94f, 0.15f);
    private float m_buttonSize = 20.0f;

    public void DoWindow(int _unusedWindowID)
    {
        GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        
        EditorGUILayout.BeginVertical();
        {
            GUILayout.BeginHorizontal("Box", GUILayout.Width(KeyChainEditor.m_inspectorRect.width * m_titlePercentage.x), 
                GUILayout.Height(KeyChainEditor.m_inspectorRect.height * m_titlePercentage.y));
            {
                if (!m_inspectorToggle)
                {
                    GUILayout.Label("Tools", EditorStyles.boldLabel);
                }
                m_inspectorToggle = GUILayout.Toggle(m_inspectorToggle, "X", "Button", GUILayout.Width(m_buttonSize));
            }
            GUILayout.EndHorizontal();
            
            if (!m_inspectorToggle && KeyChainEditor.Window.m_openPresets.Count != 0)
            {
                GUILayout.BeginVertical("Box", GUILayout.Width(KeyChainEditor.m_inspectorRect.width * m_sectionPercentage.x),
                GUILayout.Height(KeyChainEditor.m_inspectorRect.height * m_sectionPercentage.y));
                {
                    GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                    style.normal.textColor = Color.black;
                    GUILayout.Label("Save Preset", style);

                    InputMapManager.InputPreset preset = KeyChain.Instance.m_inputMapManager.GetReferencePreset(KeyChainEditor.Window.m_openPresets[KeyChainEditor.Window.m_openPresetSelected].m_ID);

                    if (GUILayout.Button("Save Preset"))
                    {
                        preset.m_map.SetPreviousState();
                        CreateMapAsset(preset.m_map, preset.m_name, preset.m_map.m_mapType.ToString());
                    }
                }
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.Label("   ");
            }

        }
        EditorGUILayout.EndVertical();

        GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);
    }

    public void CreateMapAsset(InputMap _map, string _name, string _folder)
    {
        if (KeyChainEditor.m_resourcePath != "")
        {
            MapData asset = ScriptableObject.CreateInstance<MapData>();
            asset.Initialise(_map, _name);

            string path = KeyChainEditor.m_resourcePath + "/InputMap/" + _folder + "/";
            string name = _name + ".asset";

            if (AssetDatabase.LoadAssetAtPath(path + name, typeof(MapData)) != null)
            {
                if (EditorUtility.DisplayDialog("Assets already exists",
                                    "Would you like to overwrite the old file?",
                                    "Yes", "No"))
                {
                    AssetDatabase.DeleteAsset(path + name);
                    AssetDatabase.Refresh();
                }
                else
                {
                    KeyChain.Instance.m_inputMapManager.AddDuplicatePreset(_name, _map);
                }
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + name);

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogWarning("Resource path not set!");
        }
    }
}
