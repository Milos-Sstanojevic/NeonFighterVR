using UnityEngine;

public class AGS_State_BrokenShieldIdle : IState
{
    private AlienGunslingerReferences references;
    private bool isDone;

    public AGS_State_BrokenShieldIdle(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        isDone = false;
        references.Animator.SetTrigger("BrokenShieldIdle");
        Debug.Log("BrokenShieldIdle");
        references.DashingController.ResetDashingCoroutine();
    }


    public void OnExit()
    {
    }

    public void Tick()
    {
    }

    public bool IsDone() => isDone;
}