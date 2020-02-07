﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    public enum Names {General, AI, Inventory};

    [SerializeField] private Names[] names;
    [TextArea(3, 10)]
    [SerializeField] private string[] sentences;
  
    public string[] GetSentences()
    {
        return sentences;
    }

    public Names[] GetNames()
    {
        return names;
    }

    public Dialogue(Names[] names, string[] sentences)
    {
        this.names = names;
        this.sentences = sentences;
    }
}
