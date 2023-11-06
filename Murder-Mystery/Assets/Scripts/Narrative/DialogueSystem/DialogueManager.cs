using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

// Adapted from https://youtu.be/_nRzoTzeyxU?si=AB3_KumtIm_VuaVb
// Class handles dialogue ui functionalities

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public GameObject dialogueBox;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
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

   /* public void StartDialogue(List<Dialogue> dialogues)
    {
        animator.SetBool("bIsOpen", true);
        Debug.Log("Showing Dialogues " + dialogues.Count);

        dialogueQueue.Clear();
        dialogueQueue = new Queue<Dialogue>(dialogues);
        currentSentence = 0;

        DisplayNextSentence();
    }*/


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
    public void DisplayNextSentence()
    {

        // If previous senetence is not being typed out, go to next sentence.
         if(!bIsCharacterCoroutineRunning)
         {
             if (currentSentence >= dialogueQueue.Peek().sentences.Length)
             {
                 dialogueQueue.Dequeue();
                 currentSentence = 0;
             }
             if (dialogueQueue.Count == 0)
             {
                 if(bIsInputting)
                 {
                     DisplayInputOptions();
                 }
                 else
                 {
                     EndDialogue();
                     return;
                 }   
             }

             nameText.text = dialogueQueue.Peek().characterName;
             string sentence = dialogueQueue.Peek().sentences[currentSentence];

             characterUpdateCoroutine = StartCoroutine(TypeSentence(sentence));
             bIsCharacterCoroutineRunning = true;
             currentSentence++; 
         }

         // Otherwise go fast-forward the current sentence.
         else
         {
             if(characterUpdateCoroutine != null)
             {
                 StopCoroutine(characterUpdateCoroutine);
                 bIsCharacterCoroutineRunning = false;
             }

             nameText.text = dialogueQueue.Peek().characterName;
             dialogueText.text = dialogueQueue.Peek().sentences[currentSentence - 1];
         } 
        
    }
    */

    public void DisplayNextSentence()
    {
        //Debug.Log("Displaying Sentence, current sentence : " + currentSentence);

        if (!currentDialogueTree) return;

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

            nameText.text = currentDialogue.characterName;
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

            nameText.text = currentDialogue.characterName;
            dialogueText.text = currentDialogue.sentences[currentSentence - 1];
        }
    }

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

/*    private void DisplayInputOptions()
    {
        if(inputNode != null)
        {

        }
    }*/

    private void EndDialogue()
    {
        Debug.Log("Ending dialogue");
        animator.SetBool("bIsOpen", false);

        currentDialogueTree.ResetTree();
        currentDialogueTree = null;
        
        playerMovement.SetIsUIEnabled(false);
    }

    private void DisplayDialogue(Dialogue d)
    {
        if(d == null)
        {
            Debug.Log("Null Dialogue");
        }
        else
        {
            Debug.Log(d.characterName);
            foreach (string s in d.sentences)
            {
                Debug.Log(s);
            }
        }
       
        Debug.Log("----------------------------------------");
    }
}
