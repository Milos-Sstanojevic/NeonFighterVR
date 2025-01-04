using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = enemyData.Health;
    }

    public void DecreaseHealth(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= enemyData.Health * enemyData.SecondPhaseHPRatio)
            EventManager.Instance.OnStartSecondPhaseAction();

        if (currentHealth <= 0)
            Debug.Log("Die");
    }

}