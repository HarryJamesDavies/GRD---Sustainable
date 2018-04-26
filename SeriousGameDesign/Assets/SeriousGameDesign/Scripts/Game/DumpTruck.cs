using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpTruck : MonoBehaviour
{
    public List<GameObject> m_debug = new List<GameObject>();

    public Transform m_startPoint;
    public Transform m_movePoint1;
    public Transform m_movePoint2;

    public Vector3 m_binRotation;
    public float m_binMoveSpeed = 0.3f;
    public BinCore m_currentBin;
    public List<BinCore> m_bins = new List<BinCore>();
    private Transform m_rubbishSpawnPoint;
    private Transform m_target;

    private float m_startTime;
    private float m_journeyLength;

    public float m_spawnLength = 1.0f;
    private float m_remainingTime = 0.0f;

    public void AddBin(BinCore _bin)
    {
        _bin.gameObject.SetActive(false);
        m_bins.Add(_bin);
    }

    private void Begin(BinCore _bin)
    {
        m_currentBin = _bin;
        m_currentBin.GetComponent<Rigidbody>().useGravity = false;
        m_currentBin.transform.position = m_startPoint.position;
        m_currentBin.transform.rotation = Quaternion.Euler(m_binRotation);
        m_currentBin.gameObject.SetActive(true);

        m_target = m_movePoint1;
        m_startTime = Time.time;
        m_journeyLength = Vector3.Distance(m_currentBin.transform.position, m_target.position);

        m_rubbishSpawnPoint = m_currentBin.transform.GetChild(0);
        m_remainingTime = m_spawnLength;
    }

    void Update ()
    {
        if(m_currentBin == null)
        {
            if(m_bins.Count > 0)
            {
                Begin(m_bins[0]);
                m_bins.RemoveAt(0);
            }
        }
        else
        {
            HandleBin();
        }
    }

    private void HandleBin()
    {
        if (m_currentBin != null)
        {
            MoveBin();
            SpawnRubbish();
        }
    }

    private void MoveBin()
    {
        float distCovered = (Time.time - m_startTime) * m_binMoveSpeed;
        float fracJourney = distCovered / m_journeyLength;
        m_currentBin.transform.position = Vector3.LerpUnclamped(m_currentBin.transform.position, m_target.position, fracJourney);

        if (Vector3.Distance(m_currentBin.transform.position, m_target.position) <= 0.1f)
        {
            if (m_target == m_movePoint1)
            {
                m_target = m_movePoint2;
            }
            else
            {
                m_target = m_movePoint1;
            }
            m_startTime = Time.time;
            m_journeyLength = Vector3.Distance(m_currentBin.transform.position, m_target.position);
        }
    }

    public void SpawnRubbish()
    {
        m_remainingTime -= Time.deltaTime;
        if (m_remainingTime <= 0.0f && m_currentBin.m_rubbish.Count != 0)
        {
            GameManager.Instance.m_remainingRubbish.Remove(m_currentBin.m_rubbish[0]);
            GameObject rubbish = Instantiate(m_currentBin.m_rubbish[0], m_rubbishSpawnPoint.position, Quaternion.identity, transform);
            rubbish.GetComponent<Rubbish>().m_ignoreManager = true; 
            rubbish.GetComponent<Rigidbody>().useGravity = true;
            m_currentBin.RemoveRubbish(m_currentBin.m_rubbish[0]);

            if (m_currentBin.m_rubbish.Count == 0)
            {
                ResultManager.Instance.Destory(m_currentBin);
                m_currentBin = null;
                //StartCoroutine(DestroyBin(2.0f));
            }
            else
            {
                m_remainingTime = m_spawnLength;
            }
        }
    }

    private IEnumerator DestroyBin(float _waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(_waitTime);
            if (m_currentBin != null)
            {
                ResultManager.Instance.Destory(m_currentBin);
                m_currentBin = null;
            }
        }
    }
}
