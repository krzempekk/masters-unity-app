using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Level2Settings : BaseSettings {
    public int platesNumber;

    public Level2Settings(string _preset): base(_preset) {
    }

    override public void GetSettingsFromPrefs() {
        platesNumber = PlayerPrefs.GetInt($"{preset}:l2:platesNumber", 5);
    }

    override public void SetSettingsToPrefs() {
        PlayerPrefs.SetInt($"{preset}:l2:platesNumber", platesNumber);
    }
}


public class Level2Manager : MonoBehaviour {
    public static Level2Manager instance { get; private set; }
    
    public XRBaseInteractable plate;
    public GameObject[] plateDrainers;
    public FadeScreen fadeScreen;
    public GameObject XROrigin;

    private Level2Settings settings;
    private Vector3 initialOriginPos;

    private void Awake() { 
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
            initialOriginPos = XROrigin.transform.position;
            StartLevel();
        } 
    }

    void Start() {
    }

    void Update() {
        
    }

    private void GeneratePlates() {
        List<XRSocketInteractor> sockets = new List<XRSocketInteractor>(); 
        foreach(GameObject drainer in plateDrainers) {
            sockets.AddRange(drainer.GetComponentsInChildren<XRSocketInteractor>());
        }
        for(int i = 0; i < settings.platesNumber; i++) {
            XRBaseInteractable newPlate = Instantiate(plate);
            sockets[i].interactionManager.SelectEnter(sockets[i], (IXRSelectInteractable) newPlate);
        }
    }

    private void StartLevel() {
        settings = (Level2Settings) SettingsManager.GetLevelSettings(1);

        GeneratePlates();
    }

    public void RestartLevel() {
        StartCoroutine(RestartLevelRoutine());
    }

    private IEnumerator RestartLevelRoutine() {
        fadeScreen.FadeOutAndIn();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        XROrigin.transform.position = initialOriginPos;
        StartLevel();
    }
}
