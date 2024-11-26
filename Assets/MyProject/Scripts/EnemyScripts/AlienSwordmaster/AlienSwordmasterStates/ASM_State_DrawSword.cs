using UnityEngine;

public class ASM_State_DrawSword : IState
{
    private AlienSwordmasterReferences references;
    private bool swordDrawn;

    public ASM_State_DrawSword(AlienSwordmasterReferences references)
    {
        this.references = references;
        EventManager.Instance.SubscribeToOnAlienSMSwordDrawnAction(HandleSwordDrawn);
    }

    private void HandleSwordDrawn()
    {
        swordDrawn = true;
    }

    public void OnEnter()
    {
        swordDrawn = false;
    }

    public void OnExit()
    {
        EventManager.Instance.UnsubscribeFromOnAlienSMSwordDrawnAction(HandleSwordDrawn);
    }

    public void Tick()
    {
    }

    public bool IsDone() => swordDrawn;
}