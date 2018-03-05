public class Recycle : Action
{
    public override void DoAction(ActionData _data)
    {
        if (_data.m_player.m_currentCargo.CompareTag("Recycling"))
        {
            WorldManager.Instance.RecyclingRecycled();
        }
        else
        {
            WorldManager.Instance.RubbishRecycled();
        }
        Destroy(_data.m_player.m_currentCargo);
        _data.m_player.m_currentCargo = null;
        _data.m_player.m_disableCargo = false;
        base.DoAction(_data);
    }
}
