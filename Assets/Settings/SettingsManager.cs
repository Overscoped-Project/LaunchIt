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
    private void Start()
    {
        LoadSettings();
    }
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

    public void LoadSettings()
    {
        string key = "MainMenu";

        if (PlayerPrefs.HasKey(key))
        {
            string value = PlayerPrefs.GetString(key);

            SaveData data = JsonUtility.FromJson<SaveData>(value);
            this._Brightness = data._Brightness;
            this._Contrast = data._Contrast;
            this._Volume = data._Volume;
            this.brightnessSlider = data.brightnessSlider;
            this.contrastSlider = data.contrastSlider;
            this.volumeSlider = data.volumeSlider;
        }
    }

    public void SaveSetting()
    {
        string key = "MainMenu";
        SaveData data = new SaveData();
        data._Brightness = this._Brightness;
        data._Contrast = this._Contrast;
        data._Volume = this._Volume;
        data.brightnessSlider = this.brightnessSlider;
        data.contrastSlider = this.contrastSlider;
        data.volumeSlider = this.volumeSlider;

        string value = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, value);
    }
}
