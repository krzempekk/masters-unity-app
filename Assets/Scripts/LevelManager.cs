using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig {
    public int cubeNumber;
    public bool isChestLocked;

    public LevelConfig(int cubeNumber, bool isChestLocked) {
        this.cubeNumber = cubeNumber;
        this.isChestLocked = isChestLocked;
    }
}

public class LevelManager : MonoBehaviour {
    public static LevelManager instance { get; private set; }
    public GameObject spawnArea;
    public Transform spawnParent;
    public GameObject[] spawnObjects;
    public BasicContainer[] containers;
    public ChestInteractable[] chests;
    public GameObject[] keys;
    
    private LevelConfig config;
    private Dictionary<string, int> generatedItemsTagsCount = new Dictionary<string, int>();


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

        Bounds bounds = spawnArea.GetComponent<MeshFilter>().mesh.bounds;

        for(int i = 0; i < config.cubeNumber; i++) {
            int objectIndex = Random.Range(0, spawnObjects.Length);

            GameObject objectToSpawn = spawnObjects[objectIndex];

            int objectCount = 0;
            if(generatedItemsTagsCount.TryGetValue(objectToSpawn.tag, out objectCount)) {
                generatedItemsTagsCount[objectToSpawn.tag] = objectCount + 1;
            } else {
                generatedItemsTagsCount[objectToSpawn.tag] = 1;
            }

            Vector3 positionOnArea = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                0,
                Random.Range(bounds.min.z, bounds.max.z)
            );

            Vector3 position = spawnArea.GetComponent<Transform>().TransformPoint(positionOnArea);
        
            Instantiate(objectToSpawn, position, Quaternion.Euler(0, 0, 0), spawnParent);
        }
    }

    private void ConfigureContainers() {
        foreach(BasicContainer container in containers) {
            container.ResetContainer(generatedItemsTagsCount[container.itemCategoryTag]);
        }
    }

    private void ConfigureChests() {
        foreach(ChestInteractable chest in chests) {
            chest.ResetChest(config.isChestLocked);
        }

        foreach(GameObject key in keys) {
            key.SetActive(config.isChestLocked);
        }
    }

    public void RestartLevel(LevelConfig newConfig) {
        if(newConfig != null) {
            config = newConfig;
        }

        GenerateObjects();
        ConfigureContainers();
        ConfigureChests();
    }
}
