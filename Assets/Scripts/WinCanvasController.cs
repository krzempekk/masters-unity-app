using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinCanvasController : MonoBehaviour {
    public MeshRenderer[] stars;
    public Material activeMaterial;
    public Material inactiveMaterial;
    public AudioSource audioSource;
    public AudioClip sound;
    public TextMeshProUGUI levelTime;

    public IEnumerator ShowResultsRoutine() {
        foreach(MeshRenderer star in stars) {
            star.material = inactiveMaterial;
        }

        Level1Stats stats = Level1Manager.instance.stats;

        levelTime.text = stats.getTimeElapsed().ToString(@"mm\:ss");

        int starsNumber = stats.GetStarsNumber();
        for(int i = 0; i < starsNumber; i++) {
            yield return new WaitForSeconds(1.0f);
            audioSource.PlayOneShot(sound);
            stars[i].material = activeMaterial;
        }
    }

    public void ShowResults() {
        StartCoroutine(ShowResultsRoutine());
    }
}
