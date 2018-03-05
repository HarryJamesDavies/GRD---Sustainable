public class Bin : Action
{
    public override void DoAction(ActionData _data)
    {
        if (_data.m_player.m_currentCargo.CompareTag("Rubbish"))
        {
            WorldManager.Instance.RubbishDestroyed();
        }
        else
        {
            WorldManager.Instance.RecyclingDestroyed();
        }
        Destroy(_data.m_player.m_currentCargo);
        _data.m_player.m_currentCargo = null;
        _data.m_player.m_disableCargo = false;
        base.DoAction(_data);
    }
}
