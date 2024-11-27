using UnityEngine;

public class AlienSwordmasterSwordCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            Debug.LogWarning("Player Hit");
            EventManager.Instance.OnAlienSMAttackHitAction();
        }
    }
}