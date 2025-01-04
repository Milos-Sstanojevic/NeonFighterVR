using UnityEngine;

public class AlienGunslingerAnimationEvents : MonoBehaviour
{
    public void DrawGunsAnimationFinished()
    {
        EventManager.Instance.OnDrawGunsAnimationFinished();
    }

    public void SpawnTwoHolesAroundHead()
    {
        EventManager.Instance.OnSpawnTwoHolesAroundHead();
    }

    public void SpawningHolesAroundHeadFinished()
    {
        EventManager.Instance.OnSpawningHolesAroundHeadFinished();
    }

}