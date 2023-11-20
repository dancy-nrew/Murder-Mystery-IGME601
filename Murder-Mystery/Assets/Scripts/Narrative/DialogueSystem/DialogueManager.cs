using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Adapted from https://youtu.be/_nRzoTzeyxU?si=AB3_KumtIm_VuaVb
// Class handles dialogue ui functionalities by running through a dialogue tree.
// This class is called from objects that have the dialouge tree runner script on them.

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public GameObject dialogueBox;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image characterPortraitIMG;
    public Button continueButton;
    public GameObject choicesParent;
    public List<Button> choiceButtons;
    public List<TextMeshProUGUI> choiceButtonTexts;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float characterUpdateTime = 0.5f;
    [SerializeField]
    private PlayerMovement playerMovement;

    private Queue<string> sentences;
    private Coroutine characterUpdateCoroutine;

    private Queue<Dialogue> dialogueQueue;
    private int currentSentence = 0;
    private bool bIsCharacterCoroutineRunning = false;

    private DialogueTree currentDialogueTree;
    private Dialogue currentDialogue;
    private CharacterSO currentCharacter;
    private InputNode currentInputNode;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        dialogueQueue = new Queue<Dialogue> ();
    }

    void Start()
    {
        sentences = new Queue<string>();
    }

    /*
     * This function takes in a dialogue tree and starts the process of displaying dialogue by displaying the first sentence.
     * Input: 
     * dialogue Tree: The dialogue tree asset to be traversed.
     */
    public void ShowDialogue(DialogueTree dialogueTree)
    {
        Debug.Log("Showing dialogues");
        if (!dialogueTree || (currentDialogueTree && currentDialogueTree == dialogueTree))
        {
            Debug.Log("No dialogue tree");
            return;
        }
        currentDialogueTree = dialogueTree;
        currentDialogue = currentDialogueTree.QueryTree();
        // DisplayDialogue(currentDialogue);
        if (currentDialogue == null)
        {
            if(currentDialogueTree.bIsInputting)
            {
                currentInputNode = currentDialogueTree.currentInputNode;
                ShowInput();
            }
            else
            {
                currentDialogueTree = null;
                return;
            }      
        }

        animator.SetBool("bIsOpen", true);
        playerMovement.SetIsUIEnabled(true);
        
        currentSentence = 0;

        DisplayNextSentence();
       
    }

    /*
     * This function displays the next sentence of the current dialogue tree or fast-forwards current sentence.
     * Called when continue buttone is hit in the dialouge box.
     */
    public void DisplayNextSentence()
    {

        if (!currentDialogueTree) return;
       
        
        // If the senetence is not being animated in.
        if (!bIsCharacterCoroutineRunning)
        {
            if(currentDialogue != null && currentDialogue.bTransitionToCardBattle)
            {
                EndDialogue();
                SceneManager.LoadScene("Card Battler");
                return;
            }

            if (currentSentence >= currentDialogue.sentences.Length)
            {
                currentSentence = 0;
                currentDialogue = currentDialogueTree.QueryTree();
                //DisplayDialogue(currentDialogue);
            }

            if (currentDialogue == null)
            {
                if (currentDialogueTree.bIsInputting)
                {
                    currentInputNode = currentDialogueTree.currentInputNode;
                    ShowInput();
                    return;
                }
                else
                {
                    EndDialogue();
                    return;
                }
            }

            currentCharacter = GetCharacterFromDialogue(currentDialogue);

            nameText.text = currentCharacter.displayName;
            characterPortraitIMG.sprite = currentCharacter.characterPortrait;
            string sentence = currentDialogue.sentences[currentSentence];

            characterUpdateCoroutine = StartCoroutine(TypeSentence(sentence));
            bIsCharacterCoroutineRunning = true;
            currentSentence++;
        }

        // Otherwise go fast-forward the current sentence.
        else
        {
            if (characterUpdateCoroutine != null)
            {
                StopCoroutine(characterUpdateCoroutine);
                bIsCharacterCoroutineRunning = false;
            }

            currentCharacter = GetCharacterFromDialogue(currentDialogue);

            nameText.text = currentCharacter.displayName;
            characterPortraitIMG.sprite = currentCharacter.characterPortrait;
            dialogueText.text = currentDialogue.sentences[currentSentence - 1];
        }
    }

    /*
     * This function handle showing input options to the player when an InputNode is reached while traversing the dialogue tree.
     */
    private void ShowInput()
    {
        continueButton.gameObject.SetActive(false);
        choicesParent.SetActive(true);
        currentCharacter = GameManager.Instance.GetCharacterSOFromKey(CharacterSO.ECharacter.Ace);
        dialogueText.text = "";
        nameText.text = currentCharacter.displayName;
        characterPortraitIMG.sprite = currentCharacter.characterPortrait;

        for(int i = 0; i < choiceButtons.Count; i++)
        {
            if(currentInputNode.choices.Count > i)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtonTexts[i].text = currentInputNode.choices[i];
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }

    }

    /*
     * This is a call-back function that is called when the player chooses an input in the dialogue box.
     * Input:
     * choice: Int representing which choice the player chose.
     */
    public void OnInputEnterred(int choice)
    {
        currentInputNode.choice = choice;
        currentDialogueTree.bIsInputting = false;
        currentDialogueTree.currentInputNode = null;

        continueButton.gameObject.SetActive(true);
        choicesParent.SetActive(false);
        currentDialogue = currentDialogueTree.QueryTree();
        DisplayNextSentence();

    }

    /* Coroutine that animates the senetence letter by letter.
     * Input:
     * sentence: string to be animated in.
     */
    IEnumerator TypeSentence(string sentence)
    {
       
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(characterUpdateTime);
        }

        bIsCharacterCoroutineRunning = false;
    }


    /*
     * Function is called after a dialouge tree has been completed. Closes dialogue box.
     */
    public void EndDialogue()
    {
        Debug.Log("Ending dialogue");
        animator.SetBool("bIsOpen", false);

        currentDialogueTree.ResetTree();
        currentDialogueTree = null;
        
        playerMovement.SetIsUIEnabled(false);
    }


    /*
     * Debug Function to output dialogue object to console.
     * Input:
     * d: Dialogue asset to be printed to console.
     */
    private void DisplayDialogue(Dialogue d)
    {
        if(d == null)
        {
            Debug.Log("Null Dialogue");
        }
        else
        {
            CharacterSO charSO = GetCharacterFromDialogue(d);
            Debug.Log(charSO.displayName);
            foreach (string s in d.sentences)
            {
                Debug.Log(s);
            }
        }
       
        Debug.Log("----------------------------------------");
    }

    /*
     * Gets the corresponding Character SO from a Dialogue Object using ECharcacter value.
     */
    private CharacterSO GetCharacterFromDialogue(Dialogue dialogue)
    {
        CharacterSO.ECharacter characterKey = currentDialogue.character;

        return GameManager.Instance.GetCharacterSOFromKey(characterKey);
    }


}
