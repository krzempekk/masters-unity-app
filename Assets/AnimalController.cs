using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AnimalController : MonoBehaviour {
    public XRBaseInteractable headpatZone;
    private Animator animator;
    private bool isHovering = false;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        ChangeToSleep();

        headpatZone.hoverEntered.AddListener((call) => {
            isHovering = true;
            StartCoroutine(HeadpatsRoutine());
        });
        headpatZone.hoverExited.AddListener((call) => {
            isHovering = false;
            StartCoroutine(SleepRoutine());
        });
    }

    private IEnumerator HeadpatsRoutine() {
        yield return new WaitForSeconds(2);
        if(isHovering) {
            ChangeToHappy();
        }
    }

    private IEnumerator SleepRoutine() {
        yield return new WaitForSeconds(5);
        if(!isHovering) {
            ChangeToSleep();
        }
    }

    private void ChangeToHappy() {
        animator.Play("Idle_A");
        animator.Play("Eyes_Happy");
        animator.speed = 0.5f;
    }

    private void ChangeToSleep() {
        animator.Play("Sit");
        animator.Play("Eyes_Sleep");
        animator.speed = 0.25f;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
