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
        Paine,
        WiseOldMan
    }

    public ECharacter character;
    public string displayName;
    public Sprite characterPortrait;
    public DialogueTree cardBattleDialogueTree;
    public string witnessParameter;
    public string locationParameter;
    public string motiveParameter;

}
