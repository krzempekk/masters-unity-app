using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Level1CanvasController : MonoBehaviour {
    public Button submitButton;
    public UnityEvent onLevelSettingsApplied;

    private Level1Settings level1Settings;

    void Start() {
        level1Settings = (Level1Settings) SettingsManager.GetLevelSettings(0);
        submitButton.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked() {
        level1Settings.SetSettingsToPrefs();
        onLevelSettingsApplied.Invoke();
    }
}
