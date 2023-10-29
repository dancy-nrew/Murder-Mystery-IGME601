using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

// Adapted from https://youtu.be/nKpM98I7PeM?si=6_zO-Egnx1kB-9Ys
// This class handles the data and method of the actual dialogue tree as a scriptable object.
[CreateAssetMenu()]
public class DialogueTree : ScriptableObject
{
    public Node rootNode;
    public Node.NodeState treeState = Node.NodeState.Running;
    public List<Node> nodes = new List<Node>();
    public List<Dialogue> dialogues = new List<Dialogue>();

    public Dictionary<string, bool> parameters = new Dictionary<string, bool>();

    public Node.NodeState UpdateTree()
    {
        dialogues.Clear();
        if(rootNode.state == Node.NodeState.Running)
        {
            treeState = rootNode.UpdateNode(this);
        }

        Debug.Log("Call to dialogue manager");
        DialogueManager.Instance.StartDialogue(dialogues);
        return treeState;
    }

#if (UNITY_EDITOR)
    // Creating a new Node object of passed in type
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

    // Deleting a node
    public void DeleteNode(Node node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

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

        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            rootNode.child = child;
        }
    }

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


        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            rootNode.child = null;
        }
    }
#endif


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

        RootNode rootNode = parent as RootNode;
        if (rootNode && rootNode.child != null)
        {
            children.Add(rootNode.child);
        }

        return children;
    }

    public DialogueTree Clone()
    {
        DialogueTree tree = Instantiate(this);
        tree.rootNode = tree.rootNode.Clone();
        return tree;
    }
}
