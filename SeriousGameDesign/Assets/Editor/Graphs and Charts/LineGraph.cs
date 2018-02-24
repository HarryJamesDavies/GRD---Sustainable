using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Line
{
    public string m_name;
    public Color m_colour;
    public float m_width;
    public Vector3[] m_points;

    public Line(string _name, Color _colour, float _width, Vector3[] _points)
    {
        m_name = _name;
        m_colour = _colour;
        m_width = _width;
        m_points = _points;
    }
}

public class LineGraph : Editor
{
    public string m_graphName;

    public List<Vector2> m_points = new List<Vector2>();
    public Rect m_graphDimensions;

    private const float OriginOffsetX = 30.0f;
    private const float OriginOffsetY = 30.0f;

    private const float AxisWidth = 5.0f;
    private const float AxisBarOffset = 3.0f;

    private float m_endSpacing = 10.0f;
    private const float m_areaSpacing = 3.0f;

    private Vector2 m_titleBarPercentage = new Vector2(1.0f, 0.1f);
    private Vector2 m_graphPercentage = new Vector2(0.8f, 0.9f);
    private Vector2 m_keyPercentage = new Vector2(0.2f, 0.9f);

    private float m_lineWidth = 5.0f;
    private List<Line> m_lines = new List<Line>();

    public static Color[] m_colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.white, Color.cyan, Color.gray, Color.black, };

    public void AddPoint(Vector2 _point)
    {
        int insertIndex = 0;
        for(int checkIter = 0; checkIter < m_points.Count; checkIter++)
        {
            if(m_points[checkIter].x > _point.x)
            {
                insertIndex = checkIter;
                break;
            }
        }

        m_points.Insert(insertIndex, _point);
    }

    public void ClearPoints()
    {
        m_points.Clear();
    }

    public void UpdateData(List<Vector2> _points)
    {
        for (int pointIter = 0; pointIter < m_points.Count; pointIter++)
        {
            m_points[pointIter] = new Vector2(m_points[pointIter].x, m_points[pointIter].y);
        }
    }

    public void Draw(string _graphName, Rect _graphDimensions, List<Line> _lines)
    {
        m_graphName = _graphName;
        m_graphDimensions = _graphDimensions;
        m_lines = _lines;

        GUILayout.BeginArea(new Rect(m_graphDimensions));
        {
            EditorGUILayout.BeginVertical("box", GUILayout.Width(m_graphDimensions.width), GUILayout.Height(m_graphDimensions.height));
            {
                GUILayout.Space(-m_areaSpacing);
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(-m_areaSpacing);
                    DrawTitleBar();
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(-m_areaSpacing - 1.0f);
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(-m_areaSpacing);
                    DrawGraph();

                    GUILayout.Space(-m_areaSpacing - 1.0f);
                    DrawKey();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
        GUILayout.EndArea();
    }

    private void DrawTitleBar()
    {
        EditorGUILayout.BeginHorizontal("box", GUILayout.Width(m_graphDimensions.width * m_titleBarPercentage.x),
            GUILayout.Height(m_graphDimensions.height * m_titleBarPercentage.y));
        {
            GUILayout.Space((m_graphDimensions.width * m_titleBarPercentage.x) / 2.5f);

            EditorGUILayout.BeginVertical();
            {
                GUILayout.Space((m_graphDimensions.height * m_titleBarPercentage.y) / 5.0f);
                GUILayout.Label(m_graphName);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawGraph()
    {
        Vector3 m_origin = new Vector3(OriginOffsetX + AxisBarOffset, 
            m_graphDimensions.height - (OriginOffsetY + AxisBarOffset), 1.0f);
        Vector3 m_maximum = new Vector3((m_graphDimensions.width * m_graphPercentage.x) - m_endSpacing,
            m_graphDimensions.height - (m_graphDimensions.height * m_graphPercentage.y) + m_endSpacing, 1.0f);

        EditorGUILayout.BeginHorizontal("box", GUILayout.Width(m_graphDimensions.width * m_graphPercentage.x),
            GUILayout.Height(m_graphDimensions.height * m_graphPercentage.y));
        {
            GUILayout.Label("");

            Handles.color = Color.black;
            Vector3[] XAxisPoints = {
                new Vector3(OriginOffsetX, m_graphDimensions.height - (OriginOffsetY / 3.0f), 1.0f),
                new Vector3(OriginOffsetX, m_graphDimensions.height - (m_graphDimensions.height * m_graphPercentage.y) + m_endSpacing, 1.0f)
                };
            Handles.DrawAAPolyLine(AxisWidth, XAxisPoints);

            Vector3[] YAxisPoints = {
                new Vector3(OriginOffsetX / 3.0f, m_graphDimensions.height - (OriginOffsetY), 1.0f),
                new Vector3((m_graphDimensions.width * m_graphPercentage.x) - m_endSpacing, m_graphDimensions.height - (OriginOffsetY), 1.0f)
                };
            Handles.DrawAAPolyLine(AxisWidth, YAxisPoints);

            Vector3 offset = new Vector3(OriginOffsetX + AxisBarOffset, m_graphDimensions.height - (OriginOffsetY + AxisBarOffset), 1.0f);
            foreach(Line line in m_lines)
            {
                for(int pointIter = 0; pointIter < line.m_points.Length; pointIter++)
                {
                    line.m_points[pointIter].x += offset.x;
                    line.m_points[pointIter].y = offset.y - line.m_points[pointIter].y;
                    line.m_points[pointIter].z = offset.z;
                }
                DrawLine(line);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawLine(Line _line)
    {
        Handles.color = _line.m_colour;
        Handles.DrawAAPolyLine(_line.m_width, _line.m_points);
    }

    private void DrawKey()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.Width(m_graphDimensions.width * m_keyPercentage.x),
            GUILayout.Height(m_graphDimensions.height * m_keyPercentage.y));
        {
            GUILayout.Label("Key:");            

            GUIStyle style = new GUIStyle();
            foreach (Line line in m_lines)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(3.0f);

                    style.normal.textColor = line.m_colour;
                    GUILayout.Label(line.m_name, style);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// Generates a random color (Alpha = 1.0)
    /// </summary>
    /// <returns></returns>
    public static Color GetRandomColor()
    {
        Color colour;
        colour.r = Random.Range(0.0f, 1.0f);
        colour.b = Random.Range(0.0f, 1.0f);
        colour.g = Random.Range(0.0f, 1.0f);
        colour.a = 1.0f;
        return colour;
    }
}
