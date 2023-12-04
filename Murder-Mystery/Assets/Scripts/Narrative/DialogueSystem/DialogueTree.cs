using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR)
using UnityEditor.Experimental.GraphView;
#endif

// Adapted from https://youtu.be/nKpM98I7PeM?si=6_zO-Egnx1kB-9Ys
// This class handles the data and methods of the actual dialogue tree as a scriptable object.
[CreateAssetMenu()]
public class DialogueTree : ScriptableObject
{
    public Node rootNode;
    public Node.NodeState treeState = Node.NodeState.Running;
    public List<Node> nodes = new List<Node>();
    public List<Dialogue> dialogues = new List<Dialogue>();

    public Dialogue currentDialogue;
    public bool bIsInputting = false;
    public InputNode currentInputNode = null;

    public Dictionary<string, bool> parameters = new Dictionary<string, bool>();

    /*
     * Function that traverses the tree to get the next dialogue node in the sequence.
     */
    public Dialogue QueryTree()
    {
        currentDialogue = null;
        treeState = rootNode.UpdateNode(this);

        return currentDialogue;
    }

    /*
     * Resets all node states after tree is fully traversed. Called when dialogue box closes.
     */
    public void ResetTree()
    {
        foreach (Node node in nodes)
        {
            node.state = Node.NodeState.Running;
            if(node is InputNode)
            {
                InputNode inputNode = node as InputNode;
                inputNode.choice = -1;
            }
        }
    }


/*
 * These functiones represent dialouge tree editor functions.
 */
#if (UNITY_EDITOR)

    /* Creating a new Node object of passed in type
     * Creates a new node and adds it to dialogue tree list.
     * Input:
     * type: They types of node being instantiated.
     */
    public Node CreateNode(System.Type type)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();

        nodes.Add(node);
 
        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    /*
     * Deletes a node.
     * Input:
     * node: The Node being deleted.
     */
    public void DeleteNode(Node node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    /*
     * This function parents one node to another when an edge is created in the editor.
     * Input:
     * parent: Node object that is the parent.
     * child: Node object that is the child.
     * edge: Edge connecting them in the dialogue editor.
     */
    public void AddChild(Node parent, Node child, Edge edge)
    {
        SequenceNode sequenceNode = parent as SequenceNode;
        Debug.Log("Adding Child");
        if (sequenceNode)
        {
            Debug.Log("Parent is sequence node");
            if (child == null)
            {
                Debug.Log("No Child");
            }
            if(parent == null)
            {
                Debug.Log("No Parent");
            }

            sequenceNode.children.Add(child);
        }

        ChoiceNode choiceNode = parent as ChoiceNode;
        if (choiceNode)
        {
            Debug.Log("parent is choice node");
            if(edge.output.portName == "True")
            {
                Debug.Log("port is true");
                if (choiceNode.children.Count == 0)
                    choiceNode.children.Add(child);
                else
                    choiceNode.children[0] = child;
            }
            else if(edge.output.portName == "False")
            {
                Debug.Log("port is false");
                if (choiceNode.children.Count == 0)
                {
                    choiceNode.children.Add(null);
                    choiceNode.children.Add(child);
                }
                else if(choiceNode.children.Count == 1)
                    choiceNode.children.Add(child);
                else
                    choiceNode.children[1] = child;
            }
        }

        InputNode inputNode = parent as InputNode;
        if(inputNode)
        {
            inputNode.children.Add(child);
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            rootNode.child = child;
        }
    }

    /* Unparents a child from a parent when an edge is deleted in the editor.
     * Input:
     * parent: Node that is the parent.
     * child: Node that is the child.
     * edge: Edge that has been disconnected.
    */
    public void RemoveChild(Node parent, Node child, Edge edge)
    {

        SequenceNode sequenceNode = parent as SequenceNode;
        if (sequenceNode)
        {
            sequenceNode.children.Remove(child);
        }

        ChoiceNode choiceNode = parent as ChoiceNode;
        if (choiceNode)
        {
            if (edge.output.portName == "True")
            {
                choiceNode.children[0] = null;
            }
            else if (edge.output.portName == "False")
            {
                choiceNode.children[1] = null;
            }
        }

        InputNode inputNode = parent as InputNode;
        if (inputNode)
        {
            inputNode.children.Remove(child);
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            rootNode.child = null;
        }
    }
#endif


    /*
     * Helper function that gets the children of a node.
     * Input:
     * parent: Node to get children of.
     */
    public List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();
        SequenceNode sequenceNode = parent as SequenceNode;
        if (sequenceNode)
        {
           return sequenceNode.children;
        }

        ChoiceNode choiceNode = parent as ChoiceNode;
        if (choiceNode)
        {
           return choiceNode.children;
        }

        InputNode inputNode = parent as InputNode;
        if (inputNode)
        {
            return inputNode.children;
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode && rootNode.child != null)
        {
            children.Add(rootNode.child);
        }

        return children;
    }

    /*
     * Function to clone this tree.
     */
    public DialogueTree Clone()
    {
        DialogueTree tree = Instantiate(this);
        tree.nodes.Clear();
        tree.rootNode = tree.rootNode.Clone(tree);
        return tree;
    }
}
