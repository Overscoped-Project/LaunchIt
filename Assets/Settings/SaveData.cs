using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveData
{
    [Range(0f, 2f)] public float _Brightness;
    [Range(0f, 2f)] public float _Contrast;
    public float _Volume;
    public Slider contrastSlider;
    public Slider volumeSlider;
    public float volumeBright;
    public float volumeContr;
    public float volumeVolume;
}
