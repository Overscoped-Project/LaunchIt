using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private Sound[] musik;
    public AudioMixerGroup audioMixerGroup;

    private AudioSource source;

    private List<Alien> aliensAround = new List<Alien>();

    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = audioMixerGroup;
        source.clip = musik[0].clip;
        source.volume = musik[0].volume;
        source.pitch = musik[0].pitch;
        source.playOnAwake = true;
        source.loop = true;
        source.Play();

    }

    public void AddAlien(Alien alien)
    {
        if (!aliensAround.Contains(alien)) aliensAround.Add(alien);
    }

    public void RemoveAlien(Alien alien)
    {
        if(aliensAround.Contains(alien)) aliensAround.Remove(alien);
    }

    public void UpdateMusik()
    {
        source.Pause();
            float time = source.time;

        if(aliensAround.Count > 0)
        {
            source.clip = musik[1].clip;
            source.volume = musik[1].volume;
            source.pitch = musik[1].pitch;
        } else
        {
            source.clip = musik[0].clip;
            source.volume = musik[0].volume;
            source.pitch = musik[0].pitch;
        }


        source.Play();
        source.time = time;
    }
}
