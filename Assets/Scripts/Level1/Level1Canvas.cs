using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Level1Canvas : MonoBehaviour {
    public GameObject itemNumberSlider;
    public TMP_Dropdown greenChestDropdown;
    public TMP_Dropdown purpleChestDropdown;
    public Toggle lockedChestsToggle;
    public Toggle extraItemsToggle;
    public Toggle disableTutorialToggle;

    private Level1Settings level1Settings;

    void Start() {
        level1Settings = (Level1Settings) SettingsManager.GetLevelSettings(0);

        Slider slider = itemNumberSlider.GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
        slider.value = level1Settings.cubeNumber;

        List<string> optionLabels = new List<string>(){"Losowo", "Maskotki", "Autka", "Klocki"};
        
        greenChestDropdown.AddOptions(optionLabels);
        greenChestDropdown.onValueChanged.AddListener((int option) => {
            if(option != 0 && purpleChestDropdown.value == option) {
                purpleChestDropdown.value = 0;
                level1Settings.purpleChestItem = -1;
            }
            level1Settings.greenChestItem = option - 1;
        });
        greenChestDropdown.value = level1Settings.greenChestItem + 1;

        purpleChestDropdown.AddOptions(optionLabels);
        purpleChestDropdown.onValueChanged.AddListener((int option) => {
            if(option != 0 && greenChestDropdown.value == option) {
                greenChestDropdown.value = 0;
                level1Settings.greenChestItem = -1;
            }
            level1Settings.purpleChestItem = option - 1;
        });
        purpleChestDropdown.value = level1Settings.purpleChestItem + 1;

        lockedChestsToggle.onValueChanged.AddListener((bool chestsLocked) => {
            level1Settings.isChestLocked = chestsLocked;
        });
        lockedChestsToggle.isOn = level1Settings.isChestLocked;

        extraItemsToggle.onValueChanged.AddListener((bool extraItems) => {
            level1Settings.extraItems = extraItems;
        });
        extraItemsToggle.isOn = level1Settings.extraItems;

        disableTutorialToggle.onValueChanged.AddListener((bool disableTutorial) => {
            level1Settings.disableTutorial = disableTutorial;
        });
        disableTutorialToggle.isOn = level1Settings.disableTutorial;
    }

    public void OnSliderChanged(System.Single value) {
        int cubeNumber = (int) value;
        itemNumberSlider.GetComponentInChildren<TextMeshProUGUI>().text = "" + cubeNumber;
        level1Settings.cubeNumber = cubeNumber;
    }
}