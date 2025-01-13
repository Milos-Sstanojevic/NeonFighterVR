using UnityEngine;

public class SwordAttackingController : MonoBehaviour
{
    private SwordData swordData;

    private void Awake()
    {
        swordData = GetComponent<SwordComboController>().GetSwordData();
    }

    private void OnCollisionEnter(Collision other)
    {
        ShieldController shieldController = other.gameObject.GetComponent<ShieldController>();
        EnemyDamageController enemy = other.gameObject.GetComponent<EnemyDamageController>();
        if (shieldController)
            shieldController.ActivateRipples();

        if (enemy)
            enemy.TakeDamage(swordData.damage);
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}