using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit;

public class Level1Tutorial : MonoBehaviour {
    public TutorialStep[] tutorialSteps;
    public XRBaseController[] controllers;

    public IEnumerator PlayTutorialRoutine() {
        // foreach(XRBaseController controller in controllers) {
        //     controller.gameObject.SetActive(false);
        // }

        foreach(TutorialStep step in tutorialSteps) {
            step.gameObject.SetActive(false);
        }

        foreach(TutorialStep step in tutorialSteps) {
            if(step.ShouldExecute()) {
                step.gameObject.SetActive(true);
                yield return step.Execute();
                step.gameObject.SetActive(false);
            }
        }

        // foreach(XRBaseController controller in controllers) {
        //     controller.gameObject.SetActive(true);
        // }
    }

    public void PlayTutorial() {
        StartCoroutine(PlayTutorialRoutine());
    }
}
