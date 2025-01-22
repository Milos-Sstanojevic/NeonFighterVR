using UnityEngine;

public class AGS_State_DownUpShoot : IState
{
    private AlienGunslingerReferences references;

    public AGS_State_DownUpShoot(AlienGunslingerReferences references)
    {
        this.references = references;
    }

    public void OnEnter()
    {
        Debug.Log("Down Up shot");
        float distanceToPlayer = (references.transform.position - references.Character.transform.position).magnitude;
        // float requiredDistanceSqr = references.DistanceForDownUpShoot * references.DistanceForDownUpShoot;
        float distanceToDash = distanceToPlayer - references.DistanceForDownUpShoot;
        Debug.LogError(distanceToDash);
        if (distanceToDash <= -2)
            references.DashingController.DashToMakeDistance(-distanceToDash); //- because if he is closer than required distance this number is always negative so i make it positive

        references.Animator.SetBool("StandShoot", true);
        references.DownUpShootController.StartShootDownUp();
    }


    public void OnExit()
    {
        references.Animator.SetBool("StandShoot", false);
        references.DownUpShootController.ResetDownUpShooting();
    }

    public void Tick()
    {
    }
}