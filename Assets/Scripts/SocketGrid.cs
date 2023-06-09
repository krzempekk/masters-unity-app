using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class SocketGrid: MonoBehaviour {
    public XRSocketInteractor firstSocket;
    public int redCount;
    public int blueCount;
    public int greenCount;
    public bool disableFirst = false;
    public float redOffset = 0.02f;
    public float blueOffset = 0.02f;
    public float greenOffset = 0.02f;

    private void Awake() {
        for(int r = 0; r < redCount; r++) {
            for(int b = 0; b < blueCount; b++) {
                for(int g = 0; g < greenCount; g++) {
                    if(r == 0 && b == 0 && g == 0) {
                        continue;
                    }

                    Vector3 position = firstSocket.transform.position 
                        + r * redOffset * firstSocket.transform.right
                        + b * blueOffset * firstSocket.transform.forward
                        + g * greenOffset * firstSocket.transform.up;

                    XRSocketInteractor socket = Instantiate(
                        firstSocket, 
                        position, 
                        firstSocket.transform.rotation, 
                        transform
                    );
                }
            }
        }

        firstSocket.socketActive = !disableFirst;
    }
}