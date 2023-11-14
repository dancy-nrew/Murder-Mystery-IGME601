using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Responsible for interaction with environmental clues.
 */
public class ClueInteractable : Interactable
{
    public string itemName;
    public string itemDescription;
    public Sprite itemSketch;

    public List<DialogueData.DialogueParameter> interactableConditions = new List<DialogueData.DialogueParameter>();

    protected override void Awake()
    {
        base.Awake();
        bIsInteractable = false;
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
        // Create a new clue from this item's properties
        Clue newClue = new Clue { Name = itemName, Description = itemDescription, Sketch = itemSketch };

        // Add the clue to the GameManager's list of clues
        GameManager.Instance.AddClue(newClue);

        DialogueDataWriter.Instance.UpdateDialogueData("bHasPickedUp" + itemName, true);
        gameObject.SetActive(false);
        Destroy(gameObject);

        // You can also trigger any UI updates or sound effects here
    }
}
