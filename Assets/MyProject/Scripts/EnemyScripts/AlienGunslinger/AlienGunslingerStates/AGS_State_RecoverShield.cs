public class AGS_State_RecoverShield : IState
{
    private AlienGunslingerReferences references;

    public AGS_State_RecoverShield(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.DashingController.DashAwayFromPlayer();
        references.ShieldController.RecoverShield();
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}