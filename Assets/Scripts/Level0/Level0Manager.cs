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
    private Vector3 initialOriginPos;

    private void Awake() { 
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
            initialOriginPos = XROrigin.transform.position;
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
        XROrigin.transform.position = initialOriginPos;
    }
}