using PDollarGestureRecognizer;
using UnityEngine;

public class SwordComboController : MonoBehaviour
{
    [SerializeField] private SwordData swordData;
    private bool isSwordInHand;

    private void OnEnable()
    {
        EventManager.Instance.SubscribeToOnDidGesture(StingerCombo);
        EventManager.Instance.SubscribeToOnPickupWeaponAction(SwordPickedUp);
        EventManager.Instance.SubscribeToOnReleaseWeaponAction(SwordReleased);
    }

    //napravi da se ovaj kombo pokrece ako je napisao slovo S pustio a onda sledece slovo koje je napisao je O kada napise S treba da se odradi animacija za Million Stabs, a cim se zavrsi ako napise O onda neka ga gurne u napred kao stinger
    private void StingerCombo(Result result, GameObject weapon)
    {
        if (result.GestureClass == "S" && weapon.GetComponent<HandController>().WeaponInHand.GetComponent<SwordComboController>())
            Debug.Log("STINGER");
    }

    private void SwordPickedUp(GameObject weapon, HandController hand)
    {
        if (weapon.GetComponent<SwordComboController>())
            isSwordInHand = true;
    }

    private void SwordReleased(GameObject weapon)
    {
        if (weapon.GetComponent<SwordComboController>())
            isSwordInHand = false;
    }

    private void OnDisable()
    {
        EventManager.Instance.UnsubscribeFromOnDidGesture(StingerCombo);
        EventManager.Instance.UnsubscribeFromOnPickupWeaponAction(SwordPickedUp);
        EventManager.Instance.UnsubscribeFromOnReleaseWeaponAction(SwordReleased);
    }

    public SwordData GetSwordData() => swordData;
}
