using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
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
        DisplayDialogue(currentDialogue);
        if (currentDialogue == null)
        {
            currentDialogueTree = null;
            return;
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
            if (currentSentence >= currentDialogue.sentences.Length)
            {
                currentSentence = 0;
                currentDialogue = currentDialogueTree.QueryTree();
                DisplayDialogue(currentDialogue);
            }

            if (currentDialogue == null)
            {
                EndDialogue();
                return;
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
    private void EndDialogue()
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

    private CharacterSO GetCharacterFromDialogue(Dialogue dialogue)
    {
        CharacterSO.ECharacter characterKey = currentDialogue.character;
        CharacterSO characterSO = null;
        foreach (CharacterSO charSO in GameManager.Instance.characterList)
        {
            if (charSO.character == characterKey)
            {
                characterSO = charSO;
                break;
            }
        }
        return characterSO;
    }
}
