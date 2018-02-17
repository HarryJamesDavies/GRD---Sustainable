using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GroundWorksEditor : EditorWindow
{
    [SerializeField]
    public static GroundWorksEditor Window;
    public const float BorderOffsetX = 20.0f;
    public const float BorderOffsetY = 30.0f;
    public Vector2 m_size;

    public static GWHeaderBar HeaderBar;
    private static float m_headerWidthRatio = 4.0f;
    private static float m_headerWidthPercentage = 0.0f;
    private static float m_headerHeightRatio = 1.0f;
    private static float m_headerHeightPercentage = 0.0f;
    private float m_headerMaxHeight = 50.0f;
    public Rect m_headerWindowRect = new Rect(0, 0, 200, 20);

    public static GWViewport Viewport;
    private static float m_viewWidthRatio = 4.0f;
    private static float m_viewWidthPercentage = 0.0f;
    private static float m_viewHeightRatio = 9.0f;
    private static float m_viewHeightPercentage = 0.0f;
    public Rect m_viewportWindowRect = new Rect(0, 0, 200, 200);

    public static GWInspector Inspector;
    private static float m_inspectorWidthRatio = 1.0f;
    private static float m_inspectorWidthPercentage = 0.0f;
    public Rect m_inspectorWindowRect = new Rect(0, 0, 200, 200);

    [SerializeField]
    public static World CurretWorld = null;
    [SerializeField]
    public static Layer CurrentLayer = null;
    [SerializeField]
    public static Section CurrentSection = null;
    [SerializeField]
    public static Chunk CurrentChunk = null;

    private const string ManagerPrefab = "Assets/TravellerPack/GroundWorks/Prefabs/GroundWorks.prefab";
    private const string WorldPrefab = "Assets/TravellerPack/GroundWorks/Prefabs/World.prefab";
    private Vector2 m_filePanelSize = new Vector2(100.0f, 50.0f);
    private Vector2 m_filePanelOffset = new Vector2(0.0f, 0.0f);
    private const float BorderWhiteSpace = 5.0f;

    private static bool m_initialised = false;

    [MenuItem("TravellerPack/GroundWorks")]
    static void InitialiseWindow()
    {
        // Get existing open window or if none, make a new one:
        Window = (GroundWorksEditor)GetWindow(typeof(GroundWorksEditor), false, "GroundWorks");
        Window.Show();

        CurretWorld = null;
        CurrentLayer = null;
        CurrentSection = null;
        CurrentChunk = null;

        m_initialised = false;
    }

    void InitialiseSubWindows()
    {
        float widthRatio = m_headerWidthRatio;
        if (widthRatio < m_viewWidthRatio)
        {
            widthRatio = m_viewWidthRatio;
        }

        float totalWidthRatio = widthRatio + m_inspectorWidthRatio;
        float totalHeightRatio = m_headerHeightRatio + m_viewHeightRatio;

        m_headerWidthPercentage = widthRatio / totalWidthRatio;
        m_headerHeightPercentage = m_headerHeightRatio / totalHeightRatio;
        HeaderBar = CreateInstance<GWHeaderBar>();
        HeaderBar.Initialise();

        m_viewWidthPercentage = widthRatio / totalWidthRatio;
        m_viewHeightPercentage = m_viewHeightRatio / totalHeightRatio;
        Viewport = CreateInstance<GWViewport>();
        Viewport.Initialise();

        m_inspectorWidthPercentage = m_inspectorWidthRatio / totalWidthRatio;
        Inspector = CreateInstance<GWInspector>();
        Inspector.Initialise();

        m_initialised = true;

    }

    void OnGUI()
    {
        if (!Window)
        {
            InitialiseWindow();
        }

        m_size = new Vector2(Window.position.width, Window.position.height);

        if (CurretWorld == null)
        {
            SetupGroundWorks();
        }
        else
        {
            if (!m_initialised)
            {
                InitialiseSubWindows();
            }

            CoreGroundWorks();
        }
    }

    private void SetupGroundWorks()
    {
        m_filePanelOffset = new Vector2(m_size.x / 2.0f, m_size.y / 2.0f);

        if (GroundWorks.Instance != null)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(m_filePanelOffset.y - (m_filePanelSize.y / 2.0f) - BorderWhiteSpace);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(m_filePanelOffset.x - (m_filePanelSize.x / 2.0f) - BorderWhiteSpace);

                    GUILayout.BeginVertical("Box", GUILayout.Width(m_filePanelSize.x), GUILayout.Height(m_filePanelSize.y));
                    {
                        GUILayout.Space(m_filePanelSize.y / 4.0f);

                        if (GUILayout.Button("Create World"))
                        {
                            CurretWorld = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath(WorldPrefab, typeof(GameObject)),
                                Vector3.zero, Quaternion.identity).GetComponent<World>();
                            CurretWorld.gameObject.name = "World";
                            GroundWorks.Instance.AddWorld(CurretWorld);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(m_filePanelOffset.y - (m_filePanelSize.y / 2.0f) - BorderWhiteSpace);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(m_filePanelOffset.x - (m_filePanelSize.x / 2.0f) - BorderWhiteSpace);

                    GUILayout.BeginVertical("Box", GUILayout.Width(m_filePanelSize.x), GUILayout.Height(m_filePanelSize.y));
                    {
                        GUILayout.Space(m_filePanelSize.y / 4.0f);

                        if (GUILayout.Button("Create GroundWorks"))
                        {
                            GameObject temp = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath(ManagerPrefab, typeof(GameObject)),
                                                Vector3.zero, Quaternion.identity);
                            temp.name = "GroundWorks";
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }

    private void CoreGroundWorks()
    {
        GUILayout.BeginHorizontal();
        {
            BeginWindows();

            GUILayout.BeginVertical();
            {
                m_headerWindowRect.width = m_size.x * m_headerWidthPercentage;
                m_headerWindowRect.height = m_size.y * m_headerHeightPercentage;

                if (m_headerWindowRect.height > m_headerMaxHeight)
                {
                    m_headerWindowRect.height = m_headerMaxHeight;
                }
                m_headerWindowRect = GUILayout.Window(1, m_headerWindowRect, HeaderBar.DoWindow, "Layers");

                m_viewportWindowRect.y = m_headerWindowRect.height;
                m_viewportWindowRect.width = m_size.x * m_viewWidthPercentage;
                m_viewportWindowRect.height = m_size.y * m_viewHeightPercentage;
                m_viewportWindowRect = GUILayout.Window(2, m_viewportWindowRect, Viewport.DoWindow, "ViewPort");
            }
            GUILayout.EndVertical();

            m_inspectorWindowRect.x = m_headerWindowRect.width;
            m_inspectorWindowRect.width = m_size.x * m_inspectorWidthPercentage;
            m_inspectorWindowRect.height = m_size.y;
            m_inspectorWindowRect = GUILayout.Window(3, m_inspectorWindowRect, Inspector.DoWindow, "Inspector");

            EndWindows();
        }
        GUILayout.EndHorizontal();
    }
}
