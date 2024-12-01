using System.Collections;
using UnityEngine;

public class GunBigShotController : MonoBehaviour
{
    [SerializeField] private ParticleSystem bigBullet;
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
        bigBullet.gameObject.SetActive(true);
        bigShot.Play();

        Vector3 startPosition = bigShotShootingPoint.position;
        float distance = Vector3.Distance(startPosition, targetPoint);
        float travelTime = distance / gunData.BigBulletSpeed;
        float elapsedTime = 0f;
        bigBullet.gameObject.transform.SetParent(null);

        while (elapsedTime < travelTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / travelTime;
            bigBullet.transform.position = Vector3.Lerp(startPosition, targetPoint, t);

            yield return null;
        }

        bigBullet.transform.position = targetPoint;
        bigBullet.gameObject.SetActive(false);
        bigBullet.gameObject.transform.SetParent(transform);
        bigBullet.transform.localPosition = Vector3.zero;
        bigBullet.transform.localEulerAngles = new Vector3(-90, 0, 0);
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