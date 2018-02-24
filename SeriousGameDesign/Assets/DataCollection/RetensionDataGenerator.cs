using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetensionDataGenerator : MonoBehaviour
{
    public int m_retensionDataId = -1;
    public int m_currentChunkIdD = -1;
    public float m_retensionLength = 0.0f;

    private float m_timerLength = 10.0f;

	void Start ()
    {
        m_retensionDataId = RetensionTracker.Instance.CreateData("Test" + Random.Range(0, 100));
        m_retensionLength =  RetensionTracker.Instance.m_data[m_retensionDataId].GetTotalLength();

        m_currentChunkIdD = -1;
        m_timerLength = Random.Range(3.0f, 25.0f);
        m_currentChunkIdD = RetensionTracker.Instance.m_data[m_retensionDataId].StartTimeChunk();
	}
	
	void Update ()
    {
		if(m_timerLength > 0.0f)
        {
            m_timerLength -= Time.deltaTime;
        }
        else
        {
            RetensionTracker.Instance.m_data[m_retensionDataId].StopTimeChunk(m_currentChunkIdD);
            m_retensionLength = RetensionTracker.Instance.m_data[m_retensionDataId].GetTotalLength();

            m_currentChunkIdD = -1;
            m_timerLength = Random.Range(3.0f, 20.0f);
            m_currentChunkIdD = RetensionTracker.Instance.m_data[m_retensionDataId].StartTimeChunk();
        }
	}
}
