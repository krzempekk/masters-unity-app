using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class Level1Settings : BaseSettings {
    public int cubeNumber;
    public bool isChestLocked;
    public bool extraItems;
    public bool disableTutorial;

    public Level1Settings(string _preset): base(_preset) {
    }

    override public void GetSettingsFromPrefs() {
        cubeNumber = PlayerPrefs.GetInt($"{preset}:l1:cubeNumber", 5);
        isChestLocked = GetBooleanFromPrefs("l1:isChestLocked");
        extraItems = GetBooleanFromPrefs("l1:extraItems");
        disableTutorial = GetBooleanFromPrefs("l1:disableTutorial");
    }

    override public void SetSettingsToPrefs() {
        PlayerPrefs.SetInt($"{preset}:l1:cubeNumber", cubeNumber);
        SetBooleanToPrefs("l1:isChestLocked", isChestLocked);
        SetBooleanToPrefs("l1:extraItems", extraItems);
        SetBooleanToPrefs("l1:disableTutorial", disableTutorial);
    }
}

public class Level1Manager : MonoBehaviour {
    public static Level1Manager instance { get; private set; }
    public GameObject spawnPointParent;
    public Transform spawnParent;

    public ChestInteractable[] chests;
    public GameObject[] progressIndicators;

    public Canvas progressCanvas;
    public WinCanvasController winCanvas;
    public FadeScreen fadeScreen;

    public GameObject XROrigin;
    public ItemCategory[] itemCategories;

    public Tutorial tutorial;
    public LevelStats stats;

    public GameObject exitKnob;

    private List<ItemCategory> activeCategories = new List<ItemCategory>();
    private Level1Settings settings;
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

    private void SpawnObject(List<Transform> spawnPoints, ItemCategory category) {
        GameObject objectToSpawn = category.getItem();

        int spawnPointIndex = Random.Range(0, spawnPoints.Count);
        Vector3 position = spawnPoints[spawnPointIndex].position;
        spawnPoints.RemoveAt(spawnPointIndex);
    
        Instantiate(objectToSpawn, position, Quaternion.Euler(0, Random.Range(0, 360), 0), spawnParent);
    }

    private void GenerateObjects() {
        foreach(Transform transform in spawnParent.transform) {
            Destroy(transform.gameObject);
        }

        int itemsLeft = settings.cubeNumber;

        activeCategories.Clear();

        for(int i = 0; i < chests.Length; i++) {
            ItemCategory category = itemCategories[Random.Range(0, itemCategories.Length)];
            if(!activeCategories.Contains(category)) {
                int chestsLeft = chests.Length - i;

                chests[i].itemCategory = category;
                chests[i].sticker.material = category.picture;

                category.count = itemsLeft / chestsLeft;
                itemsLeft -= category.count;

                category.progressIndicator = progressIndicators[i];
                progressIndicators[i].GetComponentInChildren<Image>().material = category.picture;

                activeCategories.Add(category);
            } else {
                i--;
            }
        }

        List<Transform> spawnPoints = new List<Transform>();

        foreach(Transform point in spawnPointParent.transform) {
            spawnPoints.Add(point);
        }

        foreach(ItemCategory category in activeCategories) {
            for(int i = 0; i < category.count; i++) {
                SpawnObject(spawnPoints, category);
            }
        }

        if(settings.extraItems) {
            for(int i = 0; i < settings.cubeNumber / 2; i++) {
                ItemCategory category = itemCategories[Random.Range(0, itemCategories.Length)];
                if(!activeCategories.Contains(category)) {
                    SpawnObject(spawnPoints, category);
                } else {
                    i--;
                }
            }
        }

    }

    private void CheckCompletedCondition() {
        if(completedContainers == chests.Length) {
            EndLevel();
        }
    }

    private void ConfigureContainer(ChestInteractable chest) {
        BasicContainer container = chest.container;
        ItemCategory itemCategory = chest.itemCategory;
        int itemCount = itemCategory.count;
        container.itemCategoryTag = itemCategory.itemTag;

        container.ResetContainer(itemCount);
        container.OnCorrectPlacement.AddListener((count) => {
            itemCategory.UpdateIndicator(count);
        });
        container.OnCorrectPlacementExit.AddListener((count) => {
            itemCategory.UpdateIndicator(count);
        });
        container.OnCompleted.AddListener(() => {
            completedContainers++;
            CheckCompletedCondition();
        });
        container.OnCompletedExit.AddListener(() => completedContainers--);

        itemCategory.UpdateIndicator(itemCount);
    }

    private void ConfigureChests() {
        foreach(ChestInteractable chest in chests) {
            chest.ResetChest(settings.isChestLocked);
            chest.key.SetActive(settings.isChestLocked);

            ConfigureContainer(chest);
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
        SettingsManager.LoadSettings();
        SettingsManager.ApplySettings();
        settings = (Level1Settings) SettingsManager.GetLevelSettings(0);

        exitKnob.SetActive(false);

        GenerateObjects();
        ConfigureChests();

        completedContainers = 0;
        progressCanvas.gameObject.SetActive(true);
        winCanvas.gameObject.SetActive(false);

        if(!settings.disableTutorial) {
            tutorial.PlayTutorial();
        }

        stats.StartLevel();
    }

    private void EndLevel() {
        stats.EndLevel();

        exitKnob.SetActive(true);

        GetComponent<AudioSource>().Play();

        progressCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(true);
        winCanvas.ShowResults();
    }
}
