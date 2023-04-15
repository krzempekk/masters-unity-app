using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOutline: MonoBehaviour {
    public GameObject[] outlinedObjects;

    private void OnEnable() {
        foreach(GameObject outlinedObject in outlinedObjects) {
            outlinedObject.GetComponent<Outline>().enabled = true;
        }
    }

    private void OnDisable() {
        foreach(GameObject outlinedObject in outlinedObjects) {
            outlinedObject.GetComponent<Outline>().enabled = false;
        }
    }
}
