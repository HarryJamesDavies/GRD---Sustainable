using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExternalViewer;

public enum ResultState
{
    Idle,
    SpawnTruck,
    MoveToFill,
    FillTruck,
    MoveToDeath
}

public class ResultManager : MonoBehaviour
{
    public static ResultManager Instance;
    public ResultState m_state = ResultState.Idle;

    private Transform m_targetA;
    public Transform m_truckSpawnPointA;
    public Transform m_truckFillPointA;
    public Transform m_truckDeathPointA;

    private Transform m_targetB;
    public Transform m_truckSpawnPointB;
    public Transform m_truckFillPointB;
    public Transform m_truckDeathPointB;

    public GameObject m_dumpTruckPrefab;
    private DumpTruck m_rubbishDumpTruck;
    private DumpTruck m_recyclingDumpTruck;

    public float m_moveToFillSpeed = 1.0f;
    public float m_moveToDeathSpeed = 1.0f;

    private float m_startTimeA;
    private float m_journeyLengthA;

    private float m_startTimeB;
    private float m_journeyLengthB;

    private int m_binIndex = 0;
    public float m_spawnLength = 2.0f;
    private float m_timeRemaining = 0.0f;

    private List<GameObject> m_rubbishToSpawn = new List<GameObject>();
    private List<GameObject> m_rubbishSpawned = new List<GameObject>();

    private bool m_finshFilling = false;

    public GameObject m_resultBinPrefab;
    public List<BinCore> m_bins = new List<BinCore>();
    public List<Transform> m_binSpawnPoints = new List<Transform>();

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
        }
    }

    public void Begin()
    {
        m_state = ResultState.SpawnTruck;
    }

    private void SpawnTrucks()
    {
        m_rubbishDumpTruck = Instantiate(m_dumpTruckPrefab, m_truckSpawnPointA.position,
           m_dumpTruckPrefab.transform.rotation, transform).GetComponent<DumpTruck>();
        m_targetA = m_truckFillPointA;
        m_startTimeA = Time.time;
        m_journeyLengthA = Vector3.Distance(m_rubbishDumpTruck.transform.position, m_targetA.position);

        m_recyclingDumpTruck = Instantiate(m_dumpTruckPrefab, m_truckSpawnPointB.position,
            m_dumpTruckPrefab.transform.rotation, transform).GetComponent<DumpTruck>();
        m_targetB = m_truckFillPointB;
        m_startTimeB = Time.time;
        m_journeyLengthB = Vector3.Distance(m_rubbishDumpTruck.transform.position, m_targetB.position);

        m_state = ResultState.MoveToFill;
    }

    private void SpawnBins()
    {
        for (int binIter = 0; binIter < BinManager.Instance.m_bins.Count; binIter++)
        {
            if (BinManager.Instance.m_bins[binIter].m_rubbish.Count != 0)
            {
                m_bins.Add(Instantiate(m_resultBinPrefab, m_binSpawnPoints[binIter].position,
                m_resultBinPrefab.transform.rotation, transform).GetComponent<BinCore>());
                m_bins[m_bins.Count - 1].m_acceptableRubbish = BinManager.Instance.m_bins[binIter].m_acceptableRubbish;
                m_bins[m_bins.Count - 1].m_rubbish = BinManager.Instance.m_bins[binIter].m_rubbish;
            }
        }
    }

    public void LandFillBin(BinCore _bin)
    {
        foreach (GameObject rubbish in _bin.m_rubbish)
        {
            GameManager.Instance.m_remainingRubbish.Remove(rubbish);
        }
        m_rubbishDumpTruck.AddBin(_bin);
    }

    public void RecycleBin(BinCore _bin)
    {
        foreach (GameObject rubbish in _bin.m_rubbish)
        {
            GameManager.Instance.m_remainingRubbish.Remove(rubbish);
        }
        m_recyclingDumpTruck.AddBin(_bin);
    }

    public void Destory(BinCore _bin)
    {
        m_bins.Remove(_bin);
        Destroy(_bin.gameObject);
    }

    void Update()
    {
        switch(m_state)
        {
            case ResultState.SpawnTruck:
                {
                    SpawnTrucks();
                    SpawnBins();
                    break;
                }
            case ResultState.MoveToFill:
                {
                    if (MoveTruck(m_moveToFillSpeed))
                    {
                        m_state = ResultState.FillTruck;
                    }
                    break;
                }
            case ResultState.FillTruck:
                {
                    if (m_bins.Count == 0)
                    {
                        m_targetA = m_truckDeathPointA;
                        m_startTimeA = Time.time;
                        m_journeyLengthA = Vector3.Distance(m_rubbishDumpTruck.transform.position, m_targetA.position);

                        m_targetB = m_truckDeathPointB;
                        m_startTimeB = Time.time;
                        m_journeyLengthB = Vector3.Distance(m_rubbishDumpTruck.transform.position, m_targetB.position);

                        m_state = ResultState.MoveToDeath;
                    }
                    break;
                }
            case ResultState.MoveToDeath:
                {
                    if (MoveTruck(m_moveToDeathSpeed))
                    {
                        Destroy(m_rubbishDumpTruck.gameObject);
                        m_rubbishDumpTruck = null;
                        Destroy(m_recyclingDumpTruck.gameObject);
                        m_recyclingDumpTruck = null;
                        m_state = ResultState.Idle;
                        GameManager.Instance.ChangeState(GameState.Play);
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private bool MoveTruck(float _speed)
    {
        if(m_rubbishDumpTruck != null)
        {
            float distCoveredA = (Time.time - m_startTimeA) * _speed;
            float fracJourneyA = distCoveredA / m_journeyLengthA;
            m_rubbishDumpTruck.transform.position = Vector3.LerpUnclamped(m_rubbishDumpTruck.transform.position,
                m_targetA.position, fracJourneyA);

            float distCoveredB = (Time.time - m_startTimeA) * _speed;
            float fracJourneyB = distCoveredB / m_journeyLengthB;
            m_recyclingDumpTruck.transform.position = Vector3.LerpUnclamped(m_recyclingDumpTruck.transform.position,
                m_targetB.position, fracJourneyB);

            return Vector3.Distance(m_rubbishDumpTruck.transform.position, m_targetA.position) <= 0.5f;
        }
        return false;
    }
}
