using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReturn : MonoBehaviour {
    private Vector3 initialPos;
    private Quaternion initialRot;
    private float returnDuration = 1.5f;
    private bool isReturning = false;

    // Start is called before the first frame update
    void Start() {
        initialPos = gameObject.transform.position;
        initialRot = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ReturnToInitialPosition() {
        if(isReturning) {
            return;
        }
        isReturning = true;
        StartCoroutine(ReturnToInitialPositionRoutine());
    }

    private IEnumerator ReturnToInitialPositionRoutine() {
        Vector3 startPos = gameObject.transform.position;
        Quaternion startRot = gameObject.transform.rotation;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        
        float time = 0;
        while(time < 1) {
            time += Time.deltaTime / returnDuration; 
            gameObject.transform.position = Vector3.Lerp(startPos, initialPos, time);
            gameObject.transform.rotation = Quaternion.Lerp(startRot, initialRot, time);
            yield return null;
        }

        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        isReturning = false;
    }
}
