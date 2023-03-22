using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestLid : XRBaseInteractable {
    public Transform lid;
    public float minAngle = 0;
    public float currentAngle = 0;
    public float maxAngle = 90;
    private Vector3 initialRotation;
    private bool isGrabbing = false;
    private IXRSelectInteractor interactor;
    private Plane surfacePlane;
    private Plane wallPlane;

    void Start() {
        surfacePlane = new Plane(lid.up, lid.position);
        wallPlane = new Plane(lid.forward, lid.position);
    }

    void ApplyRotation() {
        lid.localEulerAngles = new Vector3(currentAngle, 0, 0);
    }

    void UpdateRotation() {
        Vector3 point = interactor.transform.position;
        Vector3 projectedPoint = surfacePlane.ClosestPointOnPlane(point);
        Vector3 hingePoint = wallPlane.ClosestPointOnPlane(projectedPoint);

        if(surfacePlane.GetSide(point) == wallPlane.GetSide(point)) {
            return;
        }

        float angle = Mathf.Atan(Vector3.Distance(point, projectedPoint) / Vector3.Distance(projectedPoint, hingePoint)) * Mathf.Rad2Deg;
        if (angle < minAngle) {
            angle = minAngle;
        } else if(angle > maxAngle) {
            angle = maxAngle;
        }
        currentAngle = angle;
    }

    void Update() {
        if(isGrabbing) {
            UpdateRotation();
        }
        ApplyRotation();
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
        isGrabbing = true;
        interactor = args.interactorObject;
    }

    
    private void EndGrab(SelectExitEventArgs args) {
        isGrabbing = false;
        interactor = null;
    }
}
