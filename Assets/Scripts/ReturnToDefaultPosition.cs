using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReturnToDefaultPosition: MonoBehaviour {
    public UnityEvent OnOutOfBounds;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("XROrigin")) {
            OnOutOfBounds.Invoke();
        } else {
            ItemReturn itemReturn = other.gameObject.GetComponent<ItemReturn>();
            if(itemReturn != null) {
                itemReturn.ReturnToInitialPosition();
            }
        }
    }
}
