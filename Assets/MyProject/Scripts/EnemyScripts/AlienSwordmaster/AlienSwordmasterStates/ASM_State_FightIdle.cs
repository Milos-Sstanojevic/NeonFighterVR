
public class ASM_State_FightIdle : IState
{
    private AlienSwordmasterReferences references;

    public ASM_State_FightIdle(AlienSwordmasterReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.IsAttacing = false;
        references.Animator.SetBool("FightIdle", true);
        references.DecideNextAttack();
    }

    public void OnExit()
    {
        references.Animator.SetBool("FightIdle", false);
        references.IsAttacing = true;
    }

    public void Tick()
    {
    }

    public bool ShouldDoOutwardSlash() => references.NextAttack == AttackType.OutwardSlash;
    public bool ShouldDoInwardSlash() => references.NextAttack == AttackType.InwardSlash;
}

public enum AttackType
{
    OutwardSlash,
    InwardSlash
}
