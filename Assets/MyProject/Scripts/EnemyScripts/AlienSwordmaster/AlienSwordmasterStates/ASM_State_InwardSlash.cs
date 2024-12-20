using UnityEngine;

public class ASM_State_InwardSlash : IState
{
    private AlienSwordmasterReferences references;
    private bool inwardSlashAnimationDone;

    public ASM_State_InwardSlash(AlienSwordmasterReferences references)
    {
        this.references = references;
    }

    private void HandleInwardSlashDone()
    {
        inwardSlashAnimationDone = true;
        references.LastAttackTime = Time.time;
    }

    public void OnEnter()
    {
        if (!references.AttackHit)
            references.NumberOfAttacksDone++;

        references.AttackHit = false;

        references.IsAttacking = true;
        inwardSlashAnimationDone = false;
        references.Animator.SetBool("InwardAttack", true);
        EventManager.Instance.SubscribeToOnAlienSMInwardSlashDone(HandleInwardSlashDone);
    }

    public void OnExit()
    {
        references.Animator.SetBool("AttackSuccess", false);
        references.Animator.SetBool("InwardAttack", false);
        EventManager.Instance.UnsubscribeFromOnAlienSMInwardSlashDone(HandleInwardSlashDone);
    }

    public void Tick()
    {
    }

    public bool ShouldDoInwardSlash() => references.NextAttack == AttackType.InwardSlash;
    public bool CanAttack() => Time.time - references.LastAttackTime >= references.AttackCooldown;
    public bool IsDone()
    {
        return inwardSlashAnimationDone;
    }
}
