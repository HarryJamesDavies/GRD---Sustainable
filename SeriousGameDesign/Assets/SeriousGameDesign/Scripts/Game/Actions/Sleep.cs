public class Sleep : Action
{
    public override void DoAction(ActionData _data)
    {
        GameManager.Instance.ChangeState(GameState.Result);
        base.DoAction(_data);
    }
}
