public class AGS_State_PunishPlayer : IState
{
    private AlienGunslingerReferences references;

    public AGS_State_PunishPlayer(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetBool("PunishPlayer", true);
        references.ShootingController.StunShoot();
    }

    public void OnExit()
    {
        references.Animator.SetBool("PunishPlayer", false);
    }

    public void Tick()
    {
    }
}