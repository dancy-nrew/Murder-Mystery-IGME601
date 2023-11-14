using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject PausePanel, SettingsPanel, CluesPanel, CharactersPanel, ReturnPanel;
    public TextMeshProUGUI characterNameText, characterInfoText,
                            clueNameText, clueInfotext;



    public Image characterPortrait, clueSketch;

    public bool GamePaused = false;
    private int currentCharacterPage = 0;
    private int currentCluePage = 0;

    void Start()
    {
        ReturnPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        CluesPanel.SetActive(false);
        CharactersPanel.SetActive(false);
        PausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void UpdateCharacterPage()
    {
        
        // Ensure the page number is within bounds
        currentCharacterPage = Mathf.Clamp(currentCharacterPage, 0, GameManager.Instance.characters.Count - 1);

        // Fetch character info based on currentCharacterPage
        characterNameText.text = GameManager.Instance.GetCharacterName(currentCharacterPage);
        characterInfoText.text = GameManager.Instance.GetCharacterInfo(currentCharacterPage);
        characterPortrait.sprite = GameManager.Instance.GetCharacterSprite(currentCharacterPage);
        
    }


    public void ShowSettings()
    {
        ReturnPanel.SetActive(true);
        SettingsPanel.SetActive(true);
        PausePanel.SetActive(false);
        CharactersPanel.SetActive(false);
        CluesPanel.SetActive(false);
    }

    public void ShowCharacters()
    {
        Debug.Log("Showing characters");
        ReturnPanel.SetActive(true);
        CharactersPanel.SetActive(true);
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(false);
        CluesPanel.SetActive(false);
        UpdateCharacterPage();
    }

    public void ShowClues()
    {
        ReturnPanel.SetActive(true);
        CharactersPanel.SetActive(false);
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(false);
        CluesPanel.SetActive(true);
        UpdateCluePage();
    }

    
    public void NextCharacterPage()
    {
        // Increment page and update
        currentCharacterPage++;
        UpdateCharacterPage();
    }

    public void PreviousCharacterPage()
    {
        // Decrement page and update
        currentCharacterPage--;
        UpdateCharacterPage();
    }

    public void NextCluePage()
    {
        // Increment page and update
        currentCluePage++;
        UpdateCluePage();
    }

    public void PreviousCluePage()
    {
        // Decrement page and update
        currentCluePage--;
        UpdateCluePage();
    }

    public void Return()
    {
        SettingsPanel.SetActive(false);
        CluesPanel.SetActive(false);
        CharactersPanel.SetActive(false);
        ReturnPanel.SetActive(false);
        PausePanel.SetActive(true);
    }

    void UpdateCluePage()
    {
       // Ensure the page number is within bounds
        currentCluePage = Mathf.Clamp(currentCluePage, 0, GameManager.Instance.clues.Count - 1);

        // Fetch character info based on currentCluePage
        clueNameText.text = GameManager.Instance.GetClueName(currentCluePage);
        clueInfotext.text = GameManager.Instance.GetClueInfo(currentCluePage);
        clueSketch.sprite = GameManager.Instance.GetClueSketch(currentCluePage);
    }
    void Pause()
    {
        playerMovement.SetIsUIEnabled(true);
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    void Resume()
    {
        playerMovement.SetIsUIEnabled(false);
        ReturnPanel.SetActive(false);
        CharactersPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        CluesPanel.SetActive(false);
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }
}
