using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SettingsManager : MonoBehaviour
{


    [Range(0f, 2f)] public float _Brightness;
    [Range(0f, 2f)] public float _Contrast;
    public float _Volume;
    public Material material;
    public AudioMixer audioMixer;
    public Slider brightnessSlider;
    public Slider contrastSlider;
    public Slider volumeSlider;
    [Range(0f, 2f)] public float brightnessVolume;
    [Range(0f, 2f)] public float contrastVolume;
    [Range(-80f, 0f)] public float volumeVolume;

    private void Start()
    {
        brightnessVolume = PlayerPrefs.GetFloat(gameObject.name + "brightnessVolume");
        LoadSettings();
    }

    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_Brightness", _Brightness);
        material.SetFloat("_Contrast", _Contrast);
        Graphics.Blit(source, destination, material);
    }

    /*private void Update()
    {
        brightnessVolume = brightnessSlider.value;
        contrastVolume = contrastSlider.value;
        volumeVolume = volumeSlider.value;
    }*/

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

    public void LoadSettings()
    {
        string key = "MainMenu";

        if (PlayerPrefs.HasKey(key))
        {
            string value = PlayerPrefs.GetString(key);

            SaveData data = JsonUtility.FromJson<SaveData>(value);
            Debug.Log(value);
            this._Brightness = data._Brightness;
            this._Contrast = data._Contrast;
            this._Volume = data._Volume;
            this.brightnessVolume = data.brightnessVolume;
            this.contrastVolume = data.contrastVolume;
            this.volumeVolume = data.volumeVolume;
        }
    }

    public void SaveSetting()
    {
        string key = "MainMenu";
        SaveData data = new SaveData();
        data._Brightness = this._Brightness;
        data._Contrast = this._Contrast;
        data._Volume = this._Volume;
        data.brightnessVolume = this.brightnessVolume;
        data.contrastVolume = this.contrastVolume;
        data.volumeVolume = this.volumeVolume;

        string value = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, value);
    }
}
