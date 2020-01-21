using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveData
{
    [Range(0f, 2f)] public float _Brightness;
    [Range(0f, 2f)] public float _Contrast;
    public float _Volume;
    [Range(0f, 2f)] public float brightnessVolume;
    [Range(0f, 2f)] public float contrastVolume;
    [Range(-80f, 0f)] public float volumeVolume;
}
