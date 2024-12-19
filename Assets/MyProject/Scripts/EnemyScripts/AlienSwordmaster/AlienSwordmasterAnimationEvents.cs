using UnityEngine;

public class AlienSwordmasterAnimationEvents : MonoBehaviour
{
    private void SwordDrawnAnimationDone()
    {
        EventManager.Instance.OnAlienSMSwordDrawnAction();
    }

    private void OutwardSlashAnimationDone()
    {
        EventManager.Instance.OnAlienSMOutwardSlashAction();
    }

    private void InwardSlashAnimationDone()
    {
        EventManager.Instance.OnAlienSMInwardSlashAction();
    }

    private void JumpAwayFromPlayerAnimationDone()
    {
        EventManager.Instance.OnAlienSMJumpAwayAnimationDone();
    }

    private void PlayRegularSlashProjectileEffect()
    {
        EventManager.Instance.OnAlienSMSlashParticlePlay();
    }

    private void SpecialAttackAnimationDone()
    {
        EventManager.Instance.OnAlienSMSpecialAttackDone();
    }

    private void BreakStanceAnimationDone()
    {
        EventManager.Instance.OnBreakStanceAnimationDone();
    }
}