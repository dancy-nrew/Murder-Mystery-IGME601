using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Responsible for interaction with environmental clues.
 */
public class ClueInteractable : Interactable
{   
    public Clue self;
    public string itemName;
    public string itemDescription;
    public Sprite itemSketch;
    public String characterToAppend;
    public String appendUponPickup;
    public Boolean updatesCharcater;
    [SerializeField] public GameObject notificationIcon;

    public List<DialogueData.DialogueParameter> interactableConditions = new List<DialogueData.DialogueParameter>();

    protected override void Awake()
    {
        //Debug.Log(self.ToString());
        foreach(Clue cond in GameManager.Instance.clues)
            Debug.Log(cond.ToString());
        
        self = new Clue { Name = itemName, Description = itemDescription, Sketch = itemSketch };
        base.Awake();
        bIsInteractable = false;
        

        //Check if clue is alraedy within the journal
        foreach(Clue cond in GameManager.Instance.clues)
        {
            //Debug.Log(cond.Name+ " = " + self.Name + "?");
            if(cond.Name.Equals(self.Name))
                gameObject.SetActive(false);
        }
    }

    public override void OnInteraction()
    {
        Collect();
    }

    // Should be optimized to be event based instead of checking every frame in future.
    public void Update()
    {
        bool result = true;

        foreach (var cond in interactableConditions)
        {
            result = result && (DialogueDataWriter.Instance.CheckCondition(cond.parameterKey, cond.parameterValue));
            if (result == false)
                break;
        }
        if(result == true)
        {
            bIsInteractable = true;
        }
    }

    private void Collect()
    {
        notificationIcon.SetActive(true);
        // Add the clue to the GameManager's list of clues
        GameManager.Instance.AddClue(self);

        DialogueDataWriter.Instance.UpdateDialogueData("bHasPickedUp" + itemName, true);
        gameObject.SetActive(false);
        //Destroy(gameObject);
        // You can also trigger any UI updates or sound effects here
        string message = itemName + " has been added to your journal";
        GameManager.Instance.FlashMessage(message);

        if(updatesCharcater)
        {
            message = characterToAppend + "\'s journal entry has been updated";
            GameManager.Instance.FlashMessage(message);
            GameManager.Instance.AppendToCharacterDescription(characterToAppend, appendUponPickup);
        }
    }
    private void Start()
    {
        
    }
}
