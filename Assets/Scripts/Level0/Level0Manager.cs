using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;


public class Level0Manager : MonoBehaviour {
    public static Level0Manager instance { get; private set; }
    private MainSettings settings;
    public Tutorial tutorial;
    public GameObject XROrigin;
    public FadeScreen fadeScreen;
    private Vector3 initialOriginPos;
    private Quaternion initialOriginRot;

    private void Awake() { 
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
            initialOriginPos = XROrigin.transform.position;
            initialOriginRot = XROrigin.transform.rotation;
        } 
    }

     void Start() {
        settings = SettingsManager.GetMainSettings();

        if(!settings.disableIntroduction) {
            tutorial.PlayTutorial();
            settings.disableIntroduction = true;
            SettingsManager.SaveAndApplySettings();
        }
    }

    public void ResetPosition() {
        StartCoroutine(ResetPositionRoutine());
    }

    private IEnumerator ResetPositionRoutine() {
        fadeScreen.FadeOutAndIn();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        XROrigin.transform.position = initialOriginPos;
        XROrigin.transform.rotation = initialOriginRot;
    }

}