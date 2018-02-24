using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExternalViewer
{
    [Serializable]
    public class LineGraph : MonoBehaviour
    {
        [SerializeField] public List<LinePoints> m_values = new List<LinePoints>();
        public List<Color> m_colours = new List<Color>();
        public Line m_linePrefab;
        public int m_reserveLines = 20;

        private List<Line> m_lines = new List<Line>();

        void Start()
        {
            for (int lineIter = 0; lineIter < m_reserveLines; lineIter++)
            {
                m_lines.Add(Instantiate(m_linePrefab) as Line);
                m_lines[lineIter].transform.SetParent(transform);
                m_lines[lineIter].GetComponent<RectTransform>().position = new Vector3(-168,
                    -162, lineIter);
            }
        }

        public void Draw(List<LinePoints> _values)
        {
            if (m_lines.Count != 0)
            {
                for (int lineIter = 0; lineIter < _values.Count; lineIter++)
                {
                    Line line = m_lines[lineIter];
                    line.m_line.positionCount = 0;
                }

                for (int valueIter = 0; valueIter < _values.Count; valueIter++)
                {
                    Line line = m_lines[valueIter];
                    line.m_line.positionCount = _values[valueIter].m_points.Count;
                    line.m_line.SetPositions(_values[valueIter].m_points.ToArray());
                    line.m_line.material.color = m_colours[valueIter % m_colours.Count];
                    line.m_line.material.SetColor("_EmissionColor", m_colours[valueIter % m_colours.Count]);
                }
            }
        }
    }


    [Serializable]
    public class LinePoints
    {
        [SerializeField] public List<Vector3> m_points = new List<Vector3>();
    }
}
