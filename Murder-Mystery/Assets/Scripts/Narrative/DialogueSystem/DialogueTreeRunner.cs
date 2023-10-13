using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTreeRunner : MonoBehaviour
{

    [SerializeField]
    DialogueTree dialogueTree;

    void Start()
    {
        dialogueTree = dialogueTree.Clone();
    }

    // Update is called once per frame
    void Update()
    {
        //dialogueTree.UpdateTree();
    }

    public void UpdateTree()
    {
        dialogueTree.UpdateTree();
    }
}
