using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SystemCanvasController: MonoBehaviour {
    public Toggle distanceGrabToggle;
    public Toggle smoothMovementToggle;
    public Toggle teleportationToggle;
    
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

        teleportationToggle.onValueChanged.AddListener((teleportation) => {
            mainSettings.teleportation = teleportation;
            SettingsManager.SaveAndApplySettings();
        });
        teleportationToggle.isOn = mainSettings.teleportation;
    }
}
