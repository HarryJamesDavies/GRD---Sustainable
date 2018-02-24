using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    public int m_rubbishDestroyed = 0;
    public int m_rubbishRecycled = 0;

    public int m_recyclingDestroyed = 0;
    public int m_recyclingRecycled = 0;

    public int m_objectsInScene = 0;

    void Awake()
    {
        CreateInstance();

    }

    private void CreateInstance()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            GameObject[] trash = GameObject.FindGameObjectsWithTag("Trash");
            GameObject[] recycling = GameObject.FindGameObjectsWithTag("Recycling");
            m_objectsInScene = trash.Length + recycling.Length;
        }
    }

    public void RubbishDestroyed()
    {
        m_rubbishDestroyed++;
        CheckComplete();
    }

    public void RubbishRecycled()
    {
        m_rubbishRecycled++;
        CheckComplete();
    }

    public void RecyclingDestroyed()
    {
        m_recyclingDestroyed++;
        CheckComplete();
    }

    public void RecyclingRecycled()
    {
        m_recyclingRecycled++;
        CheckComplete();
    }

    public void CheckComplete()
    {
        if(m_recyclingDestroyed + m_recyclingRecycled + m_rubbishDestroyed + m_rubbishRecycled == m_objectsInScene)
        {
            GameManager.Instance.m_recyclingDestroyed = m_recyclingDestroyed;
            GameManager.Instance.m_recyclingRecycled = m_recyclingRecycled;
            GameManager.Instance.m_rubbishDestroyed = m_rubbishDestroyed;
            GameManager.Instance.m_rubbishRecycled = m_rubbishRecycled;
            SceneChanger.TransitionScene("GameOver");
        }
    }
}
