public class ASM_State_DashToPlayer : IState
{
    private AlienSMDashToController dashController;

    public ASM_State_DashToPlayer(AlienSwordmasterReferences references)
    {
        dashController = references.DashToController;
    }

    public void OnEnter()
    {
        dashController.Dash();
    }


    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}