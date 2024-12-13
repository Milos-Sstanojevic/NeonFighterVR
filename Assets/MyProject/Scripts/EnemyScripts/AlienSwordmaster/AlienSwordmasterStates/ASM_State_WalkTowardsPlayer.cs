using UnityEngine;

public class ASM_State_WalkTowardsPlayer : IState
{
    private AlienSwordmasterReferences references;

    public ASM_State_WalkTowardsPlayer(AlienSwordmasterReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetTrigger("Walk");
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        Vector3 direction = (references.Character.transform.position - references.transform.position).normalized;

        references.transform.position += direction * references.WalkSpeed * Time.deltaTime;
    }
}
