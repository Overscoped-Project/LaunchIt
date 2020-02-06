using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musik;

    private AudioSource source;

    private List<Alien> aliensAround = new List<Alien>();

    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = musik[0];
        source.playOnAwake = true;
        source.loop = true;
        source.Play();
    }

    public void AddAlien(Alien alien)
    {
        if (!aliensAround.Contains(alien))
            aliensAround.Add(alien);
    }

    public void RemoveAlien(Alien alien)
    {
        if(aliensAround.Contains(alien))
        aliensAround.Remove(alien);
    }

    public void UpdateMusik()
    {
            source.Pause();
            float time = source.time;

        if(aliensAround.Count > 0)
        {
            source.clip = musik[1];
        } else
        {
            source.clip = musik[0];
        }


            source.Play();
            source.time = time;
    }
}
