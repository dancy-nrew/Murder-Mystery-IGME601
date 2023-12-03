using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardData cardData;
    private GameObject frontFace;
    private GameObject backFace;
    private bool _isPlayed;

    // For in-editor debugging purposes
    public int faceValue;
    public Suit suit;

    private void Awake()
    {
        _isPlayed = false;
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
        if (_isPlayed)
        {
            return;
        }
        gameObject.layer = LayerMask.NameToLayer("Interactables");
        frontFace.layer = LayerMask.NameToLayer("Interactables");
    }

    public void MakeNonInteractable()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        frontFace.layer = LayerMask.NameToLayer("Default");
    }

    public void SetToPlayed()
    {
        _isPlayed = true;
    }

    public void SetFrontFaceMaterial(Material texture)
    {
        MeshRenderer mr = frontFace.GetComponent<MeshRenderer>();
        mr.material = texture;
    }
}
