using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class DishGrid : MonoBehaviour {
    public int targetCount = -1;
    public int currentCount = 0;
    public UnityEvent OnPlacement;
    public UnityEvent OnPlacementExit;
    public UnityEvent OnCompleted;
    public UnityEvent OnCompletedExit;
    private List<XRSocketInteractor> sockets = new List<XRSocketInteractor>();

    private void IncreaseCount(SelectEnterEventArgs args) {
        currentCount++;
        OnPlacement.Invoke();
        if(targetCount > 0 && currentCount == targetCount) {
            OnCompleted.Invoke();
        }
    }

    public void DecreaseCount(SelectExitEventArgs args) {
        OnPlacementExit.Invoke();
        if(targetCount > 0 && currentCount == targetCount) {
            OnCompletedExit.Invoke();
        }
        currentCount--;
    }


    public void Reset(int _targetCount) {
        targetCount = _targetCount;
        currentCount = 0;
        OnCompleted.RemoveAllListeners();
        OnCompletedExit.RemoveAllListeners();

        sockets.AddRange(GetComponentsInChildren<XRSocketInteractor>());

        foreach(XRSocketInteractor socket in sockets) {
            socket.selectEntered.AddListener(IncreaseCount);
            socket.selectExited.AddListener(DecreaseCount);
        }
    }
}
