using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownOrderFix: MonoBehaviour {
    private void OnEnable() {
        Canvas canvas = GetComponent<Canvas>();
        if(canvas) {
            canvas.sortingOrder = 2;
        }
    }
}
