using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * This class handles the functionality of the input node in the dialogue graph.
 */
public class InputNode : Node
{
    private const int UNCHOSEN = -1;

    public List<Node> children = new List<Node>();
    public List<string> choices = new List<string>();
    [HideInInspector]
    public int choice = UNCHOSEN;

    /*
     * If the choice has not been set, the tree is now inputting i.e., this node's choice will be shown in the dialogue box.
     */
    protected override void OnStart()
    {
        if (dialogueTree != null && choice == UNCHOSEN)
        {
            dialogueTree.bIsInputting = true;
            dialogueTree.currentInputNode = this;
        }
       
    }

    /*
     * If choice has not been set this is the first run through of the node and hence state is reset as its children have not been traversed yet.
     */
    protected override void OnStop()
    {
        if(choice == UNCHOSEN)
        {
            state = NodeState.Running;
        }
    }

    /*
     * If choice has not been set return to stop traversal of tree, else return child's state.
     */
    protected override NodeState OnUpdate()
    {
        if(choice == UNCHOSEN)
        {
            return NodeState.Success;
        }
        else
        {
            if(children[choice].state == NodeState.Running)
            {
                return children[choice].UpdateNode(dialogueTree);
            }
            else
            {
                return NodeState.Success;
            }
        }
        
    }

    public override Node Clone(DialogueTree tree)
    {
        InputNode node = Instantiate(this);
        tree.nodes.Add(node);
        node.children = children.ConvertAll(c => c.Clone(tree));
        return node;
    }

    protected override void UpdateNodeState()
    {
        if(choice != UNCHOSEN)
        {
            state = children[choice].state;
        }
    }
}
