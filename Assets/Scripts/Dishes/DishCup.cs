using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishCup : MonoBehaviour {
    public void InitializeCup(Mesh mesh, string tag) {
        GetComponent<MeshFilter>().mesh = mesh;
        transform.tag = tag;    
    }
}
