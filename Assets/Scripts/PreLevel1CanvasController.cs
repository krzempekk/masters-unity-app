using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreLevel1CanvasController: MonoBehaviour {
    public GameObject itemNumberSlider;
    public Toggle lockedChestsToggle;

    private Level1Settings level1Settings;

    void Start() {
        level1Settings = (Level1Settings) SettingsManager.GetLevelSettings(0);

        Slider slider = itemNumberSlider.GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
        slider.value = level1Settings.cubeNumber;

        lockedChestsToggle.onValueChanged.AddListener(OnChestCheckboxValueChanged);
        lockedChestsToggle.isOn = level1Settings.isChestLocked;
    }

    public void OnSliderChanged(System.Single value) {
        int cubeNumber = (int) value;
        itemNumberSlider.GetComponentInChildren<TextMeshProUGUI>().text = "" + cubeNumber;
        level1Settings.cubeNumber = cubeNumber;
    }

    public void OnChestCheckboxValueChanged(bool chestsLocked) {
        level1Settings.isChestLocked = chestsLocked;
    }
}
