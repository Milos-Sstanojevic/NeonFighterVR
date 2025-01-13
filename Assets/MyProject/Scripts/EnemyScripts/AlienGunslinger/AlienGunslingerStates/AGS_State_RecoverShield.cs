using UnityEngine;

public class AGS_State_RecoverShield : IState
{
    private AlienGunslingerReferences references;
    private bool isDone;

    public AGS_State_RecoverShield(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        Debug.Log("Recover Shield");
        references.DashingController.DashAwayFromPlayer();
        references.ShieldController.RecoverShield();
        isDone = true;
    }

    public void OnExit()
    {
        isDone = false;
    }

    public void Tick()
    {
    }

    public bool IsDone() => isDone;
}