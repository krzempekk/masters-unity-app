using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour {
    public GameObject itemNumberSlider;
    public Toggle lockedChestsToggle;
    public Button submitButton;
    public UnityEvent<LevelConfig> onSubmit;
    private int itemsNumber = 5;
    private bool chestsLocked = false;

    void Start() {
        Slider slider = itemNumberSlider.GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
        slider.value = itemsNumber;

        lockedChestsToggle.onValueChanged.AddListener(OnChestCheckboxValueChanged);
        lockedChestsToggle.isOn = chestsLocked;

        submitButton.onClick.AddListener(OnButtonClicked);
    }

    public void OnSliderChanged(System.Single value) {
        int cubeNumber = (int) value;
        itemNumberSlider.GetComponentInChildren<TextMeshProUGUI>().text = "Number of cubes: " + cubeNumber;
        this.itemsNumber = cubeNumber;
    }

    public void OnChestCheckboxValueChanged(bool chestsLocked) {
        this.chestsLocked = chestsLocked;
    }

    public void OnButtonClicked() {
        LevelConfig config = new LevelConfig(itemsNumber, chestsLocked);
        onSubmit.Invoke(config);
    }
}
