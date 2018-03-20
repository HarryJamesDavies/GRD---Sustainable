using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish : MonoBehaviour
{
	void Start ()
    {
        GameManager.Instance.m_remainingRubbish.Add(gameObject);
	}
}
