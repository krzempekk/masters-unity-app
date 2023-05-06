using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishPlate: MonoBehaviour {
    // public Mesh[] meshes;
    // public string[] tags;
    // public int colorIndex;

    void Start() {
        // InitializePlate(-1);
    }

    // public void InitializePlate(int _colorIndex) {
    //     if(_colorIndex >= 0 && _colorIndex < meshes.Length) {
    //         colorIndex = _colorIndex;
    //     } else {
    //         colorIndex = Random.Range(0, meshes.Length);
    //     }
    //     GetComponent<MeshFilter>().mesh = meshes[colorIndex];
    //     transform.tag = tags[colorIndex];
    // }

    public void InitializePlate(Mesh mesh, string tag) {
        GetComponent<MeshFilter>().mesh = mesh;
        transform.tag = tag;    
    }

    void Update() {
        
    }
}
