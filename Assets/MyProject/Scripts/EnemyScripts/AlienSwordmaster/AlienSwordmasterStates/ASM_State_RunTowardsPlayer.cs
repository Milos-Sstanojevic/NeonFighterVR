using UnityEngine;

public class ASM_State_RunTowardsPlayer : IState
{
    private AlienSwordmasterReferences references;

    public ASM_State_RunTowardsPlayer(AlienSwordmasterReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetTrigger("Run");
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        Vector3 direction = (references.Character.transform.position - references.transform.position).normalized;

        references.transform.position += direction * references.RunSpeed * Time.deltaTime;
    }
}
