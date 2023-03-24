using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour {
    public GameObject itemNumberSlider;
    public Toggle lockedChestsToggle;
    public Toggle distanceGrabToggle;
    public Button submitButton;
    public UnityEvent<LevelConfig> onSubmit;

    private int itemsNumber = 5;
    private bool chestsLocked = false;
    private bool distanceGrab = false;

    void Start() {
        Slider slider = itemNumberSlider.GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
        slider.value = itemsNumber;

        lockedChestsToggle.onValueChanged.AddListener(OnChestCheckboxValueChanged);
        lockedChestsToggle.isOn = chestsLocked;

        distanceGrabToggle.onValueChanged.AddListener(OnRaysCheckboxValueChanged);
        distanceGrabToggle.isOn = distanceGrab;

        submitButton.onClick.AddListener(OnButtonClicked);
    }

    public void OnSliderChanged(System.Single value) {
        int cubeNumber = (int) value;
        itemNumberSlider.GetComponentInChildren<TextMeshProUGUI>().text = "Number of items: " + cubeNumber;
        this.itemsNumber = cubeNumber;
    }

    public void OnChestCheckboxValueChanged(bool chestsLocked) {
        this.chestsLocked = chestsLocked;
    }

    public void OnRaysCheckboxValueChanged(bool distanceGrab) {
        this.distanceGrab = distanceGrab;
    }

    public void OnButtonClicked() {
        LevelConfig config = new LevelConfig(itemsNumber, chestsLocked, distanceGrab);
        onSubmit.Invoke(config);
    }
}
