using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen: MonoBehaviour {
    public bool fadeOnStart = true;
    public float fadeDuration = 2;
    public Color fadeColor;
    private Renderer rend;

    void Start() {
        rend = GetComponent<Renderer>();
        if(fadeOnStart) {
            FadeIn();
        }
    }

    public void FadeIn() {
        Fade(1, 0);
    }

    public void FadeOut() {
        Fade(0, 1);
    }

    public void FadeOutAndIn() {
        StartCoroutine((new[] {
            FadeRoutine(0, 1),
            FadeRoutine(1, 0)
        }).GetEnumerator());
    }

    private void Fade(float alphaIn, float alphaOut) {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    private void UpdateColor(float alphaIn, float alphaOut, float timer) {
        Color newColor = fadeColor;
        newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

        rend.material.SetColor("_BaseColor", newColor);
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut) {
        float timer = 0;
        while(timer <= fadeDuration) {
            UpdateColor(alphaIn, alphaOut, timer);
            timer += Time.deltaTime;
            yield return null;
        }

        UpdateColor(alphaIn, alphaOut, fadeDuration);
    }
}
