using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardData cardData;
    private GameObject frontFace;
    private GameObject backFace;

    // For in-editor debugging purposes
    public int faceValue;
    public Suit suit;

    private void Awake()
    {
        frontFace = transform.GetChild(0).gameObject;
        backFace = transform.GetChild(1).gameObject;
    }

    public void SetCardData(int face, Suit suit)
    {
        faceValue = face;
        this.suit = suit;
        cardData = new CardData(face, suit);
    }

    public void MakeInteractable()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactables");
        frontFace.layer = LayerMask.NameToLayer("Interactables");
    }

    public void SetFrontFaceMaterial(Material texture)
    {
        MeshRenderer mr = frontFace.GetComponent<MeshRenderer>();
        mr.material = texture;
    }
}
