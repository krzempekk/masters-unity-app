using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AnimalController : MonoBehaviour {
    public XRBaseInteractable headpatZone;
    public XRSocketInteractor bananaSocket;
    private Animator animator;
    private bool isHovering = false;

    // Start is called before the first frame update
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

        bananaSocket.selectEntered.AddListener((call) => {
            StartCoroutine(EatingRoutine());
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

    private IEnumerator EatingRoutine() {
        ChangeToEating();
        yield return new WaitForSeconds(3);
        Destroy(bananaSocket.firstInteractableSelected.transform.gameObject);
        ChangeToHappy();
    }

    private void ChangeToHappy() {
        animator.Play("Fox_Jump_Pivot_InPlace");
    }

    private void ChangeToIdle() {
        animator.Play("Fox_Idle");
    }

    private void ChangeToEating() {
        animator.Play("Fox_Falling");
    }

}
