using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using System;

public class SystemManager: MonoBehaviour, SettingsListener {
    public static SystemManager instance { get; private set; }
    public Gradient invalidGradient;
    public Gradient transparentGradient;

    private void Awake() { 
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => ApplySettings();
            DontDestroyOnLoad(this);
        } 
    }

    public void ApplySettings() {
        MainSettings settings = SettingsManager.GetMainSettings();
         
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("RayInteractor")) {
            XRRayInteractor interactor = obj.GetComponent<XRRayInteractor>();
            if(settings.distanceGrab) {
                interactor.raycastMask = ~0;
                interactor.GetComponent<XRInteractorLineVisual>().invalidColorGradient = invalidGradient;
            } else {
                interactor.raycastMask = LayerMask.GetMask("UI");
                interactor.GetComponent<XRInteractorLineVisual>().invalidColorGradient = transparentGradient;
            }
        }
    }

    void Start() {
        SettingsManager.AddListener(this);
        ApplySettings();
    }
}
