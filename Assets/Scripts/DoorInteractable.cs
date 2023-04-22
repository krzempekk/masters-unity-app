using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum NormalType {
    RED,
    RED_NEGATIVE,
    GREEN,
    GREEN_NEGATIVE,
    BLUE,
    BLUE_NEGATIVE
};

public class DoorInteractable : XRBaseInteractable {
    public Transform doors;
    public float minAngle = 0;
    public float currentAngle = 0;
    public float maxAngle = 90;

    public NormalType surfaceNormal = NormalType.BLUE;
    public NormalType wallNormal = NormalType.RED;

    public Vector3 angleVector = new Vector3(0, -1, 0);

    private IXRSelectInteractor interactor = null;
    private Plane surfacePlane;
    private Plane wallPlane;

    private Vector3 GetNormal(Transform transform, NormalType normalType) {
        switch(normalType) {
            case NormalType.RED:
                return transform.right;
            case NormalType.RED_NEGATIVE:
                return -transform.right;
            case NormalType.GREEN:
                return transform.up;
            case NormalType.GREEN_NEGATIVE:
                return -transform.up;
            case NormalType.BLUE:
                return transform.forward;
            case NormalType.BLUE_NEGATIVE:
                return -transform.forward;
            default:
                return Vector3.zero;
        }
    }

    void Start() {
        surfacePlane = new Plane(GetNormal(doors, surfaceNormal), doors.position);
        wallPlane = new Plane(GetNormal(doors, wallNormal), doors.position);
    }

    void UpdateRotation() {
        Vector3 point = interactor.transform.position;
        Vector3 projectedPoint = surfacePlane.ClosestPointOnPlane(point);
        Vector3 hingePoint = wallPlane.ClosestPointOnPlane(projectedPoint);

        if(surfacePlane.GetSide(point) != wallPlane.GetSide(point)) {
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

    void ApplyRotation() {
        doors.localEulerAngles = currentAngle * angleVector;
    }

    void Update() {
        if(interactor != null) {
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
