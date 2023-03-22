using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushieChooser: MonoBehaviour {
    public Mesh[] meshes;
    public Material[] materials;

    void Start() {
        SwitchPlushie(Random.Range(0, meshes.Length));
    }

    void Update() {
        
    }

    void SwitchPlushie(int index) {
        GetComponent<MeshFilter>().mesh = meshes[index];
        GetComponent<MeshRenderer>().material = materials[index];
    }
}
