using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Level1CanvasController : MonoBehaviour {
    public GameObject itemNumberSlider;
    public Toggle lockedChestsToggle;
    public Button submitButton;
    public UnityEvent onLevelSettingsApplied;

    private Level1Settings level1Settings;

    void Start() {
        level1Settings = (Level1Settings) SettingsManager.GetLevelSettings(0);

        Slider slider = itemNumberSlider.GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
        slider.value = level1Settings.cubeNumber;

        lockedChestsToggle.onValueChanged.AddListener(OnChestCheckboxValueChanged);
        lockedChestsToggle.isOn = level1Settings.isChestLocked;

        submitButton.onClick.AddListener(OnButtonClicked);
    }

    public void OnSliderChanged(System.Single value) {
        int cubeNumber = (int) value;
        itemNumberSlider.GetComponentInChildren<TextMeshProUGUI>().text = "" + cubeNumber;
        level1Settings.cubeNumber = cubeNumber;
    }

    public void OnChestCheckboxValueChanged(bool chestsLocked) {
        level1Settings.isChestLocked = chestsLocked;
    }

    public void OnButtonClicked() {
        level1Settings.SetSettingsToPrefs();
        onLevelSettingsApplied.Invoke();
    }
}
