using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemCanvasController: MonoBehaviour {
    public Toggle distanceGrabToggle;
    
    private MainSettings mainSettings;

    void Start() {
        mainSettings = SettingsManager.GetMainSettings();

        distanceGrabToggle.onValueChanged.AddListener(OnRaysCheckboxValueChanged);
        distanceGrabToggle.isOn = mainSettings.distanceGrab;
    }

    public void OnRaysCheckboxValueChanged(bool distanceGrab) {
        mainSettings.distanceGrab = distanceGrab;
        SettingsManager.SaveSettings();
    }
}
