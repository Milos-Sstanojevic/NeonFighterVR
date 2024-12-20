using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    private int maxHealth = 100;
    private int currentHealth = 100;

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnPlayerHitAction(TakeDamage);
    }

    private void TakeDamage(int damage)
    {
        Debug.Log(damage);
        currentHealth -= damage;

        if (currentHealth <= 0)
            Debug.Log("Died");
    }
}