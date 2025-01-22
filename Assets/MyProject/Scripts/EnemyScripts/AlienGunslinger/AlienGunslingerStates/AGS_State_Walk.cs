using UnityEngine;

public class AGS_State_Walk : IState
{
    private AlienGunslingerReferences references;
    // private Vector3 direction;

    public AGS_State_Walk(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetBool("Walk", true);
        Debug.Log("Walk");
        references.WalkController.StartWalking();
    }

    public void OnExit()
    {
        references.Animator.SetBool("Walk", false);
        references.WalkController.StopWalking();
    }

    public void Tick()
    {
    }
}