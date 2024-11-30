using PDollarGestureRecognizer;
using UnityEngine;

public class SwordComboController : MonoBehaviour
{
    [SerializeField] private SwordData swordData;
    private bool isSwordInHand;
    private bool isComboActive;

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnDidGesture(SwordGesture);
        EventManager.Instance.SubscribeToOnPickupWeaponAction(SwordPickedUp);
        EventManager.Instance.SubscribeToOnReleaseWeaponAction(SwordReleased);
        EventManager.Instance.SubscribeToOnComboAttackStarted(ComboAttackIsActive);
        EventManager.Instance.SubscribeToOnComboAttackFinished(ComboAttackFinished);
    }

    private void ComboAttackIsActive()
    {
        isComboActive = true;
    }

    private void ComboAttackFinished()
    {
        isComboActive = false;
    }

    //napravi da se ovaj kombo pokrece ako je napisao slovo S pustio a onda sledece slovo koje je napisao je O kada napise S treba da se odradi animacija za Million Stabs, a cim se zavrsi ako napise O onda neka ga gurne u napred kao stinger
    private void SwordGesture(Result result, GameObject source)
    {
        SwordComboController sword = source.GetComponent<HandData>().WeaponInHand.GetComponent<SwordComboController>();
        if (!sword || isComboActive)
            return;

        if (result.GestureClass == "S")
        {
            isComboActive = true;
            Debug.Log("STINGER");
        }

        if (result.GestureClass == "X")
        {
            isComboActive = true;
            Debug.Log("THOUSAND SLASHES");
        }
    }

    private void SwordPickedUp(GameObject weapon, HandController hand)
    {
        if (weapon.GetComponent<SwordComboController>())
            isSwordInHand = true;
    }

    private void SwordReleased(GameObject weapon, HandData handData)
    {
        if (weapon.GetComponent<SwordComboController>())
            isSwordInHand = false;
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnComboAttackFinished(ComboAttackFinished);
        EventManager.Instance.UnsubscribeFromOnComboAttackStarted(ComboAttackIsActive);
        EventManager.Instance.UnsubscribeFromOnDidGesture(SwordGesture);
        EventManager.Instance.UnsubscribeFromOnPickupWeaponAction(SwordPickedUp);
        EventManager.Instance.UnsubscribeFromOnReleaseWeaponAction(SwordReleased);
    }

    public SwordData GetSwordData() => swordData;
}
