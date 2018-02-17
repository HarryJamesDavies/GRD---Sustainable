using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class KeyChainEditor : EditorWindow
{
    [SerializeField]
    public static KeyChainEditor Window;
    public Vector2 m_windowSize;
    public static Vector2 m_minimumWindowSize = new Vector2(1050.0f, 600.0f);
    public Vector2 m_windowSizePercentage = new Vector2(0.0f, 0.0f);

    public static KCViewPort ViewPort;
    private static Vector2 m_viewportRatio = new Vector2(5.0f, 4.0f);
    private static Vector2 m_viewportPercentage;
    public static Rect m_viewportRect;

    public static KCInspector Inspector;
    private static Vector2 m_inspectorRatio = new Vector2(1.0f, 4.0f);
    private static Vector2 m_inspectorPercentage;
    public static Rect m_inspectorRect;

    public static KCToolbar ToolBar;
    private static Vector2 m_toolbarRatio = new Vector2(6.0f, 2.5f);
    private static Vector2 m_toolbarPercentage;
    public static Rect m_toolbarRect;

    public const float m_titleBarHeight = 25.0f;
    public static string m_resourcePath = "";

    public int m_openPresetSelected = -1;
    public List<KCToolbar.InputMapTag> m_openPresets = new List<KCToolbar.InputMapTag>();

    private bool m_windowInitialised = false;

    [MenuItem("TravellerPack/KeyChain")]
    static void InitialiseWindow()
    {
        Window = (KeyChainEditor)GetWindow(typeof(KeyChainEditor), false, "KeyChain");
        Window.minSize = m_minimumWindowSize;
    }

    private void InitialiseSubWindows()
    {
        if(KeyChain.Instance == null)
        {
            KeyChain instance = FindObjectOfType<KeyChain>();
            instance.CheckInstance();
        }

        float totalWidthRatio = m_viewportRatio.x + m_inspectorRatio.x;
        if (totalWidthRatio < m_toolbarRatio.x)
        {
            totalWidthRatio = m_toolbarRatio.x;
        }

        float heightRatio = m_viewportRatio.y;
        if (heightRatio < m_inspectorRatio.y)
        {
            heightRatio = m_inspectorRatio.y;
        }
        float totalHeightRatio = heightRatio + m_toolbarRatio.y;

        m_viewportRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
        m_viewportPercentage.x = m_viewportRatio.x / totalWidthRatio;
        m_viewportPercentage.y = heightRatio / totalHeightRatio;
        ViewPort = CreateInstance<KCViewPort>();

        m_inspectorRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
        m_inspectorPercentage.x = 1.0f - m_viewportPercentage.x;
        m_inspectorPercentage.y = m_viewportPercentage.y;
        Inspector = CreateInstance<KCInspector>();

        m_toolbarRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
        m_toolbarPercentage.x = 1.0f;
        m_toolbarPercentage.y = 1.0f - m_viewportPercentage.y;
        ToolBar = CreateInstance<KCToolbar>();
        ToolBar.Initialise();
    }

    void OnGUI()
    {
        //Gets the max screen space internal Rects can inhabit
        m_windowSize.x = Screen.width;
        m_windowSize.y = Screen.height;

        m_windowSizePercentage.x = (m_windowSize.x / m_minimumWindowSize.x);
        m_windowSizePercentage.y = (m_windowSize.y / m_minimumWindowSize.y);


        if (m_windowInitialised)
        {
            if (ToolBar.m_toolbarToggle)
            {
                m_toolbarRect.width = m_windowSize.x;
                m_toolbarRect.height = ToolBar.m_toolbarMinimumHeight;
            }
            else
            {
                m_toolbarRect.width = m_windowSize.x;
                m_toolbarRect.height = m_windowSize.y * m_toolbarPercentage.y;
            }
            m_toolbarRect.y = m_windowSize.y - m_toolbarRect.height;

            if (Inspector.m_inspectorToggle)
            {
                m_inspectorRect.width = Inspector.m_inspectorMinimumWidth;
                m_inspectorRect.height = m_toolbarRect.y;
            }
            else
            {
                m_inspectorRect.width = m_windowSize.x * m_inspectorPercentage.x;
                m_inspectorRect.height = m_toolbarRect.y;
            }
            m_inspectorRect.x = m_windowSize.x - m_inspectorRect.width;

            m_viewportRect.width = m_windowSize.x - m_inspectorRect.width;
            m_viewportRect.height = m_toolbarRect.y;

            BeginWindows();

            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    m_viewportRect = GUILayout.Window(1, m_viewportRect, ViewPort.DoWindow, "Viewport");

                    m_inspectorRect = GUILayout.Window(2, m_inspectorRect, Inspector.DoWindow, "Inspector");
                }
                GUILayout.EndHorizontal();

                m_toolbarRect = GUILayout.Window(3, m_toolbarRect, ToolBar.DoWindow, "Toolbar");
            }
            GUILayout.EndVertical();

            EndWindows();
        }
        else
        {
            InitialiseSubWindows();

            if (Event.current.type == EventType.Repaint)
            {
                m_windowInitialised = true;
            }
        }
    }
}