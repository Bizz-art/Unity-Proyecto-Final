using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public Camera cameraToEnable;
    public Camera cameraToDisable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraToDisable.gameObject.SetActive(false);
            cameraToEnable.gameObject.SetActive(true);
        }
    }
}
