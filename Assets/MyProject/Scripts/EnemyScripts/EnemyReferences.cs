using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyReferences : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent { get; private set; }
    public Animator Animator { get; private set; }
    readonly public float PathUpdateDelay = 0.2f;
    public event Action OnPunchAttackFinished;
    public event Action OnRoarAnimationFinished;
    public event Action OnSwipeAttackFinished;

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
    }

    private void PunchAttackFinished()
    {
        OnPunchAttackFinished?.Invoke();
    }

    private void RoarAnimationFinished()
    {
        OnRoarAnimationFinished?.Invoke();
    }

    private void SwipeAttackFinished()
    {
        OnSwipeAttackFinished?.Invoke();
    }
}
