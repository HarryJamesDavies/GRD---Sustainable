using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GWInspector : Editor
{
    private const float WorldTitleSize = 60.0f;
    private const float LayerTitleSize = 60.0f;
    private const float ChunkTitleSize = 60.0f;

    private bool m_layerInitialised = false;

    public void Initialise()
    {
    }

    public void DoWindow(int unusedWindowID)
    {
        GUILayout.BeginVertical();
        {
            //World Properties
            GUILayout.BeginVertical("Box");
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space((GroundWorksEditor.Window.m_inspectorWindowRect.width / 2.0f) - WorldTitleSize);
                    GUILayout.Label("World Properties");
                }
                GUILayout.EndHorizontal();

                GroundWorksEditor.CurretWorld.SectionsPer = EditorGUILayout.IntField("Sections Per Layer Axis", GroundWorksEditor.CurretWorld.SectionsPer);
                GroundWorksEditor.CurretWorld.ChunksPer = EditorGUILayout.IntField("Chunks Per Section Axis", GroundWorksEditor.CurretWorld.ChunksPer);

                if (GroundWorksEditor.CurrentLayer != null)
                {
                    if (GUILayout.Button("Initialise Layer"))
                    {
                        m_layerInitialised = true;
                        GroundWorksEditor.CurrentLayer.SecondaryInitialise();
                        GroundWorksEditor.Viewport.InitaliseGrid();
                    }
                }
            }
            GUILayout.EndVertical();

            //Current Layer Properties
            GUILayout.BeginVertical("Box");
            {
                if (GroundWorksEditor.CurrentLayer != null)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space((GroundWorksEditor.Window.m_inspectorWindowRect.width / 2.0f) - ChunkTitleSize);
                        GUILayout.Label(GroundWorksEditor.CurrentLayer.gameObject.name + " Properties");
                    }
                    GUILayout.EndHorizontal();

                    GroundWorksEditor.CurrentLayer.m_layerHeight = EditorGUILayout.FloatField("Layer Height", GroundWorksEditor.CurrentLayer.m_layerHeight);
                    GroundWorksEditor.CurrentLayer.m_sectionLength = EditorGUILayout.FloatField("Section Length", GroundWorksEditor.CurrentLayer.m_sectionLength);

                    if (m_layerInitialised)
                    {
                        if (GUILayout.Button("Initialise Surfaces"))
                        {
                            GroundWorksEditor.CurrentLayer.TeritaryInitialise();
                        }
                    }
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space((GroundWorksEditor.Window.m_inspectorWindowRect.width / 2.0f) - LayerTitleSize);
                        GUILayout.Label("Layer Properties");
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Label("No Layer Selected");
                }
            }
            GUILayout.EndVertical();

            //Current Section Properties
            GUILayout.BeginVertical("Box");
            {
                if (GroundWorksEditor.CurrentSection != null)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space((GroundWorksEditor.Window.m_inspectorWindowRect.width / 2.0f) - LayerTitleSize);
                        GUILayout.Label(GroundWorksEditor.CurrentSection.gameObject.name + " Properties");
                    }
                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space((GroundWorksEditor.Window.m_inspectorWindowRect.width / 2.0f) - ChunkTitleSize);
                        GUILayout.Label("Section Properties");
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Label("No Section Selected");
                }
            }
            GUILayout.EndVertical();

            //Current Chunk Properties
            GUILayout.BeginVertical("Box");
            {
                if (GroundWorksEditor.CurrentChunk != null)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space((GroundWorksEditor.Window.m_inspectorWindowRect.width / 2.0f) - LayerTitleSize);
                        GUILayout.Label(GroundWorksEditor.CurrentChunk.gameObject.name + " Properties");
                    }
                    GUILayout.EndHorizontal();

                    GroundWorksEditor.CurrentChunk.m_surface.m_heightMap = 
                        (Texture2D)EditorGUILayout.ObjectField("Height Map", GroundWorksEditor.CurrentChunk.m_surface.m_heightMap, typeof(Texture2D), false);
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space((GroundWorksEditor.Window.m_inspectorWindowRect.width / 2.0f) - ChunkTitleSize);
                        GUILayout.Label("Chunk Properties");
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Label("No Chunk Selected");
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
    }
}

