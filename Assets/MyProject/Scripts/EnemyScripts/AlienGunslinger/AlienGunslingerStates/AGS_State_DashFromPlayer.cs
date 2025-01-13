using UnityEngine;

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
        Debug.Log("Dash From Player");
    }

    public void OnExit()
    {
        references.DashingController.ResetDashDone();
    }

    public void Tick()
    {
    }
}