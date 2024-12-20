using UnityEngine;

public class AlienSwordmasterSwordCollision : MonoBehaviour
{
    private AlienSwordmasterReferences references;

    private void Awake()
    {
        references = GetComponentInParent<AlienSwordmasterReferences>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            EventManager.Instance.OnAlienSMAttackHitAction();
            EventManager.Instance.OnPlayerHitAction(references.AlienSMData.Damage);
        }
    }
}