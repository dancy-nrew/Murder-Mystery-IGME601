using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adapted from https://youtu.be/_nRzoTzeyxU?si=AB3_KumtIm_VuaVb
// Class represents a dialogue interaction.
[System.Serializable]
public class Dialogue
{

    public string characterName;
    [TextArea(3,10)]
    public string[] sentences;

    public Dialogue(string characterName, string[] sentences)
    {
        this.characterName = characterName;
        this.sentences = sentences;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
