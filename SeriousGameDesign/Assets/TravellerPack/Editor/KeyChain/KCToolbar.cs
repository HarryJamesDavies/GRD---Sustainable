using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class KCToolbar : Editor
{
    public struct InputMapTag
    {
        public string m_name;
        public int m_ID;
    }

    public float m_toolbarMinimumHeight = 65.0f;
    public bool m_toolbarToggle = false;
    private int m_toolbarTab = 1;
    private string[] m_toolbarTabNames = new string[] { "Settings", "Main", "Load" };

    private string[] m_typeString;
    private int m_typeSelected = (int)InputMapManager.InputType.NULL;

    public string m_presetName = "";
    public InputMapManager.InputType m_presetType = InputMapManager.InputType.NULL;
    private Dictionary<InputMapManager.InputType, List<InputMapTag>> m_presetNames = new Dictionary<InputMapManager.InputType, List<InputMapTag>>();
    private int m_presetSelected = -1;
    private Vector2 m_presetScrollPosition = Vector2.zero;

    public string m_deleteName = "";
    public int m_deleteID = -1;

    private bool m_initialisedPreset = false;
    private bool m_initialisedOpen = false;
    private bool m_initialisedSettings = false;

    private Vector2 m_titleBarPercentage = new Vector2(0.99f, 0.1f);
    private Vector2 m_toolbarSectionPercentage = new Vector2(0.9875f, 0.69f);
    private Vector2 m_toolbarInnerPercentage = new Vector2(0.325f, 0.63f);
    private Vector2 m_toolbarInnerScrollPercentage = new Vector2(0.977f, 0.525f);

    private float m_buttonSize = 20.0f;
    private float m_resetButtonOffset = 423.0f;

    public void Initialise()
    {
        m_typeString = Enum.GetNames(typeof(InputMapManager.InputType));

        m_initialisedPreset = false;
        InitialisePresets();
        m_initialisedOpen = false;
        InitialiseOpen();
        m_initialisedSettings = false;
        InitialiseSettings();
    }

    void InitialisePresets()
    {
        if (!m_initialisedPreset)
        {
            m_presetNames.Clear();

            for (int j = 0; j <= InputMapManager.InputTypeCount - 1; j++)
            {
                m_presetNames.Add((InputMapManager.InputType)j, new List<InputMapTag>());
            }

            for (int j = 0; j <= InputMapManager.InputTypeCount - 1; j++)
            {
                foreach (InputMapManager.InputPreset preset in KeyChain.Instance.m_inputMapManager.GetAllMap((InputMapManager.InputType)j))
                {
                    InputMapTag tempTag;
                    tempTag.m_name = preset.m_name;
                    tempTag.m_ID = preset.m_ID;

                    m_presetNames[(InputMapManager.InputType)j].Add(tempTag);
                }
            }

            m_initialisedPreset = true;
        }
    }

    void InitialiseOpen()
    {
        if (!m_initialisedOpen)
        {
            KeyChainEditor.Window.m_openPresets.Clear();
            m_initialisedOpen = true;
        }
    }

    void InitialiseSettings()
    {
        if (!m_initialisedSettings)
        {
            InputSettingsData data = Resources.Load("InputSettings/KeyChainSettings") as InputSettingsData;

            if (data != null)
            {
                KeyChainEditor.m_resourcePath = data.m_resourcePath;

                ControllerManager.m_maxNumInputs = data.m_maxNumInputs;
                ControllerManager.m_enableKeyboard = data.m_enableKeyboard;
                ControllerManager.m_enableNES = data.m_enableNES;
                ControllerManager.m_enablePS4 = data.m_enablePS4;
            }
            else
            {
                Debug.Log("No Saved Settings");

                if (EditorUtility.DisplayDialog("Set Resource Folder Path",
            "Resource Path is shorter than Project Path.\nResource Path must be in a sub-folder of Assets Folder.\nDo you wish to reset it now",
            "Yes", "No"))
                {
                    SetResourcePath();
                }

                CreateSettings();
            }

            m_initialisedSettings = true;
        }
    }

    void CreateSettings()
    {
        InputSettingsData asset = ScriptableObject.CreateInstance<InputSettingsData>();
        asset.Initialise(KeyChainEditor.m_resourcePath);

        string path = KeyChainEditor.m_resourcePath + "/InputSettings/";
        string name = "KeyChainSettings.asset";

        if (AssetDatabase.LoadAssetAtPath(path + name, typeof(InputSettingsData)) != null)
        {
            AssetDatabase.DeleteAsset(path + name);
            AssetDatabase.Refresh();
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + name);

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void DoWindow(int _unusedWindowID)
    {
        GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        EditorGUILayout.BeginVertical();
        {
            int prevToolbar = m_toolbarTab;
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.BeginHorizontal("Box", GUILayout.Width(KeyChainEditor.m_toolbarRect.width * m_titleBarPercentage.x),
                    GUILayout.Height(KeyChainEditor.m_toolbarRect.height * m_titleBarPercentage.y));
                {
                    m_toolbarTab = GUILayout.Toolbar(m_toolbarTab, m_toolbarTabNames);
                    m_toolbarToggle = GUILayout.Toggle(m_toolbarToggle, "X", "Button", GUILayout.Width(m_buttonSize));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                if (!m_toolbarToggle)
                {
                    GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);

                    switch (m_toolbarTab)
                    {
                        case 0:
                            {
                                //Settings
                                DrawSettingSection();
                                break;
                            }
                        case 1:
                            {
                                //Main
                                DrawMainSection();
                                break;
                            }
                        case 2:
                            {
                                if (prevToolbar != m_toolbarTab)
                                {
                                    InitialisePresets();
                                }
                                //Load
                                DrawLoadSection();
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);
    }

    void DrawSettingSection()
    {
        GUILayout.BeginVertical("Box", GUILayout.Width(KeyChainEditor.m_toolbarRect.width * m_toolbarSectionPercentage.x), 
            GUILayout.Height(KeyChainEditor.m_toolbarRect.height * m_toolbarSectionPercentage.y));
        {
            GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            GUILayout.BeginHorizontal(GUILayout.Width(KeyChainEditor.m_toolbarRect.width * m_toolbarInnerPercentage.y));
            {
                GUILayout.BeginVertical("Box", GUILayout.Width(KeyChainEditor.m_toolbarRect.width * m_toolbarInnerPercentage.x), 
                    GUILayout.Height(KeyChainEditor.m_toolbarRect.height * m_toolbarInnerPercentage.y));
                {
                    GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                    style.normal.textColor = Color.black;
                    GUILayout.Label("Paths", style);

                    EditorGUILayout.TextField(KeyChainEditor.m_resourcePath);
                    if (GUILayout.Button("Set Resource Path"))
                    {
                        SetResourcePath();
                        CreateSettings();
                    }
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical("Box", GUILayout.Width(KeyChainEditor.m_toolbarRect.width * m_toolbarInnerPercentage.x), 
                    GUILayout.Height(KeyChainEditor.m_toolbarRect.height * m_toolbarInnerPercentage.y));
                {
                    GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                    style.normal.textColor = Color.black;
                    GUILayout.Label("Controllers", style);

                    GUILayout.BeginHorizontal();
                    {
                        bool keyboard = ControllerManager.m_enableKeyboard;
                        bool NES = ControllerManager.m_enableNES;
                        bool PS4 = ControllerManager.m_enablePS4;

                        ControllerManager.m_enableKeyboard = GUILayout.Toggle(ControllerManager.m_enableKeyboard, "Keyboard");
                        ControllerManager.m_enableNES = GUILayout.Toggle(ControllerManager.m_enableNES, "NES");
                        ControllerManager.m_enablePS4 = GUILayout.Toggle(ControllerManager.m_enablePS4, "PS4");

                        if (ControllerManager.m_enableKeyboard != keyboard || ControllerManager.m_enableNES != NES
                            || ControllerManager.m_enablePS4 != PS4)
                        {
                            CreateSettings();
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    void DrawMainSection()
    {
        GUILayout.BeginVertical("Box", GUILayout.Width(KeyChainEditor.m_toolbarRect.width * m_toolbarSectionPercentage.x),
            GUILayout.Height(KeyChainEditor.m_toolbarRect.height * m_toolbarSectionPercentage.y));
        {
            GUILayout.BeginHorizontal(GUILayout.Width(KeyChainEditor.m_toolbarRect.width * m_toolbarInnerPercentage.y));
            {
                GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                GUILayout.BeginVertical("Box", GUILayout.Width(KeyChainEditor.m_toolbarRect.width * m_toolbarInnerPercentage.x),
                        GUILayout.Height(KeyChainEditor.m_toolbarRect.height * m_toolbarInnerPercentage.y));
                {
                    GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                    style.normal.textColor = Color.black;
                    GUILayout.Label("New Map", style);

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Name");
                        GUILayout.Space(20.0f);
                        m_presetName = EditorGUILayout.TextField(m_presetName);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Type");
                        GUILayout.Space(25.0f);
                        m_presetType = (InputMapManager.InputType)EditorGUILayout.EnumPopup(m_presetType);
                    }
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Create Map"))
                    {
                        if (m_presetType != InputMapManager.InputType.NULL)
                        {
                            KeyChain.Instance.m_inputMapManager.AddEmptyPreset(m_presetName, m_presetType);
                            m_initialisedPreset = false;
                            m_presetName = "";
                        }
                        else
                        {
                            Debug.LogWarning("Don't create Input Map of type NULL");
                        }
                    }
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical("Box", GUILayout.Width(KeyChainEditor.m_toolbarRect.width * m_toolbarInnerPercentage.x),
                        GUILayout.Height(KeyChainEditor.m_toolbarRect.height * m_toolbarInnerPercentage.y));
                {
                    GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                    style.normal.textColor = Color.black;
                    GUILayout.Label("Duplicate Map", style);


                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical("Box", GUILayout.Width(KeyChainEditor.m_toolbarRect.width * m_toolbarInnerPercentage.x),
                        GUILayout.Height(KeyChainEditor.m_toolbarRect.height * m_toolbarInnerPercentage.y));
                {
                    GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                    style.normal.textColor = Color.black;
                    GUILayout.Label("Delete Map", style);

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Name");
                        GUILayout.Space(20.0f);
                        m_deleteName = EditorGUILayout.TextField(m_deleteName);
                        m_deleteID = KeyChain.Instance.m_inputMapManager.GetID(m_presetName);
                    }
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Delete Map"))
                    {
                        InputMapManager.InputType type = KeyChain.Instance.m_inputMapManager.GetType(m_deleteID);

                        if (type != InputMapManager.InputType.NULL)
                        {
                            DeletePreset(type);
                        }
                        else
                        {
                            Debug.LogWarning("Preset does not exist");
                        }
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    void DrawLoadSection()
    {
        GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);
        GUILayout.BeginVertical("Box", GUILayout.Width(KeyChainEditor.m_toolbarRect.width * m_toolbarSectionPercentage.x),
            GUILayout.Height(KeyChainEditor.m_toolbarRect.height * m_toolbarSectionPercentage.y));
        {
            GUILayout.BeginHorizontal(GUILayout.Width(KeyChainEditor.m_toolbarRect.width * m_toolbarInnerPercentage.y));
            {
                GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                int prevType = m_typeSelected;
                m_typeSelected = EditorGUILayout.Popup(m_typeSelected, m_typeString, GUILayout.Width(m_buttonSize * 10.0f));
                m_presetType = (InputMapManager.InputType)m_typeSelected;
                if (m_typeSelected != prevType)
                {
                    m_presetSelected = -1;
                }

                GUILayout.Space(KeyChainEditor.Window.m_windowSize.x - m_resetButtonOffset);

                if (GUILayout.Button("Reset Presets", GUILayout.Width(m_buttonSize * 10.0f)))
                {
                    m_initialisedPreset = false;
                    InitialisePresets();
                }
            }
            GUILayout.EndHorizontal();

            GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            GUILayout.BeginHorizontal("Box");
            {
                string presetName = "";
                int presetID = -1;

                m_presetScrollPosition = EditorGUILayout.BeginScrollView(m_presetScrollPosition, GUILayout.Width(KeyChainEditor.m_toolbarRect.width * m_toolbarInnerScrollPercentage.x),
                        GUILayout.Height(KeyChainEditor.m_toolbarRect.height * m_toolbarInnerScrollPercentage.y));
                {
                    List<string> allNames = new List<string>();
                    List<int> allID = new List<int>();

                    if (m_presetType == InputMapManager.InputType.NULL)
                    {
                        for (int j = 0; j <= (int)InputMapManager.InputTypeCount - 1; j++)
                        {
                            foreach (InputMapTag tag in m_presetNames[(InputMapManager.InputType)j])
                            {
                                allNames.Add(tag.m_name);
                                allID.Add(tag.m_ID);
                            }
                        }
                    }
                    else
                    {
                        foreach (InputMapTag tag in m_presetNames[m_presetType])
                        {
                            allNames.Add(tag.m_name);
                            allID.Add(tag.m_ID);
                        }
                    }

                    m_presetSelected = GUILayout.SelectionGrid(m_presetSelected, allNames.ToArray(), 3);

                    if (m_presetSelected != -1)
                    {
                        presetName = allNames[m_presetSelected];
                        presetID = allID[m_presetSelected];

                        OpenPreset(presetName, presetID);
                        m_presetSelected = -1;
                    }
                    allNames.Clear();
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    void OpenPreset(string _selected, int _selectedID)
    {
        if (CheckPresetOpen(_selectedID))
        {
            InputMapTag tag;
            tag.m_name = _selected;
            tag.m_ID = _selectedID;

            KeyChainEditor.Window.m_openPresets.Add(tag);
            KeyChainEditor.Window.m_openPresetSelected = KeyChainEditor.Window.m_openPresets.Count - 1;

            KeyChain.Instance.m_inputMapManager.GetReferenceMap(KeyChainEditor.Window.m_openPresets[KeyChainEditor.Window.m_openPresetSelected].m_ID).SetPreviousState();
        }
    }

    bool CheckPresetOpen(int _ID)
    {
        foreach (InputMapTag tag in KeyChainEditor.Window.m_openPresets)
        {
            if (tag.m_ID == _ID)
            {
                return false;
            }
        }
        return true;
    }

    void DeletePreset(InputMapManager.InputType _type)
    {
        string path = KeyChainEditor.m_resourcePath + "/" + "InputMap/ " + _type.ToString() + "/" + m_presetName + ".asset";
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.Refresh();
        KeyChain.Instance.m_inputMapManager.DeletePreset(_type, m_deleteID);
        m_initialisedPreset = false;
        InitialisePresets();
        m_initialisedOpen = false;
        InitialiseOpen();
        m_deleteName = "";
    }

    void SetResourcePath()
    {
        string projectPath = Application.dataPath;

        KeyChainEditor.m_resourcePath = EditorUtility.SaveFolderPanel("Resource Folder Path", "", "");

        if (KeyChainEditor.m_resourcePath.Length <= projectPath.Length)
        {
            if (EditorUtility.DisplayDialog("Reset Resource Folder Path",
                                "Resource Path is shorter than Project Path.\nResource Path must be in a sub-folder of Assets Folder.\nDo you wish to reset it now",
                                "Yes", "No"))
            {
                SetResourcePath();
            }
            else
            {
                KeyChainEditor.m_resourcePath = "Assets";
                return;
            }
        }
        else
        {
            projectPath = projectPath.Replace("Assets", " ");
            KeyChainEditor.m_resourcePath = KeyChainEditor.m_resourcePath.Remove(0, projectPath.Length - 1);

        }
    }
}
