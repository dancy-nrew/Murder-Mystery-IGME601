using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CharacterSO : ScriptableObject
{
    public enum ECharacter
    {
        Ace,
        Yates,
        Chief,
        Morse,
        Reeves,
        Stone,
        Connor,
        Paine
    }

    public ECharacter character;
    public string displayName;
    public Sprite characterPortrait;

}
