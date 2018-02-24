using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExternalViewer
{
    [Serializable]
    public class BarChart : MonoBehaviour
    {
        public Bar m_barPrefab;

        private List<Bar> m_bars = new List<Bar>();
        private float m_chartHeight;

        public List<float> m_inputValues = new List<float>();
        public List<string> m_labels = new List<string>();
        public List<Color> m_colours = new List<Color>();

        public int m_reserveWedges = 10;
        public Transform m_inactiveBarHolder;

        void Start()
        {
            m_chartHeight = (Screen.height + GetComponent<RectTransform>().sizeDelta.y) / 2.0f;

            for (int barIter = 0; barIter < m_reserveWedges; barIter++)
            {
                m_bars.Add(Instantiate(m_barPrefab) as Bar);
                m_bars[barIter].gameObject.SetActive(false);
                m_bars[barIter].transform.SetParent(m_inactiveBarHolder);
            }
        }

        public void Draw(List<float> _values, List<string> _labels)
        {
            if (m_bars.Count != 0)
            {
                float maxValue = _values.Max();

                for (int valueIter = 0; valueIter < _values.Count; valueIter++)
                {
                    Bar bar = m_bars[valueIter];
                    bar.transform.SetParent(transform);
                    bar.gameObject.SetActive(true);

                    RectTransform rt = bar.m_bar.rectTransform;
                    float normalizedValue = (_values[valueIter] / maxValue) * 0.95f;
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, m_chartHeight * normalizedValue);
                    bar.m_bar.color = m_colours[valueIter % m_colours.Count];

                    bar.m_label.text = "UNDEFINED";
                    if (valueIter < _labels.Count)
                    {
                        bar.m_label.text = _labels[valueIter];
                    }

                    bar.m_value.text = _values[valueIter].ToString("F2");
                    bar.m_value.rectTransform.pivot = new Vector2(0.5f, 0.0f);
                    bar.m_value.rectTransform.anchoredPosition = Vector2.zero;
                }
            }
        }
    }
}
