public class ASM_State_TakeDamage : IState
{
    private AlienSwordmasterReferences references;
    private EnemyDamageController enemyDamageController;

    public ASM_State_TakeDamage(AlienSwordmasterReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetTrigger("TakeDamage");
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}
