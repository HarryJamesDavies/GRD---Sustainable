using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    //public Transform m_rubbishSpawnPoint;
    //public Transform m_recyclingSpawnPoint;
    //public Vector4 m_spawnPositionOffset = new Vector4(-1.0f, 1.0f, -1.0f, 1.0f);

    //public GameObject m_rubbishPrefab;
    //public GameObject m_recyclePrefab;

    //private int m_rubbishRecyclingToSpawn = 0;
    //private int m_rubbishRubbishToSpawn = 0;
    //private int m_recycleRecyclingToSpawn = 0;
    //private int m_recycleRubbishToSpawn = 0;

    //public Text m_rubbishRecycleText;
    //public Text m_rubbishRubbishText;

    //public Text m_recyclingRecycleText;
    //public Text m_recyclingRubbishText;

    //public float m_spawnCooldownLength = 2.0f;
    //private float m_spawnCooldown = 0.0f;

    //private float m_transitionCountdownLength = 3.0f;
    //private float m_transitionCountdown = 0.0f;
    //private bool m_gameoverComplete = false;

    //void Start()
    //{
    //    if (GameManager.Instance)
    //    {
    //        m_rubbishRecyclingToSpawn = GameManager.Instance.m_recyclingDestroyed;
    //        m_rubbishRecycleText.text = ": " + GameManager.Instance.m_recyclingDestroyed;

    //        m_rubbishRubbishToSpawn = GameManager.Instance.m_rubbishDestroyed;
    //        m_rubbishRubbishText.text = ": " + GameManager.Instance.m_rubbishDestroyed;

    //        m_recycleRecyclingToSpawn = GameManager.Instance.m_recyclingRecycled;
    //        m_recyclingRecycleText.text = ": " + GameManager.Instance.m_recyclingRecycled;

    //        m_recycleRubbishToSpawn = GameManager.Instance.m_rubbishRecycled;
    //        m_recyclingRubbishText.text = ": " + GameManager.Instance.m_rubbishRecycled;

    //        DataHandler.SaveCategoricData("TestData " + Time.time, DataHandler.OutDataPath, GenerateData());
    //    }
    //}

    //private List<CategoricData.CategoricPair> GenerateData()
    //{
    //    List<CategoricData.CategoricPair> data = new List<CategoricData.CategoricPair>();
    //    data.Add(new CategoricData.CategoricPair("RecyclingDestroyed", GameManager.Instance.m_recyclingDestroyed));
    //    data.Add(new CategoricData.CategoricPair("RecyclingRecycled", GameManager.Instance.m_recyclingRecycled));
    //    data.Add(new CategoricData.CategoricPair("RubbishDestroyed", GameManager.Instance.m_rubbishDestroyed));
    //    data.Add(new CategoricData.CategoricPair("RubbishRecycled", GameManager.Instance.m_rubbishRecycled));
    //    return data;
    //}

    //void Update()
    //{
    //    if (!m_gameoverComplete)
    //    {
    //        m_spawnCooldown -= Time.deltaTime;
    //        if (m_spawnCooldown <= 0.0f)
    //        {
    //            GameObject recycyling = RecyclingOrRubbish(true);
    //            if (recycyling)
    //            {
    //                Instantiate(recycyling, m_recyclingSpawnPoint.position + GetSpawnPositionOffset(), Quaternion.identity);
    //            }

    //            GameObject rubbish = RecyclingOrRubbish(false);
    //            if (rubbish)
    //            {
    //                Instantiate(rubbish, m_rubbishSpawnPoint.position + GetSpawnPositionOffset(), Quaternion.identity);
    //            }

    //            m_spawnCooldown = m_spawnCooldownLength;
    //        }
    //        CheckComplete();
    //    }
    //    else
    //    {
    //        m_transitionCountdown -= Time.deltaTime;
    //        if(m_transitionCountdown <= 0.0f)
    //        {
    //            SceneChanger.TransitionScene("Main");
    //        }
    //    }
    //}

    //private GameObject RecyclingOrRubbish(bool _recyclingOrRubbish)
    //{
    //    if (_recyclingOrRubbish)
    //    {
    //        if ((m_recycleRecyclingToSpawn == 0) && (m_recycleRubbishToSpawn == 0))
    //        {
    //            return null;
    //        }
    //        else if (m_recycleRecyclingToSpawn == 0)
    //        {
    //            m_recycleRubbishToSpawn--;
    //            return m_rubbishPrefab;
    //        }
    //        else if (m_recycleRubbishToSpawn == 0)
    //        {
    //            m_recycleRecyclingToSpawn--;
    //            return m_recyclePrefab;
    //        }
    //        else
    //        {
    //            int state = Random.Range(0, 2);
    //            if (state == 0)
    //            {
    //                m_recycleRecyclingToSpawn--;
    //                return m_recyclePrefab;
    //            }
    //            else
    //            {
    //                m_recycleRubbishToSpawn--;
    //                return m_rubbishPrefab;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if ((m_rubbishRecyclingToSpawn == 0) && (m_rubbishRubbishToSpawn == 0))
    //        {
    //            return null;
    //        }
    //        else if (m_rubbishRecyclingToSpawn == 0)
    //        {
    //            m_rubbishRubbishToSpawn--;
    //            return m_rubbishPrefab;
    //        }
    //        else if (m_rubbishRubbishToSpawn == 0)
    //        {
    //            m_rubbishRecyclingToSpawn--;
    //            return m_recyclePrefab;
    //        }
    //        else
    //        {
    //            int state = Random.Range(0, 2);
    //            if (state == 0)
    //            {
    //                m_rubbishRecyclingToSpawn--;
    //                return m_recyclePrefab;
    //            }
    //            else
    //            {
    //                m_rubbishRubbishToSpawn--;
    //                return m_rubbishPrefab;
    //            }
    //        }
    //    }
    //}

    //private Vector3 GetSpawnPositionOffset()
    //{
    //    return new Vector3(Random.Range(m_spawnPositionOffset.x, m_spawnPositionOffset.y),
    //        0.0f, Random.Range(m_spawnPositionOffset.z, m_spawnPositionOffset.w));
    //}

    //private void CheckComplete()
    //{
    //    if(m_recycleRecyclingToSpawn == 0 && m_recycleRubbishToSpawn == 0
    //        && m_rubbishRecyclingToSpawn == 0 && m_rubbishRubbishToSpawn == 0)
    //    {
    //        m_transitionCountdown = m_transitionCountdownLength;
    //        m_gameoverComplete = true;
    //        m_transitionCountdown = m_transitionCountdownLength;
    //    }
    //}
}


