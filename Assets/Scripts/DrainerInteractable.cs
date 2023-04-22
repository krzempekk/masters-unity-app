using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrainerInteractable : XRBaseInteractable {
    private IXRSelectInteractor interactor = null;
    public float minX = -2.0f;
    public float currentX = -2.0f;
    public float maxX = -1.65f;
    

    void Start() {
        
    }

    void Update() {
        if(interactor != null) {
            UpdatePosition();
        }

        ApplyPosition();
    }

    void UpdatePosition() {
        Vector3 point = interactor.transform.position;

        currentX = point.x - 0.2f;
        currentX = Mathf.Min(maxX, currentX);
        currentX = Mathf.Max(minX, currentX);
    }

    void ApplyPosition() {
        transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
    }

    protected override void OnEnable() {
        base.OnEnable();
        selectEntered.AddListener(StartGrab);
        selectExited.AddListener(EndGrab);
    }

    protected override void OnDisable() {
        selectEntered.RemoveListener(StartGrab);
        selectExited.RemoveListener(EndGrab);
        base.OnDisable();
    }

    private void StartGrab(SelectEnterEventArgs args) {
        interactor = args.interactorObject;
    }
    
    private void EndGrab(SelectExitEventArgs args) {
        interactor = null;
    }
}
