using System.Collections;
using UnityEngine;

public class GunBigShotController : MonoBehaviour
{
    [SerializeField] private GameObject beam;
    [SerializeField] private ParticleSystem projectile;
    [SerializeField] private ParticleSystem bigShot;
    [SerializeField] private Transform bigShotShootingPoint;
    [SerializeField] private GunData gunData;

    private bool isComboActive;

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToBigShotAction(BigShot);
        EventManager.Instance.SubscribeToOnComboAttackFinished(ComboAttackFinished);
        EventManager.Instance.SubscribeToOnComboAttackStarted(ComboAttackIsActive);
        EventManager.Instance.SubscribeToOnBigShotAnimationDoneAction(BigShotAnimationDone);
        EventManager.Instance.SubscribeToOnFireBigBulletAction(FireBigBullet);
    }

    private void ComboAttackIsActive()
    {
        isComboActive = true;
    }

    private void ComboAttackFinished()
    {
        isComboActive = false;
    }

    public void BigShot(Animator holdingHandAnimator)
    {
        isComboActive = true;
        holdingHandAnimator.SetBool("BigShot", true);
        EventManager.Instance.OnComboAttackStartedAction();
    }

    public void BigShotAnimationDone(Animator animator)
    {
        animator.SetBool("BigShot", false);
    }

    public void FireBigBullet()
    {

        RaycastHit hit;
        Vector3 targetPosition;
        if (Physics.Raycast(bigShotShootingPoint.position, bigShotShootingPoint.forward, out hit, gunData.BigBulletWeaponRange))
            targetPosition = hit.point;
        else
            targetPosition = bigShotShootingPoint.position + bigShotShootingPoint.forward * gunData.BigBulletSpeed;

        StartCoroutine(MoveBullet(targetPosition));
    }

    private IEnumerator MoveBullet(Vector3 targetPoint)
    {
        beam.SetActive(true);
        beam.transform.SetParent(null);
        projectile.gameObject.transform.SetParent(null);
        Vector3 startPosition = bigShotShootingPoint.position;
        float distance = Vector3.Distance(startPosition, targetPoint);
        float travelTime = distance / gunData.BigBulletSpeed;
        float elapsedTime = 0f;


        while (elapsedTime < travelTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / travelTime;
            projectile.transform.position = Vector3.Lerp(startPosition, targetPoint, t);

            yield return null;
        }

        projectile.transform.position = targetPoint;
        beam.gameObject.SetActive(false);
        beam.transform.SetParent(bigShotShootingPoint);
        projectile.gameObject.transform.SetParent(beam.transform);
        projectile.transform.localEulerAngles = new Vector3(-90, 0, 0);
    }

    public void BigShot()
    {
        bigShot.Play();
    }

    public bool IsComboActive() => isComboActive;

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromBigShotAction(BigShot);
        EventManager.Instance.UnsubscribeFromOnBigShotAnimationDoneAction(BigShotAnimationDone);
        EventManager.Instance.UnsubscribeFromOnFireBigBulletAction(FireBigBullet);
        EventManager.Instance.UnsubscribeFromOnComboAttackStarted(ComboAttackIsActive);
        EventManager.Instance.UnsubscribeFromOnComboAttackFinished(ComboAttackFinished);
    }
}