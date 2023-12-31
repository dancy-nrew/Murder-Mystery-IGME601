using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject PausePanel, CluesPanel, CharactersPanel, ReturnPanel, notificationIcon;
    public TextMeshProUGUI characterNameText, characterInfoText,
                            clueNameText, clueInfotext;



    public Image characterPortrait, clueSketch;

    public TextMeshProUGUI motiveText, witnessText, locationText;

    public bool GamePaused = false;
    private int currentCharacterPage = 0;
    private int currentCluePage = 0;

    HashSet<string> notificationParams = new HashSet<string> { "bMathDepartmentLeadership", "bPaineRevelation", "bHasPickedUpVase", "bStolenResearch", "bReevesRevelation", "bHasPickedUpAward", "bBrokenHeart", "bConnorRevelation", "bHasPickedUpModel" };

    void Start()
    {
        ReturnPanel.SetActive(false);
        CluesPanel.SetActive(false);
        CharactersPanel.SetActive(false);
        PausePanel.SetActive(false);

        DialogueDataWriter.Instance.dParameterUpdated += OnParameterUpdated;
    }

    // Update is called once per frame
    /*
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
    }*/

    public void Journal()
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

    public void UpdateCharacterPage()
    {
        
        // Ensure the page number is within bounds
        currentCharacterPage = Mathf.Clamp(currentCharacterPage, 0, GameManager.Instance.characters.Count - 1);
        AudioManager.Instance.PlaySFX("aJournalSection");
        // Fetch character info based on currentCharacterPage
        characterNameText.text = GameManager.Instance.GetCharacterName(currentCharacterPage);
        characterInfoText.text = GameManager.Instance.GetCharacterInfo(currentCharacterPage);
        characterPortrait.sprite = GameManager.Instance.GetCharacterSprite(currentCharacterPage);
        motiveText.text = GameManager.Instance.GetCharacterMotive(currentCharacterPage);
        witnessText.text = GameManager.Instance.GetCharacterWitness(currentCharacterPage);
        locationText.text = GameManager.Instance.GetCharacterLocation(currentCharacterPage);
        
    }


    public void ShowSettings()
    {
        ReturnPanel.SetActive(true);
        PausePanel.SetActive(false);
        CharactersPanel.SetActive(false);
        CluesPanel.SetActive(false);
    }

    public void ShowCharacters()
    {
        Debug.Log("Showing characters");
        ReturnPanel.SetActive(true);
        CharactersPanel.SetActive(true);
        CluesPanel.SetActive(false);
        UpdateCharacterPage();
    }

    public void ShowClues()
    {
        ReturnPanel.SetActive(true);
        CharactersPanel.SetActive(false);
        CluesPanel.SetActive(true);
        UpdateCluePage();
    }

    
    public void NextCharacterPage()
    {
        // Increment page and update
        AudioManager.Instance.PlaySFX("aJournalPage");
        currentCharacterPage++;
        UpdateCharacterPage();
    }

    public void PreviousCharacterPage()
    {
        // Decrement page and update
        AudioManager.Instance.PlaySFX("aJournalPage");
        currentCharacterPage--;
        UpdateCharacterPage();
    }

    public void NextCluePage()
    {
        // Increment page and update
        AudioManager.Instance.PlaySFX("aJournalPage");
        currentCluePage++;
        UpdateCluePage();
    }

    public void PreviousCluePage()
    {
        // Decrement page and update
        AudioManager.Instance.PlaySFX("aJournalPage");
        currentCluePage--;
        UpdateCluePage();
    }

    public void Return()
    {
        CluesPanel.SetActive(false);
        CharactersPanel.SetActive(false);
        ReturnPanel.SetActive(false);
        PausePanel.SetActive(true);
    }

    void UpdateCluePage()
    {
       // Ensure the page number is within bounds
        currentCluePage = Mathf.Clamp(currentCluePage, 0, GameManager.Instance.clues.Count - 1);
        AudioManager.Instance.PlaySFX("aJournalSection");
        // Fetch character info based on currentCluePage
        clueNameText.text = GameManager.Instance.GetClueName(currentCluePage);
        clueInfotext.text = GameManager.Instance.GetClueInfo(currentCluePage);
        clueSketch.sprite = GameManager.Instance.GetClueSketch(currentCluePage);
    }
    void Pause()
    {
        AudioManager.Instance.PlaySFX("aJournalOpen");
        playerMovement.SetIsUIEnabled(true);
        PausePanel.SetActive(true);
        notificationIcon.SetActive(false);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void Resume()
    {
        AudioManager.Instance.PlaySFX("aJournalClose");
        playerMovement.SetIsUIEnabled(false);
        ReturnPanel.SetActive(false);
        CharactersPanel.SetActive(false);
        CluesPanel.SetActive(false);
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    private void OnParameterUpdated(string key, bool value)
    {
       if(notificationParams.Contains(key))
       {
            notificationIcon.SetActive(true);
       }
    }
}
