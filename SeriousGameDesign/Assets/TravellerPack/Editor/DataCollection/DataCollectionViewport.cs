using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataCollectionViewport : Editor
{
    public enum GraphType
    {
        Pie,
        Line
    }

    private GraphType m_graphType = GraphType.Pie;

    private PieChart m_pieChart = null;
    private Rect m_pieChartRect;

    private LineGraph m_lineGraph = null;
    private Rect m_lineGraphRect;

    private Rect m_graphTypeRect = new Rect(new Vector2(50.0f, 0.0f), new Vector2(100.0f, 40.0f));

    private List<Color> m_colours = new List<Color>();

    public void DoWindow(int _unusedWindowID)
    {
        GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);

        EditorGUILayout.BeginVertical();
        {
            m_graphType = (GraphType)EditorGUILayout.EnumPopup(m_graphType);

            if (RetensionTracker.Instance != null)
            {
                switch (m_graphType)
                {
                    case GraphType.Pie:
                        {
                            DrawPieGraph();
                            break;
                        }
                    case GraphType.Line:
                        {
                            DrawLineGraph();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
        EditorGUILayout.EndVertical();

        GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);
    }

    private void DrawPieGraph()
    {
        if (m_pieChart == null)
        {
            m_pieChart = CreateInstance<PieChart>();
            m_pieChartRect = new Rect(25.0f, 25.0f, 0.0f, 0.0f);
        }

        int difference = RetensionTracker.Instance.m_data.Count - m_colours.Count;
        if (difference > 0)
        {
            m_colours.Add(PieChart.GetRandomColor());
        }

        RetensionTracker.Instance.CalculateAllLengths();

        List<PieChart.PieSection> sections = new List<PieChart.PieSection>();
        for (int sectionIter = 0; sectionIter < RetensionTracker.Instance.m_data.Count; sectionIter++)
        {
            Color colour;
            if(sectionIter < PieChart.m_colors.Length)
            {
                colour = PieChart.m_colors[sectionIter];
            }
            else
            {
                colour = PieChart.GetRandomColor();
            }

            sections.Add(new PieChart.PieSection(RetensionTracker.Instance.m_data[sectionIter].m_dataName,
                RetensionTracker.Instance.m_retensionPecentages[sectionIter] * 360.0f, colour));
        }

        m_pieChartRect.x = DataCollectionEditor.m_viewportRect.width * 0.025f;
        m_pieChartRect.y = DataCollectionEditor.m_viewportRect.height * 0.035f;
        m_pieChartRect.width = DataCollectionEditor.m_viewportRect.width * 0.95f;
        m_pieChartRect.height = DataCollectionEditor.m_viewportRect.height * 0.95f;
        m_pieChart.Draw(m_pieChartRect, sections);
    }

    private List<float> MultiplyList(List<float> _values, float _multiplier)
    {
        List<float> results = new List<float>();

        foreach (float value in _values)
        {
            results.Add(value * _multiplier);
        }

        return results;
    }

    private void DrawLineGraph()
    {
        if (m_lineGraph == null)
        {
            m_lineGraph = CreateInstance<LineGraph>();
            m_lineGraphRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
        }

        List<Line> lines = new List<Line>();

        int dataIter = 0;
        foreach (RetensionData data in RetensionTracker.Instance.m_data)
        {
            List<Vector3> points = new List<Vector3>();

            Color colour;
            if (dataIter < LineGraph.m_colors.Length)
            {
                colour = LineGraph.m_colors[dataIter];
            }
            else
            {
                colour = LineGraph.GetRandomColor();
            }

            foreach (TimeChunk chunk in data.m_timeChunks)
            {

                if (chunk.m_chunkLength != 0.0f)
                {
                    points.Add(new Vector3(chunk.m_startTime * 10.0f, chunk.m_chunkLength * 10.0f, 1.0f));
                }
            }
            lines.Add(new Line(data.m_dataName, colour, 10.0f, points.ToArray()));
            dataIter++;
        }

        m_lineGraphRect.x = DataCollectionEditor.m_viewportRect.width * 0.1f;
        m_lineGraphRect.y = DataCollectionEditor.m_viewportRect.height * 0.1f;
        m_lineGraphRect.width = DataCollectionEditor.m_viewportRect.width * 0.8f;
        m_lineGraphRect.height = DataCollectionEditor.m_viewportRect.height * 0.8f;
        m_lineGraph.Draw("Retension Data", m_lineGraphRect, lines);
    }

    private List<Vector2> GetPointList(RetensionData _data)
    {
        List<Vector2> results = new List<Vector2>();

        foreach (TimeChunk chunk in _data.m_timeChunks)
        {
            results.Add(new Vector2(chunk.m_startTime, chunk.m_chunkLength));
        }

        return results;
    }
}
