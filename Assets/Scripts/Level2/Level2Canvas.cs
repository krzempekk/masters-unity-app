using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Level2Canvas : MonoBehaviour {
    public GameObject platesNumberSlider;
    public GameObject cupsNumberSlider;

    private Level2Settings level2Settings;

    void Start() {
        level2Settings = (Level2Settings) SettingsManager.GetLevelSettings(1);

        Slider platesSlider = platesNumberSlider.GetComponentInChildren<Slider>();
        platesSlider.onValueChanged.AddListener(OnPlatesSliderChanged);
        platesSlider.value = level2Settings.platesNumber;

        Slider cupsSlider = cupsNumberSlider.GetComponentInChildren<Slider>();
        cupsSlider.onValueChanged.AddListener(OnCupsSliderChanged);
        cupsSlider.value = level2Settings.cupsNumber;
    }

    public void OnPlatesSliderChanged(System.Single value) {
        int platesNumber = (int) value;
        platesNumberSlider.GetComponentInChildren<TextMeshProUGUI>().text = "" + platesNumber;
        level2Settings.platesNumber = platesNumber;
    }

    public void OnCupsSliderChanged(System.Single value) {
        int cupsNumber = (int) value;
        cupsNumberSlider.GetComponentInChildren<TextMeshProUGUI>().text = "" + cupsNumber;
        level2Settings.cupsNumber = cupsNumber;
    }

}