using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Level2Canvas : MonoBehaviour {
    public GameObject platesNumberSlider;

    private Level2Settings level2Settings;

    void Start() {
        level2Settings = (Level2Settings) SettingsManager.GetLevelSettings(1);

        Slider slider = platesNumberSlider.GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
        slider.value = level2Settings.platesNumber;
    }

    public void OnSliderChanged(System.Single value) {
        int platesNumber = (int) value;
        platesNumberSlider.GetComponentInChildren<TextMeshProUGUI>().text = "" + platesNumber;
        level2Settings.platesNumber = platesNumber;
    }
}