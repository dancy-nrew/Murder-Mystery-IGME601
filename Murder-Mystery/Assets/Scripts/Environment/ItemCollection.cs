using System;
using Unity.VisualScripting;
using UnityEngine;

public class CollectibleItem : Interactable
{
    // You can add more properties here if needed
    private PlayerMovement playerMovement;
    public string itemName;
    public string itemDescription;
    public Sprite itemSketch;

   public override void OnInteraction()
    {    
            Debug.Log("Interacted item");
            Collect();
            Debug.Log("Interacted item finish");
            playerMovement.SetIsUIEnabled(false);   //Not sure why but the grabbing of items is interpreteed as being UI
    
    }

    
    private void Collect()
    {
        // Create a new clue from this item's properties
        Clue newClue = new Clue { Name = itemName, Description = itemDescription, Sketch = itemSketch };

        // Add the clue to the GameManager's list of clues
        GameManager.Instance.AddClue(newClue);

        gameObject.SetActive(false);
        Destroy(gameObject);

        // You can also trigger any UI updates or sound effects here
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