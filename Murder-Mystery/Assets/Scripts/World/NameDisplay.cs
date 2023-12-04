using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameDisplay : MonoBehaviour
{
    public TextMeshPro displayMesh;
    public string displayName;
    // Start is called before the first frame update
    void Start()
    {
        displayMesh.SetText(displayName);
        displayMesh.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        displayMesh.enabled = true;
    }
    private void OnMouseExit()
    {
        displayMesh.enabled = false;
    }
}
