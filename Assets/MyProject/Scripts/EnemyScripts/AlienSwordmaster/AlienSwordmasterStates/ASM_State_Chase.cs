
public class ASM_State_Chase : IState
{
    private AlienSwordmasterReferences references;

    public ASM_State_Chase(AlienSwordmasterReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetTrigger("Chase");
        references.Animator.SetFloat("Speed", 1);
        references.NavMeshAgent.SetDestination(references.Character.transform.position);
    }

    public void OnExit()
    {
        references.Animator.SetFloat("Speed", 0);
    }

    public void Tick()
    {
    }
}
