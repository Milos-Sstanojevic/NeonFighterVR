using UnityEngine;

public class AGS_State_Idle : IState
{
    private AlienGunslingerReferences references;

    public AGS_State_Idle(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetBool("Idle", true);
        Debug.Log("Idle");
    }

    public void OnExit()
    {
        references.Animator.SetBool("Idle", false);
    }

    public void Tick()
    {
    }
}