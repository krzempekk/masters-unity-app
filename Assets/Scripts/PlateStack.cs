using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlateStack: MonoBehaviour {

    public XRSocketInteractor firstSocket;
    public int maxHeight = 10;
    public int currentHeight = 0;
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

        // Debug.Log("Append " + sockets.Count);
        // XRSocketInteractor lastSocket = sockets[sockets.Count - 1];

        // lastSocket.selectEntered.RemoveListener(AppendSocket);
        // lastSocket.selectExited.AddListener(RemoveSocket);
        // lastSocket.allowHover = false;

        // if(sockets.Count > 1) {
        //     XRSocketInteractor previousSocket = sockets[sockets.Count - 2];
        //     previousSocket.selectExited.RemoveListener(RemoveSocket);
        //     XRGrabInteractable previousTopPlate = previousSocket.firstInteractableSelected.transform.GetComponent<XRGrabInteractable>();
        //     previousTopPlate.interactionLayers = InteractionLayerMask.GetMask("Socket");
        // }
        
        // newSocket.selectEntered.AddListener(AppendSocket);

        // sockets.Add(newSocket);
    }

    public void DecreaseHeight(SelectExitEventArgs args) {
        sockets[currentHeight].socketActive = false;
        currentHeight--;

        if(currentHeight > 0) {
            Transform interactable = sockets[currentHeight - 1].firstInteractableSelected.transform;
            interactable.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("Socket", "Direct");
        }

        // Debug.Log("Remove " + sockets.Count);
        // XRSocketInteractor lastSocket = sockets[sockets.Count - 1];
        // lastSocket.selectEntered.RemoveListener(AppendSocket);
        // sockets.Remove(lastSocket);
        // Destroy(lastSocket.gameObject);

        // lastSocket = sockets[sockets.Count - 1];
        // lastSocket.selectEntered.AddListener(AppendSocket);
        // lastSocket.allowHover = true;

        // if(sockets.Count > 1) {
        //     XRSocketInteractor previousSocket = sockets[sockets.Count - 2];
        //     previousSocket.selectExited.AddListener(RemoveSocket);
        //     XRGrabInteractable previousTopPlate = previousSocket.firstInteractableSelected.transform.GetComponent<XRGrabInteractable>();
        //     previousTopPlate.interactionLayers = InteractionLayerMask.GetMask("Socket", "Direct");
        // }
    }

    void Update() {
        
    }
}
