using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SystemCanvasController: MonoBehaviour {
    public Toggle distanceGrabToggle;
    public Toggle smoothMovementToggle;
    public Toggle tunnelingToggle;
    public Toggle teleportationToggle;
    public Toggle skipIntroductionToggle;
    
    private MainSettings mainSettings;

    void Start() {
        mainSettings = SettingsManager.GetMainSettings();

        distanceGrabToggle.onValueChanged.AddListener((distanceGrab) => {
            mainSettings.distanceGrab = distanceGrab;
            SettingsManager.SaveAndApplySettings();
        });
        distanceGrabToggle.isOn = mainSettings.distanceGrab;

        smoothMovementToggle.onValueChanged.AddListener((smoothMovement) => {
            mainSettings.smoothMovement = smoothMovement;
            SettingsManager.SaveAndApplySettings();
        });
        smoothMovementToggle.isOn = mainSettings.smoothMovement;

        tunnelingToggle.onValueChanged.AddListener((tunneling) => {
            mainSettings.tunneling = tunneling;
            SettingsManager.SaveAndApplySettings();
        });
        tunnelingToggle.isOn = mainSettings.tunneling;

        teleportationToggle.onValueChanged.AddListener((teleportation) => {
            mainSettings.teleportation = teleportation;
            SettingsManager.SaveAndApplySettings();
        });
        teleportationToggle.isOn = mainSettings.teleportation;

        skipIntroductionToggle.onValueChanged.AddListener((skipIntroduction) => {
            mainSettings.disableIntroduction = skipIntroduction;
            SettingsManager.SaveAndApplySettings();
        });
        skipIntroductionToggle.isOn = mainSettings.disableIntroduction;
    }
}
