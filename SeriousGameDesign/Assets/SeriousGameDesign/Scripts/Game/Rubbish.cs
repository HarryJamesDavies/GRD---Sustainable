using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish : MonoBehaviour
{
    public bool m_ignoreManager = false;

	void Start ()
    {
        if(m_ignoreManager)
        {
            GameManager.Instance.m_remainingRubbish.Add(gameObject);
        }
	}
}
