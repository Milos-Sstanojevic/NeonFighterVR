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
        EventManager.Instance.SubscribeToOnAlienSMSpecialAttackDone(AttackDone);

        specialAttackDone = false;
        slashAttack.gameObject.SetActive(true);

        references.Animator.SetBool("SpecialSlash", true);
        references.IsAttacking = true;

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
