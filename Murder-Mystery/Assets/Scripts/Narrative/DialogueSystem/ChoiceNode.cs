using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceNode : Node
{
    public List<Node> children = new List<Node>(2);
    public List<DialogueData.DialogueParameter> choiceConditions = new List<DialogueData.DialogueParameter>();

    protected override void OnStart()
    {
        Debug.Log("Entering Choice Node");
        bool bChooseRandom = false;
        bool result = true;

        foreach (var cond in choiceConditions)
        {
            if(cond.parameterKey.Equals("bChooseRandom"))
            {
                bChooseRandom = true;
                break;
            }

            result = result && (DialogueDataWriter.Instance.CheckCondition(cond.parameterKey, cond.parameterValue));
            Debug.Log("Result of choice" + result);
        }

        // Choose a random branch everytime.
        if (bChooseRandom)
        {
            Debug.Log("Random choice");
            float choice = UnityEngine.Random.Range(0, 2);

            if(choice == 1)
            {
                children[0].UpdateNode(dialogueTree);
            }
            else
            {
                children[1].UpdateNode(dialogueTree);
            }
        }

        else
        {
            if (result == true && children[0] != null)
            {
                children[0].UpdateNode(dialogueTree);
            }
            else if (result == false && children[1] != null)
            {
                children[1].UpdateNode(dialogueTree);
            }
        }
       
    }

    protected override void OnStop()
    {
        state = NodeState.Running;
    }

    protected override NodeState OnUpdate()
    {
        return NodeState.Success;
    }

    public override Node Clone()
    {
        ChoiceNode node = Instantiate(this);
        node.children = children.ConvertAll(c => c.Clone());
        return node;
    }

}
