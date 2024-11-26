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
            Debug.LogWarning("Player Hit");
            EventManager.Instance.OnAlienSMAttackHitAction();
        }
    }
}