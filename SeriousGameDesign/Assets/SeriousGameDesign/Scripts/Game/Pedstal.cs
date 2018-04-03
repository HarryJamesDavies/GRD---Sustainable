using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Pedstal
{
    public GameObject m_objectPanel;
    public int m_objectIndex = 0;
    public Transform m_position;
    public Image m_icon;
    public Text m_count;
    public Text m_weight;
}
