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
        }   
    }
}
