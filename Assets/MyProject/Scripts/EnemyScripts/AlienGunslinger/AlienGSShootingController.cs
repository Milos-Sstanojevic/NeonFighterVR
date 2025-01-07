using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AlienGSShootingController : MonoBehaviour
{
    [SerializeField] private float shootingTime = 2f;
    [SerializeField] private Transform shootingPoint;
    private AlienGunslingerController alienGunslingerController;
    private AlienGunslingerReferences references;
    private bool isShooting = false;
    private bool shouldMove = false;
    private int punishShootingDamage = 30;
    private Vector3 direction;
    private float nextFireRate;

    private void Awake()
    {
        references = GetComponent<AlienGunslingerReferences>();
    }

    public void StunShoot()
    {
        //stun player and shoot at him for 1 second
        shouldMove = false;
        EventManager.Instance.OnPlayerHitAction(punishShootingDamage);
        isShooting = true;
        StartCoroutine(StunShootCoroutine());
    }

    private IEnumerator StunShootCoroutine()
    {
        yield return new WaitForSeconds(1f);
        isShooting = false;
        alienGunslingerController.PunishOver();
    }

    public void SetupShooting()
    {
        direction = DirectionToMove();
        var sourceObjects = new WeightedTransformArray();
        sourceObjects.Add(new WeightedTransform(references.Character.transform, 1f));
        references.LeftHandAimConstraint.data.sourceObjects = sourceObjects;
        references.RightHandAimConstraint.data.sourceObjects = sourceObjects;
        references.RigBuilder.Build();
        isShooting = true;
        shouldMove = true;

        StartCoroutine(UnsetupShooting());
    }

    private IEnumerator UnsetupShooting()
    {
        yield return new WaitForSeconds(shootingTime);
        references.LeftHandAimConstraint.data.sourceObjects.Clear();
        references.RightHandAimConstraint.data.sourceObjects.Clear();
        references.RigBuilder.Build();
        isShooting = false;
        shouldMove = false;
    }

    private void Update()
    {
        if (isShooting && Time.time >= nextFireRate)
        {
            Shoot();
            nextFireRate = Time.time + references.GunData.FireRate;
        }

        if (!shouldMove) return;

        float speed = references.SideWalkSpeed;
        transform.localPosition += direction * speed * Time.deltaTime;
    }

    public Vector3 DirectionToMove()
    {
        float chance = Random.Range(0f, 1f);

        if (chance < 0.5f)
            return Vector3.right;
        else
            return Vector3.left;
    }

    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(shootingPoint.position, shootingPoint.forward, out hit, Mathf.Infinity))
            if (hit.collider.GetComponent<CharacterController>())
                EventManager.Instance.OnPlayerHitAction(references.GunData.GunDamage);
    }

    public bool IsShooting() => isShooting;

    public void StartShooting() => isShooting = true;
    public void StopShooting() => isShooting = false;

}