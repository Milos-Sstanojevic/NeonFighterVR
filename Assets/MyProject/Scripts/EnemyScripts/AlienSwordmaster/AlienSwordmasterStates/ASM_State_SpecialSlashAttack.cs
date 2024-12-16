public class ASM_State_SpecialSlashAttack : IState
{
    private AlienSwordmasterReferences references;
    private bool specialAttackDone;
    private SlashAttack slashAttack;

    public ASM_State_SpecialSlashAttack(AlienSwordmasterReferences references)
    {
        this.references = references;
        slashAttack = references.SlashAttack;
    }

    public void OnEnter()
    {
        specialAttackDone = false;
        slashAttack.SetParentActive();
        references.Animator.SetBool("SpecialSlash", true);
        references.IsAttacking = true;
        EventManager.Instance.SubscribeToOnAlienSMSpecialAttackDone(AttackDone);
        slashAttack.PlayParticles();
    }

    private void AttackDone()
    {
        specialAttackDone = true;
    }

    public void OnExit()
    {
        slashAttack.StartCoroutine(slashAttack.ResetParticles());
        references.IsAttacking = false;
        references.Animator.SetBool("SpecialSlash", false);
        EventManager.Instance.UnsubscribeFromOnAlienSMSpecialAttackDone(AttackDone);
    }

    public void Tick()
    {
    }

    public bool IsDone() => specialAttackDone;
}
