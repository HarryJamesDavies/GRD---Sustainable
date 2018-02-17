using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GWViewport : Editor
{
    private TPButtonGrid m_grid = null;
    private float m_portOffset = 60.0f;
    private float m_portScale = 1.0f;
    private float m_standardButtonSize = 40.0f;

    private Vector2 m_filePanelSize = new Vector2(100.0f, 50.0f);
    private Vector2 m_filePanelOffset = new Vector2(0.0f, 0.0f);
    private FileType m_fileType = FileType.png;
    private Texture2D m_layer = null;
    private float m_fileWhiteSpaceSize = 0.0f;

    private Vector2 m_buttonDimensions = new Vector2(1, 1);

    private const float BorderWhiteSpace = 2.5f;
    private const float InternalWhiteSpace = 5.0f;
    private float m_gridWhiteSpaceSize = 0.0f;

    private const string WorldPrefab =  "Assets/TravellerPack/GroundWorks/Prefabs/World.prefab";

    private Texture m_texPosi;
    private Texture m_texNega;

    public void Initialise()
    {
        m_fileWhiteSpaceSize = 2 * BorderWhiteSpace;
        m_gridWhiteSpaceSize = (2 * BorderWhiteSpace) + ((GroundWorksEditor.CurretWorld.SectionsPer + 1) * InternalWhiteSpace);

        m_texPosi = (Texture)AssetDatabase.LoadAssetAtPath("Assets/TravellerPack/EditorAssets/Textures/Positive.png", typeof(Texture));
        m_texNega = (Texture)AssetDatabase.LoadAssetAtPath("Assets/TravellerPack/EditorAssets/Textures/Negative.png", typeof(Texture));
    }

    public void DoWindow(int unusedWindowID)
    {
        if (m_layer && m_grid != null)
        {
            //Draws Chunks Grid
            float buttonsSizeX = m_buttonDimensions.x * m_standardButtonSize;
            float buttonsSizeY = m_buttonDimensions.y * m_standardButtonSize;

            if (GroundWorksEditor.Window.m_viewportWindowRect.width < GroundWorksEditor.Window.m_viewportWindowRect.height)
            {
                m_portScale = (GroundWorksEditor.Window.m_viewportWindowRect.width - m_portOffset) / buttonsSizeX;
            }
            else
            {
                m_portScale = (GroundWorksEditor.Window.m_viewportWindowRect.height - m_portOffset) / buttonsSizeY;
            }

            buttonsSizeX *= m_portScale;
            buttonsSizeY *= m_portScale;

            m_grid.SetPosition(new Vector2(((GroundWorksEditor.Window.m_viewportWindowRect.width - buttonsSizeX) / 2.0f) - m_gridWhiteSpaceSize,
                ((GroundWorksEditor.Window.m_viewportWindowRect.height - buttonsSizeY) / 2.0f) - m_gridWhiteSpaceSize));
            m_grid.SetButtonSize(m_standardButtonSize * (m_portScale));
            TPButtonGridResponse response = m_grid.RenderGUI();

            TPButtonResponse button = response.GetNextButton();
            if (button != null)
            {
                GroundWorksEditor.CurrentSection = GroundWorksEditor.CurrentLayer.GetSection(button.m_gridPosition);
                GroundWorksEditor.CurrentChunk = GroundWorksEditor.CurrentSection.GetChunk(
                    new Vector2((int)button.m_gridPosition.x - (((int)button.m_gridPosition.x / GroundWorksEditor.CurretWorld.SectionsPer) * GroundWorksEditor.CurretWorld.ChunksPer),
                    (int)button.m_gridPosition.y - (((int)button.m_gridPosition.y / GroundWorksEditor.CurretWorld.SectionsPer)) * GroundWorksEditor.CurretWorld.ChunksPer));
            }
        }
        else if(m_layer == null)
        {
            //Draws file panel
            m_filePanelOffset = new Vector2(((GroundWorksEditor.Window.m_viewportWindowRect.width / 2.0f) - m_filePanelSize.x - m_fileWhiteSpaceSize),
                ((GroundWorksEditor.Window.m_viewportWindowRect.height / 2.0f) - m_filePanelSize.y - m_fileWhiteSpaceSize));

            GUILayout.BeginVertical();
            {
                GUILayout.Space(m_filePanelOffset.y);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(m_filePanelOffset.x);

                    GUILayout.BeginVertical("Box", GUILayout.Width(m_filePanelSize.x), GUILayout.Height(m_filePanelSize.y));
                    {

                        m_fileType = (FileType)EditorGUILayout.EnumPopup("FileType:", m_fileType);
                        if (GUILayout.Button("Select Layer Map"))
                        {
                            string path = EditorUtility.OpenFilePanel("Select Layer Map", "/Assets/", m_fileType.ToString());
                            if (path.Length != 0)
                            {
                                path = path.Replace(EditorCore.GetProjectPath(), "");
                                m_layer = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));

                                GroundWorksEditor.CurrentLayer = GroundWorksEditor.CurretWorld.InitialiseLayer(m_layer, GroundWorksEditor.CurretWorld.m_activeLayers.Count);
                            }
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.Label("");
        }
    }

    public void InitaliseGrid()
    {
        m_buttonDimensions = new Vector2(m_layer.width, m_layer.height);
        float buttonsSizeX = m_buttonDimensions.x * m_standardButtonSize * m_portScale;
        float buttonsSizeY = m_buttonDimensions.y * m_standardButtonSize * m_portScale;

        Vector2 pos = new Vector2((GroundWorksEditor.Window.m_viewportWindowRect.width - buttonsSizeX - m_gridWhiteSpaceSize) / 2.0f,
            ((GroundWorksEditor.Window.m_viewportWindowRect.height - buttonsSizeY - m_gridWhiteSpaceSize) / 2.0f));

        m_grid = CreateInstance<TPButtonGrid>().Initialise(TPButtonType.Click, m_buttonDimensions, (m_standardButtonSize * m_portScale),
            pos, GroundWorksEditor.CurrentLayer.GetActiveChunks(), m_texPosi, m_texNega, true);
    }
}
