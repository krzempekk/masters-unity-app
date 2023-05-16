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
    public WinCanvasController winCanvas;

    public Tutorial tutorial;

    private Level2Settings settings;
    private Vector3 initialOriginPos;

    public GameObject exitKnob;
    public LevelStats stats;

    private void Awake() { 
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
            initialOriginPos = XROrigin.transform.position;
        } 
    }

    private void Start() {
        StartLevel();
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

    private void SetupPlateStack(PlateStack plateStack, int plateCount) {
        plateStack.Reset(plateCount);
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

    private void GeneratePlates() {
        for(int i = 0; i < plateTags.Length; i++) {
            foreach(GameObject item in GameObject.FindGameObjectsWithTag(plateTags[i])) {
                Destroy(item);
            }
        }

        List<XRSocketInteractor> sockets = new List<XRSocketInteractor>(); 
        foreach(GameObject drainer in plateDrainers) {
            sockets.AddRange(drainer.GetComponentsInChildren<XRSocketInteractor>());
        }

        int itemsLeft = settings.platesNumber;
        int stacksLeft = plateStacks.Length;
        int socketIndex = 0;
        for(int i = 0; i < plateMeshes.Length; i++) {
            int plateCount = itemsLeft / stacksLeft;
            itemsLeft -= plateCount;
            stacksLeft--;
            SetupPlateStack(plateStacks[i], plateCount);
            for(int j = 0; j < plateCount; j++) {
                DishPlate newPlate = Instantiate(plate);
                newPlate.InitializePlate(plateMeshes[i], plateTags[i]);
                sockets[socketIndex].interactionManager.SelectEnter(
                    sockets[socketIndex], 
                    newPlate.GetComponent<IXRSelectInteractable>()
                );
                socketIndex++;
            }
        }
    }

    private void SetupCupGrid(DishGrid cupGrid, int cupCount) {
        cupGrid.Reset(cupCount);
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

    private void GenerateCups() {
        for(int i = 0; i < cupTags.Length; i++) {
            foreach(GameObject item in GameObject.FindGameObjectsWithTag(cupTags[i])) {
                Destroy(item);
            }
        }

        List<XRSocketInteractor> sockets = new List<XRSocketInteractor>(); 
        foreach(GameObject drainer in cupDrainers) {
            sockets.AddRange(drainer.GetComponentsInChildren<XRSocketInteractor>());
        }

        int itemsLeft = settings.cupsNumber;
        int gridsLeft = cupGrids.Length;
        int socketIndex = 0;
        for(int i = 0; i < cupMeshes.Length; i++) {
            int cupCount = itemsLeft / gridsLeft;
            itemsLeft -= cupCount;
            gridsLeft--;
            SetupCupGrid(cupGrids[i], cupCount);
            for(int j = 0; j < cupCount; j++) {
                DishCup newCup = Instantiate(cup);
                newCup.InitializeCup(cupMeshes[i], cupTags[i]);
                sockets[socketIndex].interactionManager.SelectEnter(
                    sockets[socketIndex], 
                    newCup.GetComponent<IXRSelectInteractable>()
                );
                socketIndex++;
            }
        }
    }

    private void SetupCutleryGrid(DishGrid cutleryGrid, int cutleryCount) {
        cutleryGrid.Reset(cutleryCount);
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

    private void GenerateCutlery() {
        for(int i = 0; i < cutleryObjects.Length; i++) {
            foreach(GameObject item in GameObject.FindGameObjectsWithTag(cutleryObjects[i].tag)) {
                Destroy(item);
            }
        }


        List<XRSocketInteractor> sockets = new List<XRSocketInteractor>(); 
        foreach(GameObject drainer in cutleryDrainers) {
            sockets.AddRange(drainer.GetComponentsInChildren<XRSocketInteractor>());
        }    

        int itemsLeft = settings.cutleryNumber;
        int gridsLeft = cutleryGrids.Length;
        int socketIndex = 0;
        for(int i = 0; i < cutleryObjects.Length; i++) {
            int cutleryCount = itemsLeft / gridsLeft;
            itemsLeft -= cutleryCount;
            gridsLeft--;
            SetupCutleryGrid(cutleryGrids[i], cutleryCount);
            for(int j = 0; j < cutleryCount; j++) {
                GameObject newCutlery = Instantiate(cutleryObjects[i]);
                sockets[socketIndex].interactionManager.SelectEnter(
                    sockets[socketIndex], 
                    newCutlery.GetComponent<IXRSelectInteractable>()
                );
                socketIndex++;
            }
        }
    }

    private void StartLevel() {
        settings = (Level2Settings) SettingsManager.GetLevelSettings(1);

        exitKnob.SetActive(false);

        platesLeft = settings.platesNumber;
        completedPlateStacks = 0;
        cupsLeft = settings.cupsNumber;
        completedCupGrids = 0;
        cutleryLeft = settings.cutleryNumber;
        completedCutleryGrids = 0;

        GeneratePlates();

        GenerateCups();

        GenerateCutlery();

        progressCanvas.gameObject.SetActive(true);
        winCanvas.gameObject.SetActive(false);

        if(!settings.disableTutorial) {
            tutorial.PlayTutorial();
        }

        stats.StartLevel();
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
        stats.EndLevel();
        exitKnob.SetActive(true);

        progressCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(true);
        winCanvas.ShowResults();
    }

    public void ResetPosition() {
        XROrigin.transform.position = initialOriginPos;
    }
}
