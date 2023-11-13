using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adapted from https://youtu.be/_nRzoTzeyxU?si=AB3_KumtIm_VuaVb
// Class represents a dialogue interaction.
[System.Serializable]
public class Dialogue
{

    public CharacterSO.ECharacter character;

    [TextArea(3,10)]
    public string[] sentences;
  
    public bool bTransitionToCardBattle = false;
    public Dialogue(string[] sentences)
    {
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
