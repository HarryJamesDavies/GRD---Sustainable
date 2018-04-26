using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightObjectManager : MonoBehaviour
{
    public static WeightObjectManager Instance = null;

    public List<WeightObject> m_currentWeightedObjects = new List<WeightObject>();
    public List<WeightObject> m_weightObjectsPrefabs = new List<WeightObject>();
    public List<int> m_objectCounts = new List<int>();

    public List<Pedstal> m_pedstals = new List<Pedstal>();
    public List<Transform> m_spawnPoints = new List<Transform>();

    private void Awake()
    {
        CheckInstance();
    }

    private void CheckInstance()
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

	void Start ()
    {
		for(int pedstalIter = 0; pedstalIter < m_pedstals.Count; pedstalIter++)
        {
            if(pedstalIter < m_weightObjectsPrefabs.Count)
            {
                m_objectCounts.Add(0);

                m_pedstals[pedstalIter].m_objectPanel.SetActive(true);

                m_pedstals[pedstalIter].m_objectIndex = pedstalIter;
                m_pedstals[pedstalIter].m_icon.sprite = m_weightObjectsPrefabs[pedstalIter].m_icon;
                m_pedstals[pedstalIter].m_count.text = 0.ToString();
                m_pedstals[pedstalIter].m_weight.text = m_weightObjectsPrefabs[pedstalIter].m_weight.ToString() + " kg";

                m_currentWeightedObjects.Add(Instantiate(m_weightObjectsPrefabs[pedstalIter].m_pedstalObject,
                    m_pedstals[pedstalIter].m_position.position, m_pedstals[pedstalIter].m_position.rotation,
                    m_pedstals[pedstalIter].m_position).GetComponent<WeightObject>());
            }
        }
	}
}
