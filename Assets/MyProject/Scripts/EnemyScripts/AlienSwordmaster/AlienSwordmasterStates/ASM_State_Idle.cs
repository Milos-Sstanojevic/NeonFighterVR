public class ASM_State_Idle : IState
{
    private AlienSwordmasterReferences references;


    public ASM_State_Idle(AlienSwordmasterReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetBool("Idle", true);
        
    }

    public void OnExit()
    {
        references.Animator.SetBool("Idle", false);
    }

    public void Tick()
    {
    }
}