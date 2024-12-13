
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
        if (references.NumberOfAttacksDone > references.NumberOfAttacksBeforeDashingAway)
            references.NumberOfAttacksDone = 0;

        references.NumberOfAttacksDone++;
        references.IsAttacing = false;
        references.Animator.SetBool("FightIdle", true);
        references.DecideNextAttack();
    }

    public void OnExit()
    {
        references.Animator.SetBool("FightIdle", false);
    }

    public void Tick()
    {
    }

    public bool ShouldDashToPlayer() => Vector3.Distance(references.transform.position, references.Character.transform.position) > references.DashDistance;
}

public enum AttackType
{
    OutwardSlash,
    InwardSlash
}
