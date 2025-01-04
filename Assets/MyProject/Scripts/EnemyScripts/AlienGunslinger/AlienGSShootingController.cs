using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AlienGSShootingController : MonoBehaviour
{
    [SerializeField] private float shootingTime = 2f;
    private AlienGunslingerController alienGunslingerController;
    private AlienGunslingerReferences references;
    private bool shouldShoot = false;
    private int punishShootingDamage = 30;

    private void Awake()
    {
        references = GetComponent<AlienGunslingerReferences>();
    }

    public void StunShoot()
    {
        //stun player and shoot at him for 1 second
        EventManager.Instance.OnPlayerHitAction(punishShootingDamage);
        alienGunslingerController.PunishOver();
    }

    public void Shoot()
    {
        var sourceObjects = new WeightedTransformArray();
        sourceObjects.Add(new WeightedTransform(references.Character.transform, 1f));
        references.LeftHandAimConstraint.data.sourceObjects = sourceObjects;
        references.RightHandAimConstraint.data.sourceObjects = sourceObjects;
        references.RigBuilder.Build();
        shouldShoot = true;

        StartCoroutine(StopShooting());
    }

    private IEnumerator StopShooting()
    {
        yield return new WaitForSeconds(shootingTime);
        references.LeftHandAimConstraint.data.sourceObjects.Clear();
        references.RightHandAimConstraint.data.sourceObjects.Clear();
        references.RigBuilder.Build();
        shouldShoot = false;
    }

    private void Update()
    {
        if (!shouldShoot) return;

        float speed = references.SideWalkSpeed;
        transform.localPosition += Vector3.right * speed * Time.deltaTime;//treba da hoda levo ili desno
    }

    public bool ShouldShoot() => shouldShoot;

}