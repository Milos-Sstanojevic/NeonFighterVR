using UnityEngine;

public class AGS_State_SideWalk : IState
{
    private AlienGunslingerReferences references;
    private Vector3 direction;

    public AGS_State_SideWalk(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        references.Animator.SetBool("SideWalk", true);
        Debug.Log("Side Walk");
        direction = DirectionToMove();
    }

    public void OnExit()
    {
        references.Animator.SetBool("SideWalk", false);
    }

    public void Tick()
    {
        float speed = references.SideWalkSpeed;
        references.transform.localPosition += direction * speed * Time.deltaTime;
    }

    public Vector3 DirectionToMove()
    {
        float chance = Random.Range(0f, 1f);

        if (chance < 0.5f)
            return Vector3.right;
        else
            return Vector3.left;
    }
}