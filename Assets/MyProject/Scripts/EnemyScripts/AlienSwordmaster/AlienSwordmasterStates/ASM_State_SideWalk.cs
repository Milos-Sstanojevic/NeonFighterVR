using UnityEngine;

public class ASM_State_SideWalk : IState
{
    private AlienSwordmasterReferences references;
    private float startWalkingTime;

    public ASM_State_SideWalk(AlienSwordmasterReferences reference)
    {
        references = reference;
    }

    public void OnEnter()
    {
        references.Animator.SetTrigger("SideWalk");
        references.Rigidbody.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
        startWalkingTime = Time.time;
    }

    public void OnExit()
    {
        references.Rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    public void Tick()
    {
    }

    public bool StopWalking() => Time.time - startWalkingTime >= references.SideWalkingTime;
}