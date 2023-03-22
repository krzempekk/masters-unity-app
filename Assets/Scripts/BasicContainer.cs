using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class BasicContainer : MonoBehaviour {
    public string itemCategoryTag = "";
    public InteractionLayerMask interactionLayerMask;

    public UnityEvent OnCorrectPlacement;
    public UnityEvent OnIncorrectPlacement;
    public UnityEvent OnCompleted;


    private int correctItemsCount = 0;
    private int incorrectItemsCount = 0;
    private int totalCorrectItemsCount = 0;

    public void ResetContainer(int totalCount) {
        correctItemsCount = 0;
        incorrectItemsCount = 0;
        totalCorrectItemsCount = totalCount;
    }

    private bool CompareInteractionLayer(Collider other) {
        XRGrabInteractable interactable = other.GetComponent<XRGrabInteractable>();

        return interactable != null && (interactionLayerMask & interactable.interactionLayers) != 0;
    }

    private void CheckCompletedCondition() {
        if(correctItemsCount == totalCorrectItemsCount && incorrectItemsCount == 0) {
            OnCompleted.Invoke();
        }
    }

    void OnTriggerEnter(Collider other) {
        if(!CompareInteractionLayer(other)) {
            return;
        }

        if(other.CompareTag(itemCategoryTag)) {
            OnCorrectPlacement.Invoke();
            correctItemsCount++;
        } else {
            OnIncorrectPlacement.Invoke();
            incorrectItemsCount++;
        } 

        CheckCompletedCondition();
    }

    void OnTriggerExit(Collider other) {
        if(!CompareInteractionLayer(other)) {
            return;
        }

        if(other.CompareTag(itemCategoryTag)) {
            correctItemsCount--;
        } else {
            incorrectItemsCount--;
        } 

        CheckCompletedCondition();
    }
}
