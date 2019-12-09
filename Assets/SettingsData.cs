using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsData
{
    public float brightness;
    public float contrast;
    public float volume;

    public SettingsData (SettingsManager settingsManager)
    {
        brightness = settingsManager._Brightness;
        contrast = settingsManager._Contrast;
        volume = settingsManager.audioMixer;
    }

}
