public class AGS_State_SideToSideShoot : IState
{
    private AlienGunslingerReferences references;

    private bool isDone;//ovo ce biti u posebnoj skripti za side to side

    public AGS_State_SideToSideShoot(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetBool("CrouchShoot", true);
    }

    public void OnExit()
    {
        references.Animator.SetBool("CrouchShoot", false);
    }

    public void Tick()
    {
    }

    public bool IsDone() => isDone;
}