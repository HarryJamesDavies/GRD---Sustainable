using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExternalViewer
{
    [Serializable]
    public class PieChart : MonoBehaviour
    {
        public List<float> m_values = new List<float>();
        public List<Color> m_colours = new List<Color>();
        public Wedge m_wedgePrefab;
        public int m_reserveWedges = 20;

        private List<Wedge> m_wedges = new List<Wedge>();
        public float m_radius = 500.0f;

        void Start()
        {
            for (int wedgeIter = 0; wedgeIter < m_reserveWedges; wedgeIter++)
            {
                m_wedges.Add(Instantiate(m_wedgePrefab) as Wedge);
                m_wedges[wedgeIter].transform.SetParent(transform);
                m_wedges[wedgeIter].gameObject.SetActive(false);
                m_wedges[wedgeIter].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }

        public void Draw(List<float> _values)
        {
            if (m_wedges.Count != 0)
            {
                float zRotation = 0.0f;
                float total = 0.0f;
                _values.ForEach(delegate (float value)
                {
                    total += value;
                });

                for (int valueIter = 0; valueIter < _values.Count; valueIter++)
                {
                    if (_values[valueIter] != 0.0f)
                    {
                        Wedge wedge = m_wedges[valueIter];
                        m_wedges[valueIter].gameObject.SetActive(true);

                        wedge.m_wedge.color = m_colours[valueIter % m_colours.Count];
                        wedge.m_wedge.fillAmount = _values[valueIter] / total;

                        RectTransform rt = wedge.GetComponent<RectTransform>();

                        rt.sizeDelta = new Vector2(m_radius, m_radius);
                        wedge.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, zRotation));

                        wedge.m_value.text = _values[valueIter].ToString("F2");
                        wedge.m_value.rectTransform.rotation = Quaternion.Euler(Vector3.zero);

                        wedge.m_value.rectTransform.anchoredPosition = Vector2.zero;
                        if ((zRotation > -90.0f) || (zRotation < -270.0f))
                        {
                            wedge.m_value.rectTransform.anchoredPosition = new Vector2(0.0f, 20.0f);
                        }

                        zRotation -= wedge.m_wedge.fillAmount * 360.0f;
                    }
                }
            }
        }
    }
}