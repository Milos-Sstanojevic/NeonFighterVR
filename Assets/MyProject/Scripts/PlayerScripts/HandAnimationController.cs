using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimationController : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerAction;
    [SerializeField] private InputActionProperty gripAction;

    private Animator animator;

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnReloadingInteruptAction(StopReloadingAnimation);
    }

    private void StopReloadingAnimation()
    {
        if (animator.GetBool("Reloading") == false)
            return;
        animator.SetBool("ReloadingInterupted", true);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    //ovo treba da se prepravi (da bude ono performed a ne ovako u Update)
    private void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>();
        float gripValue = gripAction.action.ReadValue<float>();

        animator.SetFloat("Trigger", triggerValue);
        animator.SetFloat("Grip", gripValue);
    }

    public void ReloadingAnimationFinished()
    {
        GetComponentInChildren<GunSpinner>().ReloadingAnimationFinished();
    }

    public void CircleOfReloadingDone()
    {
        GetComponentInChildren<GunController>().IncreaseCurrentAmmo();
    }

    public void GunReadyAfterReloadInterupt()
    {
        GunController gunController = GetComponentInChildren<GunController>();
        if (gunController)
            gunController.OnReadyToShoot();
    }


    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnReloadingInteruptAction(StopReloadingAnimation);
    }
}
