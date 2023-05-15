using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour {
    public int correctPlacements;
    public int incorrectPlacements;
    private DateTime timeStart;
    private TimeSpan timeElapsed;

    public void RegisterCorrectPlacement() {
        correctPlacements++;
    }

    public void RegisterIncorrectPlacement() {
        incorrectPlacements++;
    }

    public void StartLevel() {
        timeStart = DateTime.Now;
        correctPlacements = 0;
        incorrectPlacements = 0;
    }

    public void EndLevel() {
        timeElapsed = DateTime.Now - timeStart;
    }

    public TimeSpan getTimeElapsed() {
        return timeElapsed;
    }

    public int GetStarsNumber() {
        if(incorrectPlacements == 0) {
            return 3;
        }

        if(incorrectPlacements <= 0.2 * correctPlacements) {
            return 2;
        }

        return 1;
    }
}
