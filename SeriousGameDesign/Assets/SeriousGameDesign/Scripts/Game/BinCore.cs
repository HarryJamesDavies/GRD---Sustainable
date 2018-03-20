using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinCore : MonoBehaviour
{
    public List<string> m_acceptableRubbish = new List<string>();

    public List<GameObject> m_rubbish = new List<GameObject>();
    public Transform m_dropPoint;

    public bool m_resultBin = false;

    void Awake()
    {
        string description = "Accepts:\n";
        foreach (string type in m_acceptableRubbish)
        {
            description += type + "\n";
        }
        GetComponent<Inspect>().m_description = description;
    }

    void Start()
    {
        if (!m_resultBin)
        {
            BinManager.Instance.AddBin(this);
        }
    }

    public void AddRubbish(GameObject _rubbish)
    {
        _rubbish.transform.position = m_dropPoint.transform.position;
        m_rubbish.Add(_rubbish);
    }

    public void RemoveRubbish(GameObject _rubbish)
    {
        m_rubbish.Remove(_rubbish);
        Destroy(_rubbish);
    }

    public Vector2 GetRatio()
    {
        if (m_rubbish.Count != 0)
        {
            int good = 0;
            int bad = 0;

            foreach (GameObject rubbish in m_rubbish)
            {
                if (m_acceptableRubbish.Contains(rubbish.tag))
                {
                    good++;
                }
                else
                {
                    bad++;
                }
            }

            float total = good + bad;

            return new Vector2(good / total, bad / total);
        }
        return Vector2.zero;
    }

    public bool CheckAcceptable(GameObject _rubbish)
    {
        return m_acceptableRubbish.Contains(_rubbish.tag);
    }

    public string GetInfo()
    {
        string info = "Accepts:";

        foreach (string item in m_acceptableRubbish)
        {
            info += "\n" + item; 
        }

        return info;
    }

    public void EmptyBin()
    {
        for (int rubbishIter = 0; rubbishIter < m_rubbish.Count; rubbishIter++)
        {
            Destroy(m_rubbish[rubbishIter]);
        }
        m_rubbish.Clear();
    }

    public List<GameObject> SortRubbish()
    {
        List<GameObject> sortedRubbish = new List<GameObject>();

        foreach(GameObject rubbish in m_rubbish)
        {
            if (!CheckAcceptable(rubbish))
            {
                sortedRubbish.Add(rubbish);
            }
        }

        foreach (GameObject rubbish in sortedRubbish)
        {
            m_rubbish.Remove(rubbish);
        }

        return sortedRubbish;
    }
}
