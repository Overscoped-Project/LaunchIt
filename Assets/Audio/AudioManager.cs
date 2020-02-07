using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup audioMixerGroup;
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = audioMixerGroup;
        }
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s  = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
        } else
        {
            Debug.LogWarning("Sound" + name +"not found");
        }
    }

    // Play the sound only if its not already playing
    public void PlayIfNot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            if (!s.source.isPlaying)
            {
                s.source.Play();
            }
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found");
        }
    }
}
