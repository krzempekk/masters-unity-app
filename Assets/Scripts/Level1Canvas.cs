using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Level1Canvas : MonoBehaviour {
    public GameObject itemNumberSlider;
    public Toggle lockedChestsToggle;
    public Toggle extraItemsToggle;

    private Level1Settings level1Settings;

    void Start() {
        level1Settings = (Level1Settings) SettingsManager.GetLevelSettings(0);

        Slider slider = itemNumberSlider.GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
        slider.value = level1Settings.cubeNumber;

        lockedChestsToggle.onValueChanged.AddListener(OnChestCheckboxValueChanged);
        lockedChestsToggle.isOn = level1Settings.isChestLocked;

        extraItemsToggle.onValueChanged.AddListener(OnExtraItemsCheckboxValueChanged);
        extraItemsToggle.isOn = level1Settings.extraItems;
    }

    public void OnSliderChanged(System.Single value) {
        int cubeNumber = (int) value;
        itemNumberSlider.GetComponentInChildren<TextMeshProUGUI>().text = "" + cubeNumber;
        level1Settings.cubeNumber = cubeNumber;
    }

    public void OnChestCheckboxValueChanged(bool chestsLocked) {
        level1Settings.isChestLocked = chestsLocked;
    }

    public void OnExtraItemsCheckboxValueChanged(bool extraItems) {
        level1Settings.extraItems = extraItems;
    }

}