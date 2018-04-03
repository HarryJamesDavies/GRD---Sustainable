using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeightObjectResult
{
    public Sprite m_icon;
    public int m_count;

    public WeightObjectResult(Sprite _icon)
    {
        m_icon = _icon;
        m_count = 1;
    }
}

public class ResultData : MonoBehaviour
{
    public float m_weightPerPerson;
    public float m_weightPerStreet;
    public float m_landfileldRubbish;
    public float m_landfilledRecycling;
    public float m_sortedRubbish;
    public List<WeightObjectResult> m_weightObjectResults = new List<WeightObjectResult>();

    public void Initalise(float _weightPerPerson, float _weightPerStreet, float _landfileldRubbish,
        float _landfilledRecycling, float _sortedRubbish, List<WeightObjectResult> __weightObjectResults)
    {
        m_weightPerPerson = _weightPerPerson;
        m_weightPerStreet = _weightPerStreet;
        m_landfileldRubbish = _landfileldRubbish;
        m_landfilledRecycling = _landfilledRecycling;
        m_sortedRubbish = _sortedRubbish;
        m_weightObjectResults.AddRange(__weightObjectResults);

        DontDestroyOnLoad(gameObject);
    }
}
