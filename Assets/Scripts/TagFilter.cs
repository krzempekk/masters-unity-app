using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class TagFilter : MonoBehaviour, IXRHoverFilter, IXRSelectFilter {
    public string targetFilter;
    
    public bool canProcess => isActiveAndEnabled;

    public bool Process(IXRHoverInteractor interactor, IXRHoverInteractable interactable) {
        return interactable.transform.CompareTag(targetFilter);
    }

    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable) {
        return interactable.transform.CompareTag(targetFilter);
    }
}
