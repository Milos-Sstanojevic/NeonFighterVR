using UnityEngine;

public class ASM_State_DashAwayFromPlayer : IState
{
    private AlienSwordmasterReferences references;
    private ParticleSystem dashParticle;
    private Renderer enemyRenderer;
    private AlienSMDashAwayController dashController;

    public ASM_State_DashAwayFromPlayer(AlienSwordmasterReferences references)
    {
        this.references = references;
        dashController = references.DashFromController;
    }

    public void OnEnter()
    {
        if (references.NumberOfAttacksDone > references.NumberOfAttacksBeforeDashingAway)
            references.NumberOfAttacksDone = 0;

        references.IsAttacking = false;
        dashController.Dash();
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}