using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutcomeManager : MonoBehaviour
{
    public static OutcomeManager Instance = null;

    public Animator m_truck;
    public Animator m_piston;
    public Animator m_bin;

    public Transform m_rubbishSpawnPoint;
    public float m_remainingTime = 0.0f;
    public float m_spawnLength = 0.0f;
    public float m_totalSpawnLength = 0.0f;

    public List<GameObject> m_totalRubbish = new List<GameObject>();
    public List<GameObject> m_currentRubbish = new List<GameObject>();
    public bool m_spawnRubbish = false;
    public bool m_rubbishSpawned = false;
    public bool m_showOutcome = false;
    public bool m_outcomeShown = false;

    public RubbishLevel m_rubbishDump;

    public int m_rubbishPerRound;
    public int m_maxDumpPerRound = 10;
    public int m_households = 100;

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
        }
    }

    public void Begin()
    {
        m_totalRubbish.AddRange(GameManager.Instance.m_landFilledRubbish);
        m_totalRubbish.AddRange(GameManager.Instance.m_landFilledRecycling);
        m_totalRubbish.AddRange(GameManager.Instance.m_sortedRubbish);
        m_currentRubbish.AddRange(m_totalRubbish);

        if (m_currentRubbish.Count > m_maxDumpPerRound)
        {
            m_rubbishPerRound = Mathf.CeilToInt(m_currentRubbish.Count / Mathf.CeilToInt((float)m_currentRubbish.Count / m_maxDumpPerRound));
        }
        m_truck.Play("MoveTruckIn");
    }
	
	public void OnTruckInFinish()
    {
        m_piston.Play("MovePistonUp");
    }

    public void OnPistonUpFinish()
    {
        m_bin.Play("MoveBinIn");
    }

    public void OnBinIn()
    {
        m_bin.Play("DumpLoad");
    }

    public void OnDumpBegin(float _length)
    {
        m_totalSpawnLength = _length;
        if(m_currentRubbish.Count == 0)
        {
            m_remainingTime = m_totalSpawnLength + 1.0f;
        }
        else if (m_currentRubbish.Count > m_maxDumpPerRound)
        {
            m_spawnLength = m_totalSpawnLength / m_rubbishPerRound;
            m_remainingTime = 0.0f;
        }
        else
        {
            m_spawnLength = m_totalSpawnLength / m_currentRubbish.Count;
            m_remainingTime = 0.0f;
        }
        m_spawnRubbish = true;
    }

    public void OnDumpEnd()
    {
        if(m_currentRubbish.Count > 0)
        {
            m_bin.Play("DumpLoad", -1, 0.0f);
            m_bin.Play("DumpLoad");
        }
        else
        {
            m_bin.Play("MoveBinOut");
        }
        m_spawnRubbish = false;
    }

    public void OnBinOut()
    {
        m_piston.Play("MovePistonDown");

        if (!m_outcomeShown)
        {
            HandleOutcome();
        }
    }

    void Update()
    {
        if (m_spawnRubbish)
        {
            SpawnRubbish();
        }
    }

    public void SpawnRubbish()
    {
        m_remainingTime -= Time.deltaTime;
        if (m_remainingTime <= 0.0f && m_currentRubbish.Count != 0)
        {

            GameObject rubbish = Instantiate(m_currentRubbish[0], m_rubbishSpawnPoint.position, Quaternion.identity, transform);
            rubbish.GetComponent<Rigidbody>().useGravity = true;
            m_currentRubbish.RemoveAt(0);

            if (m_currentRubbish.Count == 0)
            {
                m_spawnRubbish = false;
            }
            else
            {
                m_remainingTime = m_spawnLength;
            }
        }
    }

    private void HandleOutcome()
    {
        m_outcomeShown = true;

        for (int houseIter = 0; houseIter < m_households; houseIter++)
        {
            m_currentRubbish.AddRange(m_totalRubbish);
        }

        if (m_currentRubbish.Count > m_maxDumpPerRound)
        {
            m_rubbishPerRound = Mathf.CeilToInt(m_currentRubbish.Count / Mathf.CeilToInt((float)m_currentRubbish.Count / m_maxDumpPerRound));
        }

        m_truck.Play("MovePistonUp");
    }
}
