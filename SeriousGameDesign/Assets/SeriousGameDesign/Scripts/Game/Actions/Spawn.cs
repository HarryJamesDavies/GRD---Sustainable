using UnityEngine;

public class Spawn : Action
{
    public override void DoAction(ActionData _data)
    {
        WeightObject weight = _data.m_parent.GetComponent<WeightObject>();
        Instantiate(weight.m_cargoObject, 
            WeightObjectManager.Instance.m_spawnPoints[Random.Range(0, 
            WeightObjectManager.Instance.m_spawnPoints.Count)].position,
            Quaternion.identity);
        OutcomeManager.Instance.AddWeightResult(weight.m_icon, weight.m_weight);
        base.DoAction(_data);
    }
}