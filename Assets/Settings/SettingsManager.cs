using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[ExecuteInEditMode]
public class SettingsManager : MonoBehaviour
{
   

    [Range(0f, 2f)] public float _Brightness;
    [Range(0f, 2f)] public float _Contrast;
    public Material material;

    public AudioMixer audioMixer;
    public float _Volume;

    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_Brightness", _Brightness);
        material.SetFloat("_Contrast", _Contrast);
        Graphics.Blit(source, destination, material);
    }
    public void SetBrightness(float value)
    {
        _Brightness = value;
    }
    public void SetContrast(float value)
    {
        _Contrast = value;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
}
