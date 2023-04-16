using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class BasicContainer : MonoBehaviour {
    public string itemCategoryTag = "";
    public InteractionLayerMask interactionLayerMask;

    public UnityEvent<int> OnCorrectPlacement;
    public UnityEvent<int> OnCorrectPlacementExit;

    public UnityEvent OnIncorrectPlacement;
    public UnityEvent OnIncorrectPlacementExit;

    public UnityEvent OnCompleted;
    public UnityEvent OnCompletedExit;

    public UnityEvent OnUniqueCorrectPlacement;
    public UnityEvent OnUniqueIncorrectPlacement;


    private int correctItemsCount = 0;
    private int incorrectItemsCount = 0;
    private int totalCorrectItemsCount = 0;
    private bool isCompleted = false;

    private List<Collider> previousCorrectColliders = new List<Collider>();
    private List<Collider> previousIncorrectColliders = new List<Collider>();

    public void ResetContainer(int totalCount) {
        correctItemsCount = 0;
        incorrectItemsCount = 0;
        totalCorrectItemsCount = totalCount;

        isCompleted = false;

        previousCorrectColliders.Clear();
        previousIncorrectColliders.Clear();

        OnCorrectPlacement.RemoveAllListeners();
        OnCorrectPlacementExit.RemoveAllListeners();
        OnIncorrectPlacement.RemoveAllListeners();
        OnIncorrectPlacementExit.RemoveAllListeners();
        OnCompleted.RemoveAllListeners();
        OnCompletedExit.RemoveAllListeners();
    }

    private bool CompareInteractionLayer(Collider other) {
        XRGrabInteractable interactable = other.GetComponent<XRGrabInteractable>();

        return interactable != null && (interactionLayerMask & interactable.interactionLayers) != 0;
    }

    private void CheckCompletedCondition() {
        if(correctItemsCount == totalCorrectItemsCount && incorrectItemsCount == 0) {
            isCompleted = true;
            OnCompleted.Invoke();
        } else if(isCompleted == true) {
            isCompleted = false;
            OnCompletedExit.Invoke();
        }
    }

    void OnTriggerEnter(Collider other) {
        if(!CompareInteractionLayer(other)) {
            return;
        }

        if(other.CompareTag(itemCategoryTag)) {
            correctItemsCount++;
            OnCorrectPlacement.Invoke(totalCorrectItemsCount - correctItemsCount);

            if(!previousCorrectColliders.Contains(other)) {
                previousCorrectColliders.Add(other);
                OnUniqueCorrectPlacement.Invoke();
            }
        } else {
            // incorrectItemsCount++;

            other.GetComponent<ItemReturn>().ReturnToInitialPosition();

            OnIncorrectPlacement.Invoke();

            if(!previousIncorrectColliders.Contains(other)) {
                previousIncorrectColliders.Add(other);
                OnUniqueIncorrectPlacement.Invoke();
            }
        } 

        CheckCompletedCondition();
    }

    void OnTriggerExit(Collider other) {
        if(!CompareInteractionLayer(other)) {
            return;
        }

        if(other.CompareTag(itemCategoryTag)) {
            correctItemsCount--;
            OnCorrectPlacementExit.Invoke(totalCorrectItemsCount - correctItemsCount);
        } else {
            // incorrectItemsCount--;
            OnIncorrectPlacementExit.Invoke();
        } 

        CheckCompletedCondition();
    }
}
