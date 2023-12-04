using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

// Adapted from https://youtu.be/nKpM98I7PeM?si=6_zO-Egnx1kB-9Ys
// This class represent the graph window representation of a dialogue tree
public class DialogueTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<DialogueTreeView, GraphView.UxmlTraits> { }

    public Action<NodeView> OnNodeSelected;
    DialogueTree currentTree;

    public DialogueTreeView()
    {
        // Adds grid background
        Insert(0, new GridBackground());

        // Adds mouse manipulations to the graph window.
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/DialogeTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    // Construct the graph window from a dialoge tree scriptable object
    public void PopulateView(DialogueTree tree)
    {
        currentTree = tree;

        // Unsubscribing from graph view changed before deleting. Deleting to clear graph.
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if(currentTree.rootNode == null)
        {
            currentTree.rootNode = currentTree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(currentTree);
            AssetDatabase.SaveAssets();
        }

        // Creating nodeViews for each node in the dialogue tree
        currentTree.nodes.ForEach(node => CreateNodeView(node));

        // Creating edges between nodes
        currentTree.nodes.ForEach(node =>
        {
            NodeView parentView = GetNodeViewFromNode(node);
            var children = currentTree.GetChildren(node);
            if (parentView.node is ChoiceNode)
            {
                if(children.Count >0)
                {
                    NodeView childViewTrue = GetNodeViewFromNode(children[0]);
                    Edge edge1 = parentView.outputs[0].ConnectTo(childViewTrue.input);
                    AddElement(edge1);
                }

                if (children.Count > 1)
                {
                    NodeView childViewFalse = GetNodeViewFromNode(children[1]);
                    Edge edge2 = parentView.outputs[1].ConnectTo(childViewFalse.input);
                    AddElement(edge2);
                }  
               
            }
            else
            {
                children.ForEach(c =>
                {

                    NodeView childView = GetNodeViewFromNode(c);

                    Edge edge = parentView.outputs[0].ConnectTo(childView.input);
                    AddElement(edge);

                });
            }
        });
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => 
        endPort.direction != startPort.direction && 
        endPort.node != startPort.node).ToList();
    }

    // This function is called every time there is a change in the viewport
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if(graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                // Checking if node has been deleted
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    currentTree.DeleteNode(nodeView.node);
                }

                // Checking if edge has been deleted
                Edge edge = elem as Edge;
                if(edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    currentTree.RemoveChild(parentView.node, childView.node, edge);
                }
            });
        }

        //Edges to be added
        if (graphViewChange.edgesToCreate != null)
        { 
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                currentTree.AddChild(parentView.node, childView.node, edge);
            });
        }

        return graphViewChange;
    }

    // Function to add actions to menu that pops when user right clicks
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        // base.BuildContextualMenu(evt);
        var types = TypeCache.GetTypesDerivedFrom<Node>();
        foreach(var type in types)
        {
            evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
        }
    }

    private void CreateNode(System.Type type)
    {
        Node node = currentTree.CreateNode(type);
        CreateNodeView(node);
    }

    private void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }

    NodeView GetNodeViewFromNode(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }
}
