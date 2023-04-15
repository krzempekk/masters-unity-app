using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TutorialStep : MonoBehaviour {
    public AudioClip audioClip;
    public AudioSource audioSource;

    virtual public bool ShouldExecute() {
        return true;
    }

    virtual public IEnumerator Execute() {
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length); 
    }
}
