using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;


public class Level0Manager : MonoBehaviour {
    public static Level0Manager instance { get; private set; }
    private MainSettings settings;
    public Tutorial tutorial;

    private void Awake() { 
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
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
}