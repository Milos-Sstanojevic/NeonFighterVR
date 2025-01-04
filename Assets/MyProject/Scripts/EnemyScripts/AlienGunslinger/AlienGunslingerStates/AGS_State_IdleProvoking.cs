using UnityEngine;

public class AGS_State_IdleProvoking : IState
{
    private AlienGunslingerReferences references;
    private bool isDone;


    public AGS_State_IdleProvoking(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        isDone = false;
        references.Animator.SetBool("IdleProvoking", true);
        Debug.Log("IdleProvoking");
    }

    private void AnimationFinished()
    {
        isDone = true;
    }

    public void OnExit()
    {
        references.Animator.SetBool("IdleProvoking", false);
    }

    public void Tick()
    {
    }

    public bool IsDone() => isDone;
}