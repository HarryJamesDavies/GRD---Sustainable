using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataCollectionEditor : EditorWindow
{

    [SerializeField]
    public static DataCollectionEditor Window;
    public Vector2 m_windowSize;
    public static Vector2 m_minimumWindowSize = new Vector2(1050.0f, 600.0f);
    
    public static DataCollectionViewport Viewport;
    private static Vector2 m_viewportRatio = new Vector2(5.0f, 4.0f);
    private static Vector2 m_viewportPercentage;
    public static Rect m_viewportRect;

    public static DataCollectionHeader Header;
    private static Vector2 m_headerRatio = new Vector2(5.0f, 1.0f);
    private static Vector2 m_headerPercentage;
    public static Rect m_headerRect;


    [MenuItem("TravellerPack/DataCollection")]
    static void InitialiseWindow()
    {
        CheckInitialised();
    }

    private static void CheckInitialised()
    {
        if(Window == null)
        {
            Window = (DataCollectionEditor)GetWindow(typeof(DataCollectionEditor), false, "DataCollection");
            Window.autoRepaintOnSceneChange = true;
            Window.minSize = m_minimumWindowSize;
        }

        float totalWidthRatio = m_viewportRatio.x;
        float totalHeightRatio = m_viewportRatio.y + m_headerRatio.y;

        if (Header == null)
        {
            m_headerRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
            m_headerPercentage.x = m_headerRatio.x / totalWidthRatio;
            m_headerPercentage.y = m_headerRatio.y / totalHeightRatio;
            Header = CreateInstance<DataCollectionHeader>();
        }

        if (Viewport == null)
        {
            Viewport = CreateInstance<DataCollectionViewport>(); m_viewportRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
            m_viewportPercentage.x = m_viewportRatio.x / totalWidthRatio;
            m_viewportPercentage.y = m_viewportRatio.y / totalHeightRatio;
        }
    }

    void OnGUI()
    {
        m_windowSize.x = Screen.width;
        m_windowSize.y = Screen.height;
        
        CheckInitialised();

        m_headerRect.width = m_windowSize.x * m_headerPercentage.x;
        m_headerRect.height = m_windowSize.y * m_headerPercentage.y;

        m_viewportRect.width = m_windowSize.x * m_viewportPercentage.x;
        m_viewportRect.height = m_windowSize.y * m_viewportPercentage.y;
        m_viewportRect.y = m_headerRect.height;

        BeginWindows();

        GUILayout.BeginVertical();
        {
            m_headerRect = GUILayout.Window(0, m_headerRect, Header.DoWindow, "Header");
            m_viewportRect = GUILayout.Window(1, m_viewportRect, Viewport.DoWindow, "Viewport");
        }
        GUILayout.EndVertical();

        EndWindows();
    }
}
