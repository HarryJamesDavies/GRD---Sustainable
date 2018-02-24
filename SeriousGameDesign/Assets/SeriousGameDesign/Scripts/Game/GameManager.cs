using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int m_rubbishDestroyed = 0;
    public int m_rubbishRecycled = 0;

    public int m_recyclingDestroyed = 0;
    public int m_recyclingRecycled = 0;

    void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if(Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void RubbishDestroyed()
    {
        m_rubbishDestroyed++;
    }

    public void RubbishRecycled()
    {
        m_rubbishRecycled++;
    }

    public void RecyclingDestroyed()
    {
        m_recyclingDestroyed++;
    }

    public void RecyclingRecycled()
    {
        m_recyclingRecycled++;
    }
}