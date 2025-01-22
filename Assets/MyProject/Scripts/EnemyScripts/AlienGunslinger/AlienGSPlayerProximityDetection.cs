using UnityEngine;

public class AlienGSPlayerProximityDetection : MonoBehaviour
{
    private AlienGSDashingController alienDashingController;

    private void Awake()
    {
        alienDashingController = GetComponentInParent<AlienGSDashingController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
            alienDashingController.StartDashingCoroutine();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>())
            alienDashingController.ResetDashingCoroutine();
    }
}