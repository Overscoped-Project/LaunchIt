using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    public enum Names {Pilot, AI, Inventory, Unknown};

    [SerializeField] private Names[] names;
    [TextArea(3, 10)]
    [SerializeField] private string[] sentences;

    public enum EventCode {None, Intro, GameEnd_Spaceship, Access_Repository, GameEnd_Repository, Outro};
    [SerializeField] private EventCode eventCode;
    public bool playBefore;
  
    public string[] GetSentences()
    {
        return sentences;
    }

    public Names[] GetNames()
    {
        return names;
    }

    public EventCode getEventCode()
    {
        return eventCode;
    }

    public Dialogue(Names[] names, string[] sentences)
    {
        this.names = names;
        this.sentences = sentences;
    }
}
