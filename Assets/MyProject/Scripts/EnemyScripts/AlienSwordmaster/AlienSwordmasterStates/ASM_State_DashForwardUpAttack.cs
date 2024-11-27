using UnityEngine;

public class ASM_State_DashForwardUpAttack : IState
{
    private AlienSwordmasterReferences references;
    private Transform playerTransform;
    private float dashSpeed = 3f;
    private bool dashDone;
    private float stopDistance = 1.5f;

    public ASM_State_DashForwardUpAttack(AlienSwordmasterReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        dashDone = false;
        playerTransform = references.Character.transform;
        references.NavMeshAgent.isStopped = true;
    }

    public void OnExit()
    {
        references.NavMeshAgent.isStopped = false;
    }

    public void Tick()
    {
        if (dashDone) return;

        Vector3 direction = (playerTransform.position - references.transform.position).normalized;
        Vector3 targetPosition = playerTransform.position - direction * stopDistance;

        references.transform.position = Vector3.MoveTowards(references.transform.position, targetPosition, dashSpeed * Time.deltaTime);

        if (Vector3.Distance(references.transform.position, targetPosition) <= 0.1f)
            dashDone = true;
    }

    public bool IsDone() => dashDone;
}