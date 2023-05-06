using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class Level2Settings : BaseSettings {
    public int platesNumber;
    public int cupsNumber;
    public int cutleryNumber;
    public bool disableTutorial;

    public Level2Settings(string _preset): base(_preset) {
    }

    override public void GetSettingsFromPrefs() {
        platesNumber = PlayerPrefs.GetInt($"{preset}:l2:platesNumber", 5);
        cupsNumber = PlayerPrefs.GetInt($"{preset}:l2:cupsNumber", 5);
        cutleryNumber = PlayerPrefs.GetInt($"{preset}:l2:cutleryNumber", 5);
        disableTutorial = GetBooleanFromPrefs("l2:disableTutorial");
    }

    override public void SetSettingsToPrefs() {
        PlayerPrefs.SetInt($"{preset}:l2:platesNumber", platesNumber);
        PlayerPrefs.SetInt($"{preset}:l2:cupsNumber", cupsNumber);
        PlayerPrefs.SetInt($"{preset}:l2:cutleryNumber", cutleryNumber);
        SetBooleanToPrefs("l2:disableTutorial", disableTutorial);
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
    public TextMeshProUGUI plateProgressLabel;
    private int platesLeft;
    private int completedPlateStacks;

    public DishCup cup;
    public GameObject[] cupDrainers;
    public DishGrid[] cupGrids;
    public Mesh[] cupMeshes;
    public string[] cupTags;
    public TextMeshProUGUI cupProgressLabel;
    private int cupsLeft;
    private int completedCupGrids;

    public GameObject[] cutleryDrainers;
    public DishGrid[] cutleryGrids;
    public GameObject[] cutleryObjects;
    public TextMeshProUGUI cutleryProgressLabel;
    private int cutleryLeft;
    private int completedCutleryGrids;


    public Canvas progressCanvas;
    public Canvas winCanvas;

    public Tutorial tutorial;

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
        if(
            plateStacks.Length == completedPlateStacks && 
            cupGrids.Length == completedCupGrids &&
            cutleryGrids.Length == completedCutleryGrids
        ) {
            EndLevel();
        }
    }

    private void SetupPlateStacks() {
        foreach(PlateStack plateStack in plateStacks) {
            plateStack.Reset();
            plateProgressLabel.text = "" + platesLeft;
            plateStack.OnPlacement.AddListener(() => {
                platesLeft--;
                plateProgressLabel.text = "" + platesLeft;
            });
            plateStack.OnPlacementExit.AddListener(() => {
                platesLeft++;
                plateProgressLabel.text = "" + platesLeft;
            });
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

    private void SetupCupGrids() {
        foreach(DishGrid cupGrid in cupGrids) {
            cupGrid.Reset();
            cupProgressLabel.text = "" + cupsLeft;
            cupGrid.OnPlacement.AddListener(() => {
                cupsLeft--;
                cupProgressLabel.text = "" + cupsLeft;
            });
            cupGrid.OnPlacementExit.AddListener(() => {
                cupsLeft++;
                cupProgressLabel.text = "" + cupsLeft;
            });
            cupGrid.OnCompleted.AddListener(() => {
                completedCupGrids++;
                CheckCompletedCondition();
            });
            cupGrid.OnCompletedExit.AddListener(() => {
                completedCupGrids--;
            });
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

        for(int i = 0; i < cupMeshes.Length; i++) {
            cupGrids[i].targetCount = cupCounts[i];
        }

    }

    private void SetupCutleryGrids() {
        foreach(DishGrid cutleryGrid in cutleryGrids) {
            cutleryGrid.Reset();
            cutleryProgressLabel.text = "" + cutleryLeft;
            cutleryGrid.OnPlacement.AddListener(() => {
                cutleryLeft--;
                cutleryProgressLabel.text = "" + cutleryLeft;
            });
            cutleryGrid.OnPlacementExit.AddListener(() => {
                cutleryLeft++;
                cutleryProgressLabel.text = "" + cutleryLeft;
            });
            cutleryGrid.OnCompleted.AddListener(() => {
                completedCutleryGrids++;
                CheckCompletedCondition();
            });
            cutleryGrid.OnCompletedExit.AddListener(() => {
                completedCutleryGrids--;
            });
        }    
    }

    private void GenerateCutlery() {
        List<XRSocketInteractor> sockets = new List<XRSocketInteractor>(); 
        foreach(GameObject drainer in cutleryDrainers) {
            sockets.AddRange(drainer.GetComponentsInChildren<XRSocketInteractor>());
        }    

        int[] cutleryCounts = new int[cutleryObjects.Length];
        for(int i = 0; i < settings.cutleryNumber; i++) {
            int cutleryIndex = Random.Range(0, cutleryObjects.Length);

            GameObject newCutlery = Instantiate(cutleryObjects[cutleryIndex]);

            cutleryCounts[cutleryIndex]++;

            sockets[i].interactionManager.SelectEnter(sockets[i], newCutlery.GetComponent<IXRSelectInteractable>());
        }

        for(int i = 0; i < cutleryGrids.Length; i++) {
            cutleryGrids[i].targetCount = cutleryCounts[i];
        }
    }

    private void StartLevel() {
        settings = (Level2Settings) SettingsManager.GetLevelSettings(1);

        platesLeft = settings.platesNumber;
        completedPlateStacks = 0;
        cupsLeft = settings.cupsNumber;
        completedCupGrids = 0;
        cutleryLeft = settings.cutleryNumber;
        completedCutleryGrids = 0;

        SetupPlateStacks();
        GeneratePlates();

        SetupCupGrids();
        GenerateCups();

        SetupCutleryGrids();
        GenerateCutlery();

        progressCanvas.gameObject.SetActive(true);
        winCanvas.gameObject.SetActive(false);
    }

    public void RestartLevel() {
        StartCoroutine(RestartLevelRoutine());
    }

    private IEnumerator RestartLevelRoutine() {
        fadeScreen.FadeOutAndIn();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        XROrigin.transform.position = initialOriginPos;
        StartLevel();

        if(!settings.disableTutorial) {
            tutorial.PlayTutorial();
        }
    }

    private void EndLevel() {
        progressCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(true);
    }
}
