using UnityEngine;

public class AGS_State_PunishPlayer : IState
{
    private AlienGunslingerReferences references;

    public AGS_State_PunishPlayer(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetTrigger("PunishPlayer");
        Debug.Log("Punish Player");
        references.ShootingController.StunShoot();
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}