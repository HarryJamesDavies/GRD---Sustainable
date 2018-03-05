using UnityEngine;

public class Carry : Action
{
    public override void DoAction(ActionData _data)
    {
        _data.m_player.CarryObject(_data.m_parent.gameObject);
        base.DoAction(_data);
    }
}
