using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Dish : MonoBehaviour {
    private LevelStats stats;
    private XRGrabInteractable grabInteractable;
    private IXRSelectInteractor initialSocket;
    private bool grabbed;
    private int selectCount = 0;

    void Start() {
        stats = Level2Manager.instance.stats;
        grabInteractable = GetComponent<XRGrabInteractable>();

        initialSocket = grabInteractable.firstInteractorSelecting;


        grabInteractable.selectEntered.AddListener((call) => {
            IXRSelectInteractor interactor = call.interactorObject;

            selectCount++;
            if(interactor is XRDirectInteractor) {
                grabbed = true;
            }
            if(grabbed && interactor is XRSocketInteractor && interactor != initialSocket) {
                if(selectCount == 2) {
                    stats.RegisterCorrectPlacement();
                } else {
                    stats.RegisterIncorrectPlacement();
                }
                grabInteractable.selectEntered.RemoveAllListeners();
            }
        });
    }
}
