using System.Collections;
using UnityEngine;

public class AlienSMBrokenStanceController : MonoBehaviour
{
    [SerializeField] private float maxStanceTime;
    [SerializeField] private float damageWindow;
    [SerializeField] private float poiseCooldown;
    private EnemyDamageController enemyDamageController;
    private bool recoverFromBrokenStance;
    private bool poiseOnCooldown;

    private void Awake()
    {
        enemyDamageController = GetComponent<EnemyDamageController>();
    }

    public IEnumerator BreakStance()
    {
        if (poiseOnCooldown) yield break;

        poiseOnCooldown = true;
        recoverFromBrokenStance = false;
        float elapsedTime = 0f;
        float lastDamaeTime = Time.time;

        while (elapsedTime < maxStanceTime)
        {
            if (enemyDamageController.ReceivedDamage())
                lastDamaeTime = Time.time;

            if (Time.time - lastDamaeTime > damageWindow)
            {
                recoverFromBrokenStance = true;
                yield return StartCoroutine(StartPoiseCooldown());
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!recoverFromBrokenStance)
        {
            recoverFromBrokenStance = true;
            yield return StartCoroutine(StartPoiseCooldown());
        }
    }

    private IEnumerator StartPoiseCooldown()
    {
        yield return new WaitForSeconds(poiseCooldown);
        poiseOnCooldown = false;
    }

    public bool RecoverFromBrokenStance() => recoverFromBrokenStance;
}