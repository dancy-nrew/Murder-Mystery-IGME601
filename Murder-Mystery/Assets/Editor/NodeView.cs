using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

// Adapted from https://youtu.be/nKpM98I7PeM?si=6_zO-Egnx1kB-9Ys
// This class handles the representation of a node in the graph view
public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Node node;
    public Port input;
    public List<Port> outputs = new List<Port>();
    public Action<NodeView> OnNodeSelected;

   public NodeView(Node node)
   {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutputPorts();
   }

    private void CreateInputPorts()
    {
        if(node is SequenceNode)
        {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if(node is DialogueNode)
        {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if(node is ChoiceNode)
        {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if(node is RootNode)
        {

        }

        if(input != null)
        {
            input.portName = "";
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        if (node is DialogueNode)
        {

        }
        else if (node is SequenceNode)
        {
            Port output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            output.portName = "";
            outputs.Add(output);
        }
        else if(node is ChoiceNode)
        {
            Port output1 = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            output1.portName = "True";
            outputs.Add(output1);
            Port output2 = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            output2.portName = "False";
            outputs.Add(output2);
        }
        else if(node is RootNode)
        {
            Port output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            output.portName = "";
            outputs.Add(output);
        }

        foreach(var port in outputs)
        {
            outputContainer.Add(port);
        }
        
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);

        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
    }

    public override void OnSelected()
    {
        base.OnSelected();

        if(OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }
}
