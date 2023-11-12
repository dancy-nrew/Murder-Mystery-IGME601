using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

/*
 * Represents the data of a choice node.
*/
public class ChoiceNode : Node
{
    public List<Node> children = new List<Node>(2);
    public List<DialogueData.DialogueParameter> choiceConditions = new List<DialogueData.DialogueParameter>();

    protected override void OnStart()
    {
       
    }

    protected override void OnStop()
    {
        state = NodeState.Running;
    }

    protected override NodeState OnUpdate()
    {
        bool bChooseRandom = false;
        bool result = true;

        foreach (var cond in choiceConditions)
        {
            if (cond.parameterKey.Equals("bChooseRandom"))
            {
                bChooseRandom = true;
                break;
            }

            result = result && (DialogueDataWriter.Instance.CheckCondition(cond.parameterKey, cond.parameterValue));
        }

        // Removed randome functionality temporarily after refactor.
        // Choose a random branch everytime.
        /*NodeState nodeState = NodeState.Running;
        if (bChooseRandom)
        {
            float choice = UnityEngine.Random.Range(0, 2);

            if (choice == 1)
            {
                nodeState = children[0].UpdateNode(dialogueTree);
            }
            else
            {
                nodeState = children[1].UpdateNode(dialogueTree);
            }
        }*/
        /*
                else
                {*/

        NodeState nodeState = NodeState.Running;
        if (result == true && children[0] != null)
        {
            nodeState = children[0].UpdateNode(dialogueTree);
        }
        else if (result == false && children[1] != null)
        {
            nodeState = children[1].UpdateNode(dialogueTree);
        }

        //}
        return nodeState;
    }

    /*
     * Function to clone this node.
     */
    public override Node Clone(DialogueTree tree)
    {
        ChoiceNode node = Instantiate(this);
        tree.nodes.Add(node);
        node.children = children.ConvertAll(c => c.Clone(tree));
        return node;
    }

}
