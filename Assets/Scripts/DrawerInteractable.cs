using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawerInteractable : XRBaseInteractable {
    public float minValue = -2.0f;
    public float currentValue = -2.0f;
    public float maxValue = -1.65f;
    public float offset = -0.2f;
    public Vector3 axisVector = new Vector3(1, 0, 0);

    private IXRSelectInteractor interactor = null;

    void Update() {
        if(interactor != null) {
            UpdatePosition();
        }

        ApplyPosition();
    }

    void UpdatePosition() {
        float interactorValue = Vector3.Dot(interactor.transform.position, axisVector);

        currentValue = interactorValue + offset;
        currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
    }

    void ApplyPosition() {
        transform.position = Vector3.Scale(transform.position, Vector3.one - axisVector) + currentValue * axisVector;
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
