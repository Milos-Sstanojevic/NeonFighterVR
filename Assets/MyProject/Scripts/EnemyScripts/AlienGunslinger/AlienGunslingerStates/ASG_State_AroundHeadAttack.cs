using UnityEngine;

public class ASG_State_AroundHeadAttack : IState
{
    private AlienGunslingerReferences references;
    private bool isDone;

    public ASG_State_AroundHeadAttack(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        EventManager.Instance.SubscribeToOnSpawningHolesAroundHeadFinished(FinishedAnimation);
        references.Animator.SetBool("HandSeparate", true);
        Debug.Log("Around Head Attack");
        isDone = false;
        references.CachedAttackType = null;
    }

    private void FinishedAnimation()
    {
        isDone = true;
    }

    public void OnExit()
    {
        EventManager.Instance.UnsubscribeFromOnSpawningHolesAroundHeadFinished(FinishedAnimation);
        references.Animator.SetBool("HandSeparate", false);
        references.AroundHeadAttackController.ShootFromTwoHoles();
    }

    public void Tick()
    {

    }

    public bool IsDone() => isDone;
}