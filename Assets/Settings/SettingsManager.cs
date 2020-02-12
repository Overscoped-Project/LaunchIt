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
    public float _VolumeMaster;
    public float _VolumeMusic;
    public float _VolumeSFX;
    public Material material;
    public AudioMixer audioMixer;
    public Slider brightnessSlider;
    public Slider contrastSlider;
    public Slider volumeMasterSlider;
    public Slider volumeMusicSlider;
    public Slider volumeSFXSlider;

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

    public void SetVolumeMaster(float volume)
    {
        audioMixer.SetFloat("MASTER", Mathf.Log10(volume*2)*100);
    }

    public void SetVolumeMusic(float volume)
    {
        audioMixer.SetFloat("MUSIC", Mathf.Log10(volume*2) * 100);
    }
    public void SetVolumeSFX(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume*2) * 100);
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
            this._VolumeMaster = data._VolumeMaster;
            this._VolumeMusic = data._VolumeMusic;
            this._VolumeSFX = data._VolumeSFX;
            this.brightnessSlider.value = data.volumeBright;
            this.contrastSlider.value = data.volumeContr;
            this.volumeMasterSlider.value = data.volumeVolumeMaster;
            this.volumeMusicSlider.value = data.volumeVolumeMusic;
            this.volumeSFXSlider.value = data.volumeVolumeSFX;
        }
    }

    public void SaveSetting()
    {
        string key = "MainMenu";
        SaveData data = new SaveData();
        data._Brightness = this._Brightness;
        data._Contrast = this._Contrast;
        data._VolumeMaster = this._VolumeMaster;
        data._VolumeMusic = this._VolumeMusic;
        data._VolumeSFX = this._VolumeSFX;
        data.volumeBright = this.brightnessSlider.value;
        data.volumeContr = this.contrastSlider.value;
        data.volumeVolumeMaster = this.volumeMasterSlider.value;
        data.volumeVolumeMusic = this.volumeMusicSlider.value;
        data.volumeVolumeSFX = this.volumeSFXSlider.value;

        string value = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, value);
    }
}
