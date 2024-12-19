using System.Collections;
using UnityEngine;

public class AlienSMBrokenStanceController : MonoBehaviour
{
    [SerializeField] private float maxStanceTime;
    [SerializeField] private float damageWindow;
    private EnemyDamageController enemyDamageController;
    private bool recoverFromBrokenStance;

    private void Awake()
    {
        enemyDamageController = GetComponent<EnemyDamageController>();
    }

    public IEnumerator BreakStance()
    {
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
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!recoverFromBrokenStance)
            recoverFromBrokenStance = true;
    }

    public bool RecoverFromBrokenStance() => recoverFromBrokenStance;
}