using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class Level1Settings : BaseSettings {
    public int cubeNumber;
    public bool isChestLocked;

    public Level1Settings(string _preset): base(_preset) {
    }

    override public void GetSettingsFromPrefs() {
        cubeNumber = PlayerPrefs.GetInt($"{preset}:l1:cubeNumber", 5);
        isChestLocked = GetBooleanFromPrefs("isChestLocked");
    }

    override public void SetSettingsToPrefs() {
        PlayerPrefs.SetInt($"{preset}:l1:cubeNumber", cubeNumber);
        SetBooleanToPrefs("isChestLocked", isChestLocked);
    }
}

public class LevelManager : MonoBehaviour {
    public static LevelManager instance { get; private set; }
    public GameObject spawnPointParent;
    public Transform spawnParent;
    public GameObject[] spawnObjects;

    public ChestInteractable[] chests;
    public GameObject[] keys;

    public TextMeshProUGUI[] countLabels;
    public Canvas progressCanvas;
    public Canvas winCanvas;
    public FadeScreen fadeScreen;

    public GameObject XROrigin;
    
    private Level1Settings settings;
    private Dictionary<string, int> generatedItemsTagsCount = new Dictionary<string, int>();
    private int completedContainers = 0;
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

    private void GenerateObjects() {
        foreach(Transform transform in spawnParent.transform) {
            Destroy(transform.gameObject);
        }

        generatedItemsTagsCount.Clear();

        List<Transform> spawnPoints = new List<Transform>();

        foreach(Transform point in spawnPointParent.transform) {
            spawnPoints.Add(point);
        }

        for(int i = 0; i < settings.cubeNumber; i++) {
            int objectIndex = Random.Range(0, spawnObjects.Length);

            GameObject objectToSpawn = spawnObjects[objectIndex];

            int objectCount = 0;
            if(generatedItemsTagsCount.TryGetValue(objectToSpawn.tag, out objectCount)) {
                generatedItemsTagsCount[objectToSpawn.tag] = objectCount + 1;
            } else {
                generatedItemsTagsCount[objectToSpawn.tag] = 1;
            }

        
            int spawnPointIndex = Random.Range(0, spawnPoints.Count);
            Vector3 position = spawnPoints[spawnPointIndex].position;
            spawnPoints.RemoveAt(spawnPointIndex);
        
            Instantiate(objectToSpawn, position, Quaternion.Euler(0, Random.Range(0, 360), 0), spawnParent);
        }
    }

    private TextMeshProUGUI GetLabelWithTag(string tag) {
        foreach(TextMeshProUGUI label in countLabels) {
            if(label.tag == tag) {
                return label;
            }
        }

        return null;
    }

    private void UpdateLabel(string tag, int count) {
        TextMeshProUGUI label = GetLabelWithTag(tag);
        label.text = "" + count;
    }

    private void CheckCompletedCondition() {
        if(completedContainers == chests.Length) {
            GetComponent<AudioSource>().Play();
            progressCanvas.gameObject.SetActive(false);
            winCanvas.gameObject.SetActive(true);
        }
    }

    private void ConfigureContainer(BasicContainer container) {
        string tag = container.itemCategoryTag;
        TextMeshProUGUI label = GetLabelWithTag(tag);
        int itemCount = generatedItemsTagsCount[tag];

        container.ResetContainer(itemCount);
        container.OnCorrectPlacement.AddListener((count) => {
            UpdateLabel(tag, count);
        });
        container.OnCorrectPlacementExit.AddListener((count) => {
            UpdateLabel(tag, count);
        });
        container.OnCompleted.AddListener(() => {
            completedContainers++;
            CheckCompletedCondition();
        });
        container.OnCompletedExit.AddListener(() => completedContainers--);

        UpdateLabel(tag, itemCount);
    }

    private void ConfigureChests() {
        foreach(ChestInteractable chest in chests) {
            chest.ResetChest(settings.isChestLocked);

            BasicContainer container = chest.GetComponentInChildren<BasicContainer>();
            ConfigureContainer(container);
        }

        foreach(GameObject key in keys) {
            key.SetActive(settings.isChestLocked);
        }
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

    private void StartLevel() {
        settings = (Level1Settings) SettingsManager.GetLevelSettings(0);

        GenerateObjects();
        ConfigureChests();

        completedContainers = 0;
        progressCanvas.gameObject.SetActive(true);
        winCanvas.gameObject.SetActive(false);
    }
}
