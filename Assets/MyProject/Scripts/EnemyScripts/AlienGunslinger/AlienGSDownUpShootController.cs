using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AlienGSDownUpShootController : MonoBehaviour
{
    [SerializeField] private Vector3 startingTargetPosition;
    [SerializeField] private Transform shootingTarget;
    [SerializeField] private float distanceToTravel;
    [SerializeField] private float duration;
    [SerializeField] private MultiAimConstraint spineAimConstraint;
    private bool doneShooting;
    private bool shotPlayer;
    private AlienGunslingerReferences references;
    private AlienGunslingerController alienGunslingerController;
    private Coroutine shootingCoroutine;
    private GameObject helperTargetObject;

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnPlayerHitAction(ShotPlayer);
    }

    private void ShotPlayer(int dmg)
    {
        if (alienGunslingerController.GetCurrentState() == typeof(AGS_State_DownUpShoot))
            shotPlayer = true;
    }

    private void Start()
    {
        alienGunslingerController = GetComponent<AlienGunslingerController>();
        references = GetComponent<AlienGunslingerReferences>();
        helperTargetObject = Instantiate(new GameObject());
    }

    public void StartShootDownUp()
    {
        shootingCoroutine = StartCoroutine(ShootDownUpCoroutine());
        references.ShootingController.StartDownUpShoot();
        SetupRigBuilderConstraints();
    }

    private void SetupRigBuilderConstraints()
    {
        spineAimConstraint.gameObject.SetActive(true);
        helperTargetObject.transform.position = references.Character.transform.position;
        var sourceObjects = new WeightedTransformArray();
        sourceObjects.Add(new WeightedTransform(helperTargetObject.transform, 1f));
        references.HipsAimConstraint.data.sourceObjects = sourceObjects;
        references.SpineAimContraint.data.sourceObjects = sourceObjects;
        references.RigBuilder.Build();
    }

    private IEnumerator ShootDownUpCoroutine()
    {
        shootingTarget.localPosition = startingTargetPosition;
        Vector3 startingPosition = shootingTarget.localPosition;
        Vector3 targetPosition = shootingTarget.localPosition + Vector3.up * distanceToTravel;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            shootingTarget.localPosition = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        shootingTarget.localPosition = targetPosition;
        doneShooting = true;
    }

    public void ResetDownUpShooting()
    {
        StopCoroutine(shootingCoroutine);
        ResetRigBuilderConstraints();
        doneShooting = false;
        shotPlayer = false;
        references.ShootingController.StopDownUpShooting();
    }

    private void ResetRigBuilderConstraints()
    {
        spineAimConstraint.gameObject.SetActive(false);
        var sourceObjects = new WeightedTransformArray();
        sourceObjects.Add(new WeightedTransform(references.Character.transform, 1f));
        references.HipsAimConstraint.data.sourceObjects = sourceObjects;
        references.SpineAimContraint.data.sourceObjects = sourceObjects;
        references.RigBuilder.Build();
    }

    private void OnDestroy()
    {
        Destroy(helperTargetObject);
    }

    public bool DoneShooting() => doneShooting;
    public bool IsShotPlayer() => shotPlayer;
}