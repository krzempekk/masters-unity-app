using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishPlate: MonoBehaviour {
    public Mesh[] meshes;
    public int colorIndex;

    void Start() {
        InitializePlate(-1);
    }

    void InitializePlate(int _colorIndex) {
        if(_colorIndex >= 0 && _colorIndex < meshes.Length) {
            colorIndex = _colorIndex;
        } else {
            colorIndex = Random.Range(0, meshes.Length);
        }
        GetComponent<MeshFilter>().mesh = meshes[colorIndex];
    }

    void Update() {
        
    }
}
