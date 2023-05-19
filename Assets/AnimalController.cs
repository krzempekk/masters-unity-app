using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AnimalController : MonoBehaviour {
    private Animator animator;
    private XRSimpleInteractable interactable;
    private bool routineTriggered = false;

    void Start() {
        animator = GetComponent<Animator>();
        interactable = GetComponent<XRSimpleInteractable>();

        ChangeToIdle();

        interactable.firstHoverEntered.AddListener((call) => {
            if(!routineTriggered) {
                routineTriggered = true;
                StartCoroutine(GazeHoverRoutine());
            }
        });

        // interactable.lastHoverExited.AddListener((call) => {
        //     isHovering = false;
        //     StartCoroutine(IdleRoutine());
        // });
    }

    private IEnumerator GazeHoverRoutine() {
        yield return new WaitForSeconds(1);
        ChangeToHappy();
        yield return new WaitForSeconds(2.5f);
        ChangeToIdle();
        routineTriggered = false;
    }

    // private IEnumerator IdleRoutine() {
    //     yield return new WaitForSeconds(3);
    //     if(!isHovering) {
    //         ChangeToIdle();
    //     }
    // }

    private void ChangeToHappy() {
        animator.Play("Fox_Attack_Tail");
        animator.speed = 0.6f;
    }

    private void ChangeToIdle() {
        animator.Play("Fox_Idle");
        animator.speed = 1.0f;
    }
}
