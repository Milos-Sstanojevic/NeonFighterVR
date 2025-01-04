public class AGS_State_DashFromPlayer : IState
{
    private AlienGunslingerReferences references;

    public AGS_State_DashFromPlayer(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.DashingController.Dash();
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}