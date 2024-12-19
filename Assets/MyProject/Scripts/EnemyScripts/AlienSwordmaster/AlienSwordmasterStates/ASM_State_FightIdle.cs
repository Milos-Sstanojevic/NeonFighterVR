using UnityEngine;

public class ASM_State_FightIdle : IState
{
    private AlienSwordmasterReferences references;

    public ASM_State_FightIdle(AlienSwordmasterReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.IsAttacking = false;
        references.Animator.SetBool("FightIdle", true);
        DecideNextAttack();
    }

    private AttackType DecideNextAttack()
    {
        references.NextAttack = Random.value < 0.5f ? AttackType.OutwardSlash : AttackType.InwardSlash;

        return references.NextAttack;
    }

    public void OnExit()
    {
        references.Animator.SetBool("FightIdle", false);
    }

    public void Tick()
    {
    }
}

public enum AttackType
{
    OutwardSlash,
    InwardSlash
}

