using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    [SerializeField] private string name;
    [TextArea(3, 10)]
    [SerializeField] private string[] sentences;
  
    public string[] GetSentences()
    {
        return sentences;
    }

    public Dialogue(string name, string[] sentences)
    {
        this.name = name;
        this.sentences = sentences;
    }
}
