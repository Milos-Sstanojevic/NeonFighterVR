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
        references.Animator.SetBool("BrokenShieldIdle", true);
        Debug.Log("BrokenShieldIdle");
    }

    private void AnimationFinished()
    {
        isDone = true;
    }

    public void OnExit()
    {
        references.Animator.SetBool("BrokenShieldIdle", false);
    }

    public void Tick()
    {
    }

    public bool IsDone() => isDone;
}