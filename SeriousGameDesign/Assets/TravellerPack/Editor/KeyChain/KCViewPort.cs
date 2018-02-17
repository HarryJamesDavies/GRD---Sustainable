using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class KCViewPort : Editor
{
    private Vector2 m_presetBarPrecentage = new Vector2(0.99f, 0.05f);
    private Vector2 m_viewportPrecentage = new Vector2(0.99f, 0.865f);
    private Vector2 m_viewportSectionPrecentage = new Vector2(0.488f, 0.84f);
    private float m_toolbarWidthPercentage = 0.95f;

    private Vector2 m_tabSizes = new Vector2(100.0f, 24.0f);

    public void DoWindow(int _unusedWindowID)
    {
        EditorGUILayout.BeginVertical();
        {
            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);

            DrawPresetBar();

            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);

            GUILayout.Space(-3.5f);

            //Sets up the main Rect in which all sub Rects will sit
            EditorGUILayout.BeginHorizontal("Box", GUILayout.Width(KeyChainEditor.m_viewportRect.width * m_viewportPrecentage.x),
                GUILayout.Height(KeyChainEditor.m_viewportRect.height * m_viewportPrecentage.y));
            {
                if (KeyChainEditor.Window.m_openPresetSelected != -1)
                {
                    GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.BeginVertical("Box", GUILayout.Width(KeyChainEditor.m_viewportRect.width * m_viewportSectionPrecentage.x),
                            GUILayout.Height(KeyChainEditor.m_viewportRect.height * m_viewportSectionPrecentage.y));
                        {
                            DrawDefaultActions(KeyChain.Instance.m_inputMapManager.GetReferenceMap(KeyChainEditor.Window.m_openPresets[KeyChainEditor.Window.m_openPresetSelected].m_ID));
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical("Box", GUILayout.Width(KeyChainEditor.m_viewportRect.width * m_viewportSectionPrecentage.x),
                            GUILayout.Height(KeyChainEditor.m_viewportRect.height * m_viewportSectionPrecentage.y));
                        {
                            DrawCustomActions(KeyChain.Instance.m_inputMapManager.GetReferenceMap(KeyChainEditor.Window.m_openPresets[KeyChainEditor.Window.m_openPresetSelected].m_ID));
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                    style.normal.textColor = Color.white;
                    GUILayout.Label("Open InputMap Preset Below!", style);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);
    }

    void DrawPresetBar()
    {
        GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Rect presetBar = EditorGUILayout.BeginHorizontal("Box", GUILayout.Width(KeyChainEditor.m_viewportRect.width * m_presetBarPrecentage.x),
            GUILayout.Height(KeyChainEditor.m_viewportRect.height * m_presetBarPrecentage.y), GUILayout.MinHeight(m_tabSizes.y));
        {
            if (KeyChainEditor.Window.m_openPresets.Count >= 0)
            {
                List<string> allNames = new List<string>();
                foreach (KCToolbar.InputMapTag tag in KeyChainEditor.Window.m_openPresets)
                {
                    string name = tag.m_name;

                    if (KeyChain.Instance.m_inputMapManager.GetReferenceMap(tag.m_ID).CheckAlterations())
                    {
                        name += "*";
                    }

                    allNames.Add(name);
                }

                KeyChainEditor.Window.m_openPresetSelected = GUILayout.Toolbar(KeyChainEditor.Window.m_openPresetSelected, allNames.ToArray(),
                   GUILayout.Width(KeyChainEditor.m_viewportRect.width * m_toolbarWidthPercentage));

                Event currentEvent = Event.current;

                if (currentEvent.type == EventType.ContextClick)
                {
                    Vector2 mousePos = currentEvent.mousePosition;
                    if (presetBar.Contains(mousePos))
                    {
                        GenericMenu menu = new GenericMenu();

                        menu.AddItem(new GUIContent("Close Tab"), false, ClosePreset);
                        menu.ShowAsContext();
                        currentEvent.Use();
                    }
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    void DrawDefaultActions(InputMap _map)
    {
        EditorGUILayout.IntField("Default Actions:", _map.m_defaultActionCount);

        EditorGUILayout.Space();

        for (int i = 0; i <= _map.m_defaultActionCount - 1; i++)
        {
            EditorGUILayout.BeginHorizontal();

            int dataLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;

            float width = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 60.0f;

            EditorGUILayout.TextField("Action", _map.m_defaultAction[i].m_name);

            EditorGUILayout.Popup("Key", _map.GetKeyIndex(_map.m_defaultAction[i].m_button.m_key), _map.m_keyNames.ToArray());

            EditorGUI.indentLevel = dataLevel;
            EditorGUIUtility.labelWidth = width;

            EditorGUILayout.EndHorizontal();
        }
    }

    void DrawCustomActions(InputMap _map)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.IntField("Custom Actions:", _map.m_customActionCount);

            Color prevColor = GUI.color;
            GUI.color = Color.green;
            if (GUILayout.Button("+", GUILayout.Width(20.0f)))
            {
                KeyChain.Instance.m_inputMapManager.GetReferenceMap(KeyChainEditor.Window.m_openPresets[KeyChainEditor.Window.m_openPresetSelected].m_ID).AddCustomAction();
            }
            GUI.color = prevColor;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        for (int i = 0; i <= _map.m_customActionCount - 1; i++)
        {
            EditorGUILayout.BeginHorizontal();
            {
                int dataLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 1;

                float width = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 60.0f;

                _map.SetCustomAction(i, EditorGUILayout.TextField("Action", _map.m_customAction[i].m_name));

                _map.SetCustomButton(i, _map.m_keyNames[EditorGUILayout.Popup("Key", _map.GetKeyIndex(_map.m_customAction[i].m_button.m_key), _map.m_keyNames.ToArray())]);

                Color prevColor = GUI.color;
                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(20.0f)))
                {
                    InputMap map = KeyChain.Instance.m_inputMapManager.GetReferenceMap(KeyChainEditor.Window.m_openPresets[KeyChainEditor.Window.m_openPresetSelected].m_ID);
                    map.RemoveAction(map.m_customAction[i].m_name);
                }
                GUI.color = prevColor;

                EditorGUI.indentLevel = dataLevel;
                EditorGUIUtility.labelWidth = width;
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    public void ClosePreset()
    {
        if (KeyChainEditor.Window.m_openPresets.Count != 0)
        {
            KCToolbar.InputMapTag tag = KeyChainEditor.Window.m_openPresets[KeyChainEditor.Window.m_openPresetSelected];
            KeyChainEditor.Window.m_openPresets.Remove(tag);

            KeyChainEditor.Window.m_openPresetSelected = 0;
            if (KeyChainEditor.Window.m_openPresets.Count == 0)
            {
                KeyChainEditor.Window.m_openPresetSelected = -1;
            }
        }
    }
}
