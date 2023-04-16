using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SystemCanvasController: MonoBehaviour {
    public Toggle distanceGrabToggle;
    public GameObject heightSlider;
    
    private MainSettings mainSettings;

    void Start() {
        mainSettings = SettingsManager.GetMainSettings();

        distanceGrabToggle.onValueChanged.AddListener(OnRaysCheckboxValueChanged);
        distanceGrabToggle.isOn = mainSettings.distanceGrab;

        Slider slider = heightSlider.GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
        slider.value = mainSettings.height;
    }

    public void OnRaysCheckboxValueChanged(bool distanceGrab) {
        mainSettings.distanceGrab = distanceGrab;
        SettingsManager.SaveSettings();
    }

    public void OnSliderChanged(System.Single height) {
        heightSlider.GetComponentInChildren<TextMeshProUGUI>().text = "" + height;
        mainSettings.height = height;
        SettingsManager.SaveSettings();
    }
}
