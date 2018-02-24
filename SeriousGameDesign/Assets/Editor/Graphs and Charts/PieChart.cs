//================================== Analytics ==================================//
//
// Author:  Harry Davies
// Purpose: Creates a Pie Chart from angles for display in a custom window
//
//================================================================================//
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PieChart : Editor
{
    public class PieSection
    {
        public PieSection(string _name, float _angle, Color _color)
        {
            m_name = _name;
            m_angle = _angle;
            m_color = _color;
        }

        public string m_name;
        public float m_angle;
        public Color m_color;
    }

    public static Color[] m_colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.white, Color.cyan, Color.gray, Color.black, };

    public List<PieSection> m_sections = new List<PieSection>();

    public Rect m_chartDimensions;

    public float m_centerSnapDistance = 100.0f;

    public float m_keyWidth = 10.0f;
    public float m_keySectionHeight = 17.5f;
    public bool m_keyDropDown = true;

    /// <summary>
    /// Adds a new section to Pie Chart based on angle (Degrees).
    /// </summary>
    public void AddSection(string _name, float _angle)
    {
        m_sections.Add(new PieSection(_name, _angle, GetColor(m_sections.Count)));
    }

    public void UpdateData(List<float> _angles)
    {
        for(int sectionIter = 0; sectionIter < m_sections.Count; sectionIter++)
        {
            m_sections[sectionIter].m_angle = _angles[sectionIter];
        }
    }

    /// <summary>
    /// Removes all sections from Pie Chart.
    /// </summary>
    public void ClearSections()
    {
        m_sections.Clear();
    }

    /// <summary>
    /// Draws Pie Chart inside specified Rect
    /// </summary>
    public void Draw(Rect _chartDimensions, List<PieSection> _sections)
    {
        m_chartDimensions = _chartDimensions;
        m_sections = _sections;

        DrawPieChart();

        //Draws chart key
        DrawKey();
    }

    /// <summary>
    /// Draws Key with section names and colours
    /// </summary>
    void DrawKey()
    {
        //Draw key box
        GUI.backgroundColor = Color.white;
        Rect ViewPort = (Rect)EditorGUILayout.BeginVertical("Box", GUILayout.Width(250.0f), GUILayout.Height(m_keySectionHeight * (m_sections.Count + 1)));
        {
            //Draws key title
            EditorGUILayout.LabelField("Key:", EditorStyles.boldLabel);

            //Loops through sections adding their values to the key
            int index = 2;
            foreach (PieSection section in m_sections)
            {
                EditorGUILayout.BeginHorizontal();

                //Adds entery text, including name and percentage
                float percent = (section.m_angle / 360.0f) * 100.0f;
                EditorGUILayout.LabelField(section.m_name + ":  " + percent.ToString("0.00") + "%   ");

                //Draws color box to indicate what section relates to  which entery
                Rect boxRect = new Rect(ViewPort.xMax - 13.0f, ViewPort.yMin + ((m_keySectionHeight * index) - (m_keySectionHeight / 2)), 10.0f, 10.0f);
                Handles.color = section.m_color;
                Handles.DrawSolidRectangleWithOutline(boxRect, section.m_color, Color.black);
                EditorGUILayout.EndHorizontal();
                index++;
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawPieChart()
    {
        //Sets the center of the Pie chart at an offset to prevent key overlap
        Vector3 center = m_chartDimensions.center/* + _offset*/;

        //If radius is greater then chart center to edge, snap chart center to the ViewPort's center
        if (center.x - m_chartDimensions.center.x <= m_centerSnapDistance)
        {
            center = m_chartDimensions.center;
        }

        //Calculate positional vectors relative to viewport
        Vector3 up = center - new Vector3(center.x, (center.y + m_chartDimensions.height));
        Vector3 forward = new Vector3(center.x, center.y, 0.0f) - new Vector3(center.x, center.y, 1.0f);
        Vector3 from = Vector3.zero;

        //Calculate radius reltive to viewport dimensions
        float radius = m_chartDimensions.width - m_chartDimensions.width / 1.75f;
        if (m_chartDimensions.height < m_chartDimensions.width)
        {
            radius = m_chartDimensions.height - m_chartDimensions.height / 1.75f;
        }

        //Draw section at rotation around the circle
        float rotation = 0.0f;
        foreach (PieSection section in m_sections)
        {
            //Returns the starting rotation of next section
            rotation = DrawPieSection(section.m_angle, rotation, radius, center, up, forward, from, section.m_color);
        }

        //Draws outline around edge of the circle
        Handles.color = Color.black;
        Handles.CircleHandleCap(0, center, Quaternion.identity, radius, EventType.Ignore);
    }

    /// <summary>
    /// Draws Pie section at rotation around the circle
    /// </summary>
    /// <param name="_angle"></param>
    /// <param name="_rotation"></param>
    /// <param name="_radius"></param>
    /// <param name="_center"></param>
    /// <param name="_up"></param>
    /// <param name="_forward"></param>
    /// <param name="_from"></param>
    /// <param name="_color"></param>
    /// <returns></returns>
    float DrawPieSection(float _angle, float _rotation, float _radius, Vector3 _center, Vector3 _up, Vector3 _forward, Vector3 _from, Color _color)
    {
        Handles.color = _color;

        //Adds current section angle to previous rotation
        float rotation = _rotation + _angle;

        //Converts rotation float to vector3 via euler angles
        _from = Quaternion.Euler(0.0f, 0.0f, rotation) * _up;

        //Draws pie section based on angle and position vectors
        Handles.DrawSolidArc(_center, _forward, _from, _angle, _radius);
        Handles.color = Color.black;

        //Draws edge to mark the end of a section
        float angle = (rotation - 90.0f) / 180;
        Vector3 edge = new Vector2(((_radius * Mathf.Cos((angle * Mathf.PI))) + _center.x), ((_radius * Mathf.Sin((angle * Mathf.PI))) + _center.y));
        Handles.DrawLine(_center, edge);
        return rotation;
    }

    /// <summary>
    /// Gets next avaible color on list (loops)
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    Color GetColor(int _index)
    {
        int index = _index % m_colors.Length;
        return m_colors[index];
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
