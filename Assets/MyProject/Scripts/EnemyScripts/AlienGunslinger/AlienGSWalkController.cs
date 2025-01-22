using System.Collections;
using UnityEngine;

public class AlienGSWalkController : MonoBehaviour
{
    [SerializeField] private float walkingTime = 3f;
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private Transform objectForDirectionOfMovement;
    private bool doneWalking;
    private Coroutine walkingCoroutine;
    private bool shouldWalk;

    public void StartWalking()
    {
        shouldWalk = true;
        doneWalking = false;
        walkingCoroutine = StartCoroutine(WalkingCoroutine());
    }

    private IEnumerator WalkingCoroutine()
    {
        yield return new WaitForSeconds(walkingTime);
        doneWalking = true;
        shouldWalk = false;
    }

    public void StopWalking()
    {
        doneWalking = true;
        shouldWalk = false;
        StopCoroutine(walkingCoroutine);
    }

    private void Update()
    {
        if (!shouldWalk) return;

        Vector3 movementDirection = objectForDirectionOfMovement.forward;
        movementDirection.y = 0;
        movementDirection.Normalize();

        transform.position += movementDirection * walkSpeed * Time.deltaTime;
    }

    public bool IsDoneWalking() => doneWalking;
}