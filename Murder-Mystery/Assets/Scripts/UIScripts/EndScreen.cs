using Codice.CM.WorkspaceServer.Tree.GameUI.Checkin.Updater;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private GameObject endScreen;
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private float fadeSpeed;

    private bool bDisplayEndScreen = false;
    private bool bFadedIn = false;
    private bool bStartFading = false;


    // Start is called before the first frame update
    void Start()
    {
        DialogueDataWriter.Instance.dParameterUpdated += OnParameterUpdated;
    }

    // Update is called once per frame
    void Update()
    {
        if (bDisplayEndScreen && playerMovement.GetIsUIEnabled() == false)
        {
            bStartFading = true;
            playerMovement.SetIsUIEnabled(true);
            bDisplayEndScreen = false;
        }

        if (bStartFading && !bFadedIn)
        {
            if(_canvasGroup.alpha < 1)
            {
                endScreen.SetActive(true);
                _canvasGroup.alpha += Time.deltaTime * fadeSpeed;
                if(_canvasGroup.alpha >= 1)
                {
                    bFadedIn = true;
                }
            }
        }
    }

    private void OnParameterUpdated(string key, bool value)
    {
        Debug.Log("Parameter in end screen " + key);
        if(key.Equals("bGameEnd") && value == true)
        {
            bDisplayEndScreen = true;
        }
    }

    private void OnDestroy()
    {
        DialogueDataWriter.Instance.dParameterUpdated -= OnParameterUpdated;
    }
}
