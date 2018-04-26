using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish : MonoBehaviour
{
    public bool m_ignoreManager = false;

    public float m_weight = 0.0f;

	void Start ()
    {
        if(!m_ignoreManager)
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.m_remainingRubbish.Add(gameObject);
            }
        }
	}
}
