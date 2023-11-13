using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<CharacterSO> characterList = new List<CharacterSO>();
    private Dictionary<CharacterSO.ECharacter, CharacterSO> characterDict = new Dictionary<CharacterSO.ECharacter, CharacterSO>();

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
       foreach(CharacterSO character in characterList)
        {
            characterDict.Add(character.character, character);
        }
    }

    public CharacterSO GetCharacterSOFromKey(CharacterSO.ECharacter key)
    {
        return characterDict[key];
    }
}
