using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TPTabBar : Editor
{
    public float m_minimumTabWidth = 100.0f;
    public float m_maximumTabWidth = 20.0f;

    public List<TPTab> m_tabs = new List<TPTab>();

    public TPTabBar Initialise()
    {
        m_tabs.Clear();
        return this;
    }

    public void RenderGUI()
    {
        GUILayout.BeginHorizontal();
        {
            foreach(TPTab tab in m_tabs)
            {
                tab.RenderGUI();
            }
        }
        GUILayout.EndHorizontal();
    }

    public void AddTab(string _name)
    {
        m_tabs.Add(CreateInstance<TPTab>().Initialise(m_minimumTabWidth, _name));
    }

    public void AddTabRange(List<string> _names)
    {
        foreach (string name in _names)
        {
            m_tabs.Add(CreateInstance<TPTab>().Initialise(m_minimumTabWidth, name));
        }
    }
}
