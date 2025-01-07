using UnityEngine;

public class AGS_State_IdleProvoking : IState
{
    private AlienGunslingerReferences references;
    private bool isDone;
    private AlienGunslingerController alienGunslingerController;

    public AGS_State_IdleProvoking(AlienGunslingerReferences references, AlienGunslingerController alienGunslingerController)
    {
        this.references = references;
        this.alienGunslingerController = alienGunslingerController;
    }

    public void OnEnter()
    {
        isDone = false;
        references.Animator.SetBool("IdleProvoking", true);
        alienGunslingerController.UnsetPlayerTriedToAttack();
        Debug.Log("IdleProvoking");
        references.CachedIdleOrIdleProvoking = null;
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