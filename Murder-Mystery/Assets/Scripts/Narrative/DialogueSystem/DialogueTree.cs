using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

// This class handles the data and method of the actual dialogue tree as a scriptable object.
[CreateAssetMenu()]
public class DialogueTree : ScriptableObject
{
    public Node rootNode;
    public Node.NodeState treeState = Node.NodeState.Running;
    public List<Node> nodes = new List<Node>();

    public Node.NodeState UpdateTree()
    {
        if(rootNode.state == Node.NodeState.Running)
        {
            treeState = rootNode.UpdateNode();
        }

        return treeState;
    }

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

    public void AddChild(Node parent, Node child)
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

        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            rootNode.child = child;
        }
    }

    public void RemoveChild(Node parent, Node child)
    {

        SequenceNode sequenceNode = parent as SequenceNode;
        if (sequenceNode)
        {
            sequenceNode.children.Remove(child);
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            rootNode.child = null;
        }
    }

    public List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();
        SequenceNode sequenceNode = parent as SequenceNode;
        if (sequenceNode)
        {
           return sequenceNode.children;
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
