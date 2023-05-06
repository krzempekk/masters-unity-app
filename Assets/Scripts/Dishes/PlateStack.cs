using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class PlateStack: MonoBehaviour {

    public XRSocketInteractor firstSocket;
    public int maxHeight = 10;
    public int targetHeight = -1;
    public int currentHeight = 0;
    public UnityEvent OnPlacement;
    public UnityEvent OnPlacementExit;
    public UnityEvent OnCompleted;
    public UnityEvent OnCompletedExit;

    private List<XRSocketInteractor> sockets = new List<XRSocketInteractor>();

    void Start() {
        sockets.Add(firstSocket);

        for(int i = 1; i <= maxHeight; i++) {
            XRSocketInteractor socket = Instantiate(
                firstSocket, 
                firstSocket.transform.position + new Vector3(0, i * 0.02f, 0), 
                firstSocket.transform.rotation, 
                transform
            );
            socket.socketActive = false;
            sockets.Add(socket);
        }

        foreach(XRSocketInteractor socket in sockets) {
            socket.selectEntered.AddListener(IncreaseHeight);
            socket.selectExited.AddListener(DecreaseHeight);
        }

        sockets[0].socketActive = true;
    }

    private void IncreaseHeight(SelectEnterEventArgs args) {
        currentHeight++;
        sockets[currentHeight].socketActive = true;

        if(currentHeight > 1) {
            Transform interactable = sockets[currentHeight - 2].firstInteractableSelected.transform;
            interactable.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("Socket");
        }

        OnPlacement.Invoke();
        if(targetHeight > 0 && currentHeight == targetHeight) {
            OnCompleted.Invoke();
        }
    }

    public void DecreaseHeight(SelectExitEventArgs args) {
        OnPlacementExit.Invoke();
        if(targetHeight > 0 && currentHeight == targetHeight) {
            OnCompletedExit.Invoke();
        }

        sockets[currentHeight].socketActive = false;
        currentHeight--;

        if(currentHeight > 0) {
            Transform interactable = sockets[currentHeight - 1].firstInteractableSelected.transform;
            interactable.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("Socket", "Direct");
        }
    }

    public void Reset() {
        targetHeight = -1;
        currentHeight = 0;
        OnCompleted.RemoveAllListeners();
        OnCompletedExit.RemoveAllListeners();
    }
}
