using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int health = 100;

    private void Start()
    {
        health = 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SwordComboController>() && other is MeshCollider)
            TakeDamage(other.gameObject.GetComponent<SwordComboController>().GetSwordData().damage);
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health == 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
