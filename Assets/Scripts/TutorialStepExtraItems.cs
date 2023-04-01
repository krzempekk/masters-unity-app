using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TutorialStepExtraItems : TutorialStep {
    override public bool ShouldExecute() {
        return ((Level1Settings) SettingsManager.GetLevelSettings(0)).extraItems;
    }
}
