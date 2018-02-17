using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TPTab : Editor
{
    public class TabReturn
    {
        public enum TabState
        {
            Tab,
            Close
        }

        public bool m_result;
        public TabState m_state;
        public string m_name;

        public TabReturn(bool _result, string _name, TabState _state = TabState.Tab)
        {
            m_result = _result;
            m_name = _name;
            m_state = _state;
        }
    }

    public float m_width;
    public string m_name = "";

    public float m_tabRatio = 4.0f;
    public float m_tabPercentage = 0.0f;
    public float m_tabWidth = 0.0f;

    public float m_closeButtonRatio = 1.0f;
    public float m_closeButtonPercentage = 0.0f;
    public float m_closeButtonWidth = 0.0f;

    public TPTab Initialise(float _width, string _name)
    {
        m_width = _width;
        m_name = _name;

        float totalRatio = m_tabRatio + m_closeButtonRatio;

        m_tabPercentage = m_tabRatio / totalRatio;
        m_tabWidth = m_width * m_tabPercentage;

        m_closeButtonPercentage = m_closeButtonRatio / totalRatio;
        m_closeButtonWidth = m_width * m_closeButtonPercentage;

        return this;
    }

    public TabReturn RenderGUI()
    {
        bool result = false;
        TabReturn.TabState state = TabReturn.TabState.Tab;

        GUILayout.BeginHorizontal();
        {
            bool tab = GUILayout.Button(m_name, GUILayout.Width(m_tabWidth));
            GUILayout.Space(-5.0f);
            bool close = GUILayout.Button("X", GUILayout.Width(m_closeButtonWidth));

            if (tab)
            {
                result = true;
            }
            else if(close)
            {
                result = true;
                state = TabReturn.TabState.Close;
            }
        }
        GUILayout.EndHorizontal();

        TabReturn tabReturn = new TabReturn(result, m_name, state);
        return tabReturn;
    }
}
