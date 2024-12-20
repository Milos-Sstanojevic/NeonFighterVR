using System.Collections;
using UnityEngine;

public class EnemyDamageController : MonoBehaviour
{
    private int health = 100;
    private bool receivedDamage;
    private bool receivedBigDamage;
    private float damageResetTimer = 0.1f;

    private void Start()
    {
        health = 100;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<SwordComboController>())
            TakeDamage(other.gameObject.GetComponent<SwordComboController>().GetSwordData().damage);
    }


    public void TakeDamage(int damage)
    {
        if (damage >= 50)
            receivedBigDamage = true;
        else
            receivedDamage = true;

        health -= damage;

        // if (health <= 0)
        //     Die();

        StartCoroutine(ResetDamageState());
    }

    private IEnumerator ResetDamageState()
    {
        yield return new WaitForSeconds(damageResetTimer);
        receivedDamage = false;
        receivedBigDamage = false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public bool ReceivedDamage()
    {
        bool result = receivedDamage; receivedBigDamage = false;
        return result;
    }

    public bool ReceivedBigDamage() => receivedBigDamage;
}
