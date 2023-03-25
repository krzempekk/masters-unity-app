using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class LevelConfig {
    public int cubeNumber;
    public bool isChestLocked;
    public bool distanceGrab;

    public LevelConfig(int cubeNumber, bool isChestLocked, bool distanceGrab) {
        this.cubeNumber = cubeNumber;
        this.isChestLocked = isChestLocked;
        this.distanceGrab = distanceGrab;
    }
}

public class LevelManager : MonoBehaviour {
    public static LevelManager instance { get; private set; }
    public GameObject spawnPointParent;
    public Transform spawnParent;
    public GameObject[] spawnObjects;
    public BasicContainer[] containers;
    public ChestInteractable[] chests;
    public GameObject[] keys;
    public XRRayInteractor[] rayInteractors;
    
    private LevelConfig config;
    private Dictionary<string, int> generatedItemsTagsCount = new Dictionary<string, int>();
    
    public TextMeshProUGUI[] countLabels;
    
    public Gradient invalidGradient;
    public Gradient transparentGradient;


    private void Awake() { 
        if (instance != null && instance != this) { 
            Destroy(this); 
        } else { 
            instance = this; 
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

        for(int i = 0; i < config.cubeNumber; i++) {
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

    private void ConfigureContainers() {
        foreach(BasicContainer container in containers) {
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
            UpdateLabel(tag, itemCount);
        }
    }

    private void UpdateLabel(string tag, int count) {
        TextMeshProUGUI label = GetLabelWithTag(tag);
        label.text = "" + count;
    }

    private void ConfigureChests() {
        foreach(ChestInteractable chest in chests) {
            chest.ResetChest(config.isChestLocked);
        }

        foreach(GameObject key in keys) {
            key.SetActive(config.isChestLocked);
        }
    }

    private void ConfigureRayInteractors() {
        foreach(XRRayInteractor interactor in rayInteractors) {
            if(config.distanceGrab) {
                interactor.raycastMask = ~0;
                interactor.GetComponent<XRInteractorLineVisual>().invalidColorGradient = invalidGradient;
            } else {
                interactor.raycastMask = LayerMask.GetMask("UI");
                interactor.GetComponent<XRInteractorLineVisual>().invalidColorGradient = transparentGradient;
            }
            Debug.Log(interactor.raycastMask);
        }
    }

    public void RestartLevel(LevelConfig newConfig) {
        if(newConfig != null) {
            config = newConfig;
        }

        GenerateObjects();
        ConfigureContainers();
        ConfigureChests();
        ConfigureRayInteractors();
    }
}
