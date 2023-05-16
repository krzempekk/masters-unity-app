using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AnimalController : MonoBehaviour {
    public XRBaseInteractable headpatZone;
    private Animator animator;
    private bool isHovering = false;

    void Start() {
        animator = GetComponent<Animator>();
        ChangeToIdle();

        headpatZone.hoverEntered.AddListener((call) => {
            isHovering = true;
            StartCoroutine(HeadpatsRoutine());
        });

        headpatZone.hoverExited.AddListener((call) => {
            isHovering = false;
            StartCoroutine(IdleRoutine());
        });
    }

    private IEnumerator HeadpatsRoutine() {
        yield return new WaitForSeconds(2);
        if(isHovering) {
            ChangeToHappy();
        }
    }

    private IEnumerator IdleRoutine() {
        yield return new WaitForSeconds(4);
        if(!isHovering) {
            ChangeToIdle();
        }
    }

    private void ChangeToHappy() {
        animator.Play("Fox_Jump_Pivot_InPlace");
    }

    private void ChangeToIdle() {
        animator.Play("Fox_Idle");
    }
}
