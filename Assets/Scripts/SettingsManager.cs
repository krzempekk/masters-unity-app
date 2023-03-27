using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseSettings {  
    public string preset;

    public BaseSettings(string _preset) {
        preset = _preset;
    }

    protected bool GetBooleanFromPrefs(string property) {
        return Convert.ToBoolean(PlayerPrefs.GetInt($"{preset}:{property}", 0));
    }

    protected void SetBooleanToPrefs(string property, bool value) {
        PlayerPrefs.SetInt($"{preset}:{property}", Convert.ToInt32(value));
    }
  
    abstract public void GetSettingsFromPrefs();
    abstract public void SetSettingsToPrefs();
}

public class MainSettings: BaseSettings {
    public MainSettings(string _preset): base(_preset) {
    }

    public bool distanceGrab;

    override public void GetSettingsFromPrefs() {
        distanceGrab = GetBooleanFromPrefs("distanceGrab");
    }

    override public void SetSettingsToPrefs() {
        SetBooleanToPrefs("distanceGrab", distanceGrab);
    }
}

public interface SettingsListener {
    void ApplySettings();
}

public class SettingsManager: MonoBehaviour {
    public static SettingsManager instance { get; private set; }
    public string preset = "default";
    private MainSettings mainSettings;
    private List<BaseSettings> levelSettings = new List<BaseSettings>();
    private List<SettingsListener> listeners = new List<SettingsListener>();

    private void Awake() { 
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
            InitializeSettings();
            DontDestroyOnLoad(this);
        } 
    }

    private void InitializeSettings() {
        mainSettings = new MainSettings(preset);
        levelSettings.Add(new Level1Settings(preset));

        LoadSettings();
    }

    public static void AddListener(SettingsListener listener) {
        instance.listeners.Add(listener);
    }

    public static MainSettings GetMainSettings() {
        return instance.mainSettings;
    }

    public static BaseSettings GetLevelSettings(int levelIndex) {
        return instance.levelSettings[levelIndex];
    }

    public static void LoadSettings() {
        instance.mainSettings.GetSettingsFromPrefs();
        foreach(BaseSettings _levelSettings in instance.levelSettings) {
            _levelSettings.GetSettingsFromPrefs();
        }
    }

    public static void SaveSettings() {
        instance.mainSettings.SetSettingsToPrefs();
        foreach(BaseSettings _levelSettings in instance.levelSettings) {
            _levelSettings.SetSettingsToPrefs();
        }

        foreach(SettingsListener listener in instance.listeners) {
            listener.ApplySettings();
        }
    }
}
