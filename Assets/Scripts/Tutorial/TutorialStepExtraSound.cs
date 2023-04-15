using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TutorialStepExtraSound : TutorialStep {
    public AudioClip extraSound;

    override public IEnumerator Execute() {
        yield return base.Execute();
        audioSource.PlayOneShot(extraSound);
        yield return new WaitForSeconds(extraSound.length); 
    }
}
