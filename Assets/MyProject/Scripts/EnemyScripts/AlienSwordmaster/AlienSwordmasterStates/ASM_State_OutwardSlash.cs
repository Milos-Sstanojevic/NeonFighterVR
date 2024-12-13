using UnityEngine;

public class ASM_State_OutwardSlash : IState
{
    private AlienSwordmasterReferences references;
    private bool outwardSlashAnimationDone;
    private bool attackHit;

    public ASM_State_OutwardSlash(AlienSwordmasterReferences references)
    {
        this.references = references;
    }

    private void HandleOutwardSlashDone()
    {
        outwardSlashAnimationDone = true;
        references.LastAttackTime = Time.time;
    }

    public void OnEnter()
    {
        attackHit = false;
        references.IsAttacing = true;
        references.Animator.SetBool("OutwardAttack", true);
        EventManager.Instance.SubscribeToOnAlienSMOutwardSlashDone(HandleOutwardSlashDone);
        EventManager.Instance.SubscribeToOnAlienSMAttackHitAction(HandleAttackHit);
    }

    private void HandleAttackHit()
    {
        attackHit = true;
        references.Animator.SetBool("AttackSuccess", true);
    }

    public void OnExit()
    {
        references.Animator.SetBool("OutwardAttack", false);
        outwardSlashAnimationDone = false;
        EventManager.Instance.UnsubscribeFromOnAlienSMOutwardSlashDone(HandleOutwardSlashDone);
        EventManager.Instance.UnsubscribeFromOnAlienSMAttackHitAction(HandleAttackHit);
    }

    public void Tick()
    {
    }

    public bool ShouldDoOutwardSlash() => references.NextAttack == AttackType.OutwardSlash;
    public bool CanAttack() => Time.time - references.LastAttackTime >= references.AttackCooldown;
    public bool AttackHit() => attackHit;
    public bool IsDone()
    {
        return outwardSlashAnimationDone;
    }
}