public class Bin : Action
{
    public override void DoAction(ActionData _data)
    {
        _data.m_parent.GetComponent<BinCore>().AddRubbish(_data.m_player.m_currentCargo);
        _data.m_player.DropCargo();
        base.DoAction(_data);
    }
}
