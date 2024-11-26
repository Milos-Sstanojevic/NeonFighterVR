using UnityEngine;

public class ASM_State_InwardSlash : IState
{
    private AlienSwordmasterReferences references;
    private bool inwardSlashAnimationDone;
    private bool attackHit;

    public ASM_State_InwardSlash(AlienSwordmasterReferences references)
    {
        this.references = references;
    }

    private void HandleInwardSlashDone()
    {
        inwardSlashAnimationDone = true;
        references.LastAttackTime = Time.time;
    }

    private void HandleAttackHit()
    {
        attackHit = true;
    }

    public void OnEnter()
    {
        attackHit = false;
        inwardSlashAnimationDone = false;
        references.Animator.SetBool("InwardAttack", true);
        EventManager.Instance.SubscribeToOnAlienSMInwardSlashDone(HandleInwardSlashDone);
        EventManager.Instance.SubscribeToOnAlienSMAttackHitAction(HandleAttackHit);
        // references.Animator.SetBool("AttackSuccess", true);
    }

    public void OnExit()
    {
        references.Animator.SetBool("AttackSuccess", false);
        references.Animator.SetBool("InwardAttack", false);
        EventManager.Instance.UnsubscribeFromOnAlienSMInwardSlashDone(HandleInwardSlashDone);
        EventManager.Instance.UnsubscribeFromOnAlienSMAttackHitAction(HandleAttackHit);
    }

    public void Tick()
    {
    }

    public bool CanAttack() => Time.time - references.LastAttackTime >= references.AttackCooldown;
    public bool AttackHit() => attackHit;
    public bool IsDone()
    {
        return inwardSlashAnimationDone;
    }
}
