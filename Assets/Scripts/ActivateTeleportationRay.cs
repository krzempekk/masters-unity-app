using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateTeleportationRay : MonoBehaviour {
    public GameObject teleportationRay;
    public InputActionProperty activateRay;

    void Update() {
        teleportationRay.SetActive(activateRay.action.ReadValue<float>() > 0.1f);
    }
}
