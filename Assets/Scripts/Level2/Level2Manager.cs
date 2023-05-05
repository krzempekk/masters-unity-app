using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Level2Settings : BaseSettings {
    public int platesNumber;
    public int cupsNumber;

    public Level2Settings(string _preset): base(_preset) {
    }

    override public void GetSettingsFromPrefs() {
        platesNumber = PlayerPrefs.GetInt($"{preset}:l2:platesNumber", 5);
        cupsNumber = PlayerPrefs.GetInt($"{preset}:l2:cupsNumber", 5);
    }

    override public void SetSettingsToPrefs() {
        PlayerPrefs.SetInt($"{preset}:l2:platesNumber", platesNumber);
        PlayerPrefs.SetInt($"{preset}:l2:cupsNumber", cupsNumber);
    }
}


public class Level2Manager : MonoBehaviour {
    public static Level2Manager instance { get; private set; }
    
    public FadeScreen fadeScreen;
    public GameObject XROrigin;

    public DishPlate plate;
    public GameObject[] plateDrainers;
    public PlateStack[] plateStacks;
    public Mesh[] plateMeshes;
    public string[] plateTags;

    public DishCup cup;
    public GameObject[] cupDrainers;
    public Mesh[] cupMeshes;
    public string[] cupTags;

    
    private int completedPlateStacks;

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

    private void CheckCompletedCondition() {
        if(plateStacks.Length == completedPlateStacks) {
            EndLevel();
        }
    }

    private void SetupPlateStacks() {
        foreach(PlateStack plateStack in plateStacks) {
            plateStack.Reset();
            plateStack.OnCompleted.AddListener(() => {
                completedPlateStacks++;
                CheckCompletedCondition();
            });
            plateStack.OnCompletedExit.AddListener(() => {
                completedPlateStacks--;
            });
        }
    }

    private void GeneratePlates() {
        List<XRSocketInteractor> sockets = new List<XRSocketInteractor>(); 
        foreach(GameObject drainer in plateDrainers) {
            sockets.AddRange(drainer.GetComponentsInChildren<XRSocketInteractor>());
        }

        int[] plateCounts = new int[plateMeshes.Length];
        for(int i = 0; i < settings.platesNumber; i++) {
            DishPlate newPlate = Instantiate(plate);

            int colorIndex = Random.Range(0, plateMeshes.Length);
            newPlate.InitializePlate(plateMeshes[colorIndex], plateTags[colorIndex]);
            plateCounts[colorIndex]++;

            sockets[i].interactionManager.SelectEnter(sockets[i], newPlate.GetComponent<IXRSelectInteractable>());
        }

        for(int i = 0; i < plateMeshes.Length; i++) {
            plateStacks[i].targetHeight = plateCounts[i];
        }
    }

    private void GenerateCups() {
        List<XRSocketInteractor> sockets = new List<XRSocketInteractor>(); 
        foreach(GameObject drainer in cupDrainers) {
            sockets.AddRange(drainer.GetComponentsInChildren<XRSocketInteractor>());
        }

        int[] cupCounts = new int[cupMeshes.Length];
        for(int i = 0; i < settings.cupsNumber; i++) {
            DishCup newCup = Instantiate(cup);

            int colorIndex = Random.Range(0, cupMeshes.Length);
            newCup.InitializeCup(cupMeshes[colorIndex], cupTags[colorIndex]);
            cupCounts[colorIndex]++;

            sockets[i].interactionManager.SelectEnter(sockets[i], newCup.GetComponent<IXRSelectInteractable>());
        }
    }

    private void StartLevel() {
        completedPlateStacks = 0;
        settings = (Level2Settings) SettingsManager.GetLevelSettings(1);

        SetupPlateStacks();
        GeneratePlates();

        GenerateCups();
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

    private void EndLevel() {
        Debug.Log("End level");
    }
}
