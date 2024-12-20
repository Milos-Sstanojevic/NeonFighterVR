using UnityEngine;

public class ASM_State_JumpAwayFromPlayer : IState
{
    private AlienSwordmasterReferences references;
    private bool jumpedAway;

    public ASM_State_JumpAwayFromPlayer(AlienSwordmasterReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.IsAttacking = false;
        jumpedAway = false;
        EventManager.Instance.SubscribeToOnAlienSMJumpAwayAnimationDone(JumpAnimationDone);
        references.Animator.SetTrigger("JumpAway");
        references.Rigidbody.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
    }

    private void JumpAnimationDone()
    {
        jumpedAway = true;
    }

    public void OnExit()
    {
        EventManager.Instance.UnsubscribeFromOnAlienSMJumpAwayAnimationDone(JumpAnimationDone);
        references.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Tick()
    {
    }


    public bool IsDone() => jumpedAway;
}
