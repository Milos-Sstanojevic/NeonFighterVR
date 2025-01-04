using System.Collections.Generic;
using UnityEngine;

public class AlienGSAroundHeadAttack : MonoBehaviour
{
    //treba da se stvore kugle oko njegove glave koje ce uvek da budu okrenute ka igracu i na svakih 2 sekunde se 2 ispale ka igracu
    [SerializeField] private List<GameObject> shootingHoles;
    private int shootHoleToEnable;

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnSpawnTwoHolesAroundHead(EnableTwoShootingHoles);
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

    //svake 2 sekunde treba da ispali iz 2 rupe
    private void ShootFromTwoHoles()
    {

    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnSpawnTwoHolesAroundHead(EnableTwoShootingHoles);
    }
}