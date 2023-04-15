using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit;

public class Level1Tutorial : MonoBehaviour {
    public TutorialStep[] tutorialSteps;
    public XRBaseController[] controllers;
    public Light directionalLight;

    public IEnumerator SmoothLight(float startIntensity, float endIntensity, float duration = 5.0f) {
        float time = 0;
        while(time < 1) {
            time += Time.deltaTime / duration; 
            directionalLight.intensity = Mathf.Lerp(startIntensity, endIntensity, time);
            yield return null;
        }
    }

    public IEnumerator PlayTutorialRoutine() {
        // foreach(XRBaseController controller in controllers) {
        //     controller.gameObject.SetActive(false);
        // }

        directionalLight.intensity = 0;

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

        StartCoroutine(SmoothLight(0, 0.6f));

        // foreach(XRBaseController controller in controllers) {
        //     controller.gameObject.SetActive(true);
        // }
    }

    public void PlayTutorial() {
        StartCoroutine(PlayTutorialRoutine());
    }
}
