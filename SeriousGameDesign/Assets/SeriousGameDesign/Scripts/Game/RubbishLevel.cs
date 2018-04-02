using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishLevel : MonoBehaviour
{
    public Vector2 m_heightLimits = new Vector2();

    public float m_rubbishVolume = 0.0f;
    public float m_maxRubbishVolume = 100.0f;

    public Vector3 m_removePosition = new Vector3(1000.0f, 1000.0f, 1000.0f);

    public BinCore m_bin = null;

    void Update ()
    {
        float volumeRatio = m_rubbishVolume / m_maxRubbishVolume;
        if(volumeRatio < 0.0f)
        {
            volumeRatio = 0.0f;
        }
        else if(volumeRatio > 1.0f)
        {
            volumeRatio = 1.0f;
        }

        float heightDifference = m_heightLimits.y - m_heightLimits.x;
        transform.position = new Vector3(transform.position.x, m_heightLimits.x + (heightDifference * volumeRatio), transform.position.z);
	}

    private void OnTriggerEnter(Collider _other)
    {
        if(_other.GetComponent<Rubbish>())
        {
            if(m_bin != null)
            {
                if(!m_bin.m_rubbish.Contains(_other.gameObject))
                {
                    return;
                }
            }
            else
            {
                GameManager.Instance.m_remainingRubbish.Remove(_other.gameObject);
            }

            if(m_rubbishVolume < m_maxRubbishVolume)
            {
                m_rubbishVolume++;
            }

            _other.transform.position = m_removePosition;
            _other.GetComponent<Rigidbody>().useGravity = false;
        }
    }
}
