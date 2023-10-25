using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private Queue<string> sentences;
    private Coroutine characterUpdateCoroutine;

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
    }

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(List<Dialogue> dialogues)
    {
        animator.SetBool("bIsOpen", true);
        Debug.Log("Showing Dialogues " + dialogues.Count);

        sentences.Clear();

        foreach (Dialogue dialogue in dialogues)
        {
            nameText.text = dialogue.characterName;

            

            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        if(characterUpdateCoroutine != null)
        {
            StopCoroutine(characterUpdateCoroutine);
        }
        
        characterUpdateCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(characterUpdateTime);
        }
    }

    private void EndDialogue()
    {
        animator.SetBool("bIsOpen", false);
    }
}
