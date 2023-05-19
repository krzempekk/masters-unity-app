using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class ExcludeSelectedFilter : MonoBehaviour, IXRHoverFilter {
    public bool canProcess => isActiveAndEnabled;

    public bool Process(IXRHoverInteractor interactor, IXRHoverInteractable interactable) {
        XRGrabInteractable grabInteractable = interactable.transform.GetComponent<XRGrabInteractable>();
        XRSocketInteractor socketInteractor = interactor.transform.GetComponent<XRSocketInteractor>();
        return grabInteractable.firstInteractorSelecting is not XRSocketInteractor && socketInteractor.interactablesSelected.Count == 0;
    }
}
