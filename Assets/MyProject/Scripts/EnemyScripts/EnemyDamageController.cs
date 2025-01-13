using System.Collections;
using UnityEngine;

public class EnemyDamageController : MonoBehaviour
{
    [SerializeField] private bool shieldActive;
    private int health = 100;
    private bool receivedDamage;
    private bool receivedBigDamage;
    private float damageResetTimer = 0.1f;
    private EnemyHealthController enemyHealthController;

    private void Awake()
    {
        enemyHealthController = GetComponent<EnemyHealthController>();
    }

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnShieldBrokenAction(ShieldBroken);
        EventManager.Instance.SubscribeToOnShieldRecoveredAction(ShieldRecovered);
    }

    private void ShieldBroken()
    {
        shieldActive = false;
    }

    private void ShieldRecovered()
    {
        shieldActive = true;
    }

    private void Start()
    {
        health = 100;
    }

    // private void OnCollisionEnter(Collision other)
    // {
    //     SwordComboController sword = other.gameObject.GetComponent<SwordComboController>();
    //     if (sword)
    //         TakeDamage(sword.GetSwordData().damage);
    // }

    public void TakeDamage(int damage)
    {
        if (shieldActive)
            return;

        if (damage >= 50)
            receivedBigDamage = true;
        else
            receivedDamage = true;

        enemyHealthController.DecreaseHealth(damage);

        StartCoroutine(ResetDamageState());
    }

    private IEnumerator ResetDamageState()
    {
        yield return new WaitForSeconds(damageResetTimer);
        receivedDamage = false;
        receivedBigDamage = false;
    }

    public bool ReceivedDamage()
    {
        bool result = receivedDamage;
        receivedDamage = false;
        return result;
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnShieldBrokenAction(ShieldBroken);
        EventManager.Instance.UnsubscribeFromOnShieldRecoveredAction(ShieldRecovered);
    }

    public bool ReceivedBigDamage() => receivedBigDamage;
}
