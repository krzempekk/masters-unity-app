using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Level2CanvasController : MonoBehaviour {
    public Button submitButton;
    public UnityEvent onLevelSettingsApplied;

    private Level2Settings level2Settings;

    void Start() {
        level2Settings = (Level2Settings) SettingsManager.GetLevelSettings(1);
        submitButton.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked() {
        level2Settings.SetSettingsToPrefs();
        onLevelSettingsApplied.Invoke();
    }
}
