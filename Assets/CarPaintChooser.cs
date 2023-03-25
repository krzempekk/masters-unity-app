using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPaintChooser: MonoBehaviour {
    public Material[] materials;

    void Start() {
        SwitchCarPaint(Random.Range(0, materials.Length));
    }

    void SwitchCarPaint(int index) {
        GetComponent<MeshRenderer>().material = materials[index];
    }
}
