public class AGS_State_DrawGun : IState
{
    private AlienGunslingerReferences references;
    private bool isDone;

    public AGS_State_DrawGun(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        EventManager.Instance.SubscribeToOnDrawGunsAnimationFinished(AnimationFinished);
        isDone = false;
        references.Animator.SetBool("DrawGun", true);
        references.Shield.SetActive(true);
    }

    private void AnimationFinished()
    {
        isDone = true;
    }

    public void OnExit()
    {
        EventManager.Instance.UnsubscribeFromOnDrawGunsAnimationFinished(AnimationFinished);
        references.Animator.SetBool("DrawGun", false);
    }

    public void Tick()
    {
    }

    public bool IsDone() => isDone;
}
