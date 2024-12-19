public class ASM_State_BreakStance : IState
{
    private AlienSwordmasterReferences references;
    private bool animationDone;

    public ASM_State_BreakStance(AlienSwordmasterReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        EventManager.Instance.SubscribeToOnBreakStanceAnimationDone(AnimationDone);
        references.Animator.SetTrigger("BreakStance");
        references.BrokenStanceController.StartCoroutine(references.BrokenStanceController.BreakStance());
    }

    private void AnimationDone()
    {
        animationDone = true;
    }

    public void OnExit()
    {
        animationDone = false;
        EventManager.Instance.UnsubscribeFromOnBreakStanceAnimationDone(AnimationDone);
    }

    public void Tick()
    {
    }

    public bool IsDone() => animationDone;
}
