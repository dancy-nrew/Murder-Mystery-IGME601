using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance._shouldLoadFromSavedLocation)
        {
            gameObject.transform.position = GameManager.Instance.LoadPlayerPosition();
            NPCInteractable[] npcInteractables = GameObject.FindObjectsOfType<NPCInteractable>();

            foreach(NPCInteractable npc in npcInteractables)
            {
                if(npc.character == GameManager.Instance._lastTalkedToCharacter)
                {
                    if (DialogueDataWriter.Instance.CheckCondition("bHasWon" + npc.character.ToString() + "CardBattle", true))
                    {
                        npc.OnInteraction();
                    }
                    break;
                }
            }
        }   
        else if(GameManager.Instance._justWalkedThroughDoor)
        {
            GameObject[] doors =  GameObject.FindGameObjectsWithTag("Door");
            string doorKey = GameManager.Instance.GetDoorInfo();
            foreach (GameObject door in doors)
            {
                DoorInteractable doorInteractable = door.GetComponent<DoorInteractable>();

                if(doorInteractable != null && doorKey == doorInteractable.doorKey)
                {
                    gameObject.transform.position = doorInteractable.doorDropOff.position;
                    break;
                }
            }
        }
    }
}
