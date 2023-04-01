using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCategory: MonoBehaviour {    
    public Material picture;
    public GameObject[] items;

    public GameObject progressIndicator;
    public string itemTag;
    public int count;

    public GameObject getItem() {
        GameObject item = items[Random.Range(0, items.Length)];
        item.tag = itemTag;
        return item;
    }

    public void UpdateIndicator(int currentCount) {
        progressIndicator.GetComponentInChildren<TextMeshProUGUI>().text = "" + currentCount;
    }
}
