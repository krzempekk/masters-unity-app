using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestInteractable: XRBaseInteractable {
    public Transform chestLid;

    public float minAngle = 0;
    public float currentAngle = 0;
    public float maxAngle = 90;

    public bool isLocked = true;
    public XRSocketInteractor keyHole;
    public BasicContainer container;
    public GameObject key;
    public ItemCategory itemCategory;
    public MeshRenderer sticker;

    private IXRSelectInteractor interactor = null;
    private Plane surfacePlane;
    private Plane wallPlane;

    void Start() {
        surfacePlane = new Plane(chestLid.up, chestLid.position);
        wallPlane = new Plane(chestLid.forward, chestLid.position);

        keyHole.selectEntered.AddListener(UnlockChest);
        keyHole.selectExited.AddListener(LockChest);
    }

    public void ResetChest(bool locked) {
        currentAngle = locked ? minAngle : maxAngle;
        isLocked = locked;
    }

    private void UnlockChest(SelectEnterEventArgs args) {
        isLocked = false;
    }

    private void LockChest(SelectExitEventArgs args) {
        isLocked = true;
    }

    void ApplyRotation() {
        chestLid.localEulerAngles = new Vector3(currentAngle, 0, 0);
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
        if(interactor != null && (!isLocked || currentAngle - minAngle > 3)) {
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
        interactor = args.interactorObject;
    }

    
    private void EndGrab(SelectExitEventArgs args) {
        interactor = null;
    }
}
