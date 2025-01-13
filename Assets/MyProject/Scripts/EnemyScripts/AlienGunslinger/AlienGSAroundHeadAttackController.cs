using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGSAroundHeadAttackController : MonoBehaviour
{
    [SerializeField] private float timeBetweenShots = 2f;
    [SerializeField] private List<GameObject> shootingHoles;
    private int shootHoleToEnable;
    private int shootHoleToDisable;
    private Coroutine shootCoroutine;

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnSpawnTwoHolesAroundHead(EnableTwoShootingHoles);
        EventManager.Instance.SubscribeToOnShieldBrokenAction(ShieldBroken);
    }

    private void ShieldBroken()
    {
        foreach (GameObject hole in shootingHoles)
            hole.SetActive(false);

        if (shootCoroutine == null)
            return;

        StopCoroutine(shootCoroutine);
        shootCoroutine = null;
    }

    public void EnableTwoShootingHoles()
    {
        for (int i = 0; i < 2; i++)
        {
            shootingHoles[shootHoleToEnable].SetActive(true);
            shootHoleToEnable++;
        }

        if (shootHoleToEnable >= shootingHoles.Count)
        {
            shootHoleToEnable = 0;
        }
    }

    public void ShootFromTwoHoles()
    {
        shootCoroutine = StartCoroutine(ShootFromTwoHolesCoroutine());
    }

    public IEnumerator ShootFromTwoHolesCoroutine()
    {
        while (shootHoleToDisable < shootingHoles.Count)
        {
            for (int i = 0; i < 2; i++)
            {
                shootingHoles[shootHoleToDisable].SetActive(false);
                shootHoleToDisable++;
            }

            if (shootHoleToDisable >= shootingHoles.Count)
            {
                shootHoleToDisable = 0;
                shootCoroutine = null;
                yield break;
            }

            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    public bool HasActiveHolesAroundHead()
    {
        foreach (GameObject hole in shootingHoles)
            if (hole.activeSelf)
                return true;

        return false;
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnSpawnTwoHolesAroundHead(EnableTwoShootingHoles);
        EventManager.Instance.UnsubscribeFromOnShieldBrokenAction(ShieldBroken);
    }
}