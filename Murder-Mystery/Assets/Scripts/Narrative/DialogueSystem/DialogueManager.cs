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

    public void StartDialogue(List<Dialogue> dialogues)
    {
        animator.SetBool("bIsOpen", true);
        Debug.Log("Showing Dialogues " + dialogues.Count);

        dialogueQueue.Clear();
        dialogueQueue = new Queue<Dialogue>(dialogues);
        currentSentence = 0;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        if(!bIsCharacterCoroutineRunning)
        {
            if (currentSentence >= dialogueQueue.Peek().sentences.Length)
            {
                dialogueQueue.Dequeue();
                currentSentence = 0;
            }
            if (dialogueQueue.Count == 0)
            {
                EndDialogue();
                return;
            }

            nameText.text = dialogueQueue.Peek().characterName;
            string sentence = dialogueQueue.Peek().sentences[currentSentence];

            characterUpdateCoroutine = StartCoroutine(TypeSentence(sentence));
            bIsCharacterCoroutineRunning = true;
            currentSentence++; 
        }
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

    private void EndDialogue()
    {
        animator.SetBool("bIsOpen", false);
        playerMovement.SetIsUIEnabled(false);
    }
}
