using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExternalViewer;

public class BinAnalyser : MonoBehaviour
{
    public GameObject m_chartHolder;
    public PieChart m_chart;
    public List<float> m_values = new List<float>();
    private BinCore m_bin;

    public Button m_landFillButton;
    public Button m_sortButton;
    public Button m_recycleButton;

    public float m_minimumRatio = 0.1f;

    void OnTriggerEnter(Collider _other)
    {
        m_bin = _other.gameObject.GetComponentInParent<BinCore>();
        if (m_bin != null)
        {
            Vector2 ratio = m_bin.GetRatio();

            m_values.Clear();
            m_values.Add(ratio.x);
            m_values.Add(ratio.y);
            if (ratio == Vector2.zero)
            {
                m_values.Add(1.0f);
            }
            else if(ratio.y <= m_minimumRatio)
            {
                m_recycleButton.gameObject.SetActive(true);
            }
            else if (ratio.y != 1.0f)
            {
                m_sortButton.gameObject.SetActive(true);
            }

            m_landFillButton.gameObject.SetActive(true);
            m_chartHolder.SetActive(true);
        }
    }

    void Update()
    {
        if(m_bin != null)
        {
            m_chart.Draw(m_values, new Vector3(0.0f, 90.0f, 0.0f), false);
        }
    }

    void OnTriggerExit(Collider _other)
    {
        if (m_bin != null)
        {
            ResetAnalyser();
        }
    }

    public void LandFill()
    {
        foreach(GameObject rubbish in m_bin.m_rubbish)
        {
            if(rubbish.CompareTag("Standard"))
            {
                GameManager.Instance.m_landFilledRubbish.Add(rubbish);
            }
            else
            {
                GameManager.Instance.m_landFilledRecycling.Add(rubbish);
            }
        }
        ResultManager.Instance.LandFillBin(m_bin);
        ResetAnalyser();
    }

    public void Sort()
    {
        SortedRubbish sortedRubbish = m_bin.SortRubbish();
        GameManager.Instance.m_sortedRubbish.AddRange(sortedRubbish.m_rubbish);
        GameManager.Instance.m_sortedRecycling.AddRange(sortedRubbish.m_recycing);

        Vector2 ratio = m_bin.GetRatio();

        m_values.Clear();
        m_values.Add(ratio.x);
        m_values.Add(ratio.y);
        if (ratio == Vector2.zero)
        {
            m_values.Add(1.0f);
        }
        else if (ratio.y <= m_minimumRatio)
        {
            m_recycleButton.gameObject.SetActive(true);
        }
        else if(ratio.y != 1.0f)
        {
            m_sortButton.gameObject.SetActive(true);
        }
    }

    public void Recycle()
    {
        GameManager.Instance.m_recycledRubbish.AddRange(m_bin.m_rubbish);
        ResultManager.Instance.RecycleBin(m_bin);
        ResetAnalyser();
    }

    private void ResetAnalyser()
    {
        m_bin = null;
        m_values.Clear();
        m_chartHolder.SetActive(false);
        m_landFillButton.gameObject.SetActive(false);
        m_sortButton.gameObject.SetActive(false);
        m_recycleButton.gameObject.SetActive(false);
    }
}
