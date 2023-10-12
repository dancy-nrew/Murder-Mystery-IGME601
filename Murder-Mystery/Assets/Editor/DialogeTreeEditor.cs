using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogeTreeEditor : EditorWindow
{
    /* [SerializeField]
     private VisualTreeAsset m_VisualTreeAsset = default;*/

    DialogueTreeView treeView;
    InspectorView inspectorView;

    [MenuItem("Window/UI Toolkit/DialogeTreeEditor")]
    public static void ShowExample()
    {
        DialogeTreeEditor wnd = GetWindow<DialogeTreeEditor>();
        wnd.titleContent = new GUIContent("DialogeTreeEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
/*        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);*/

        // Instantiate UXML
/*        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);*/

        var visualTree =  AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/DialogeTreeEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/DialogeTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<DialogueTreeView>();
        inspectorView = root.Q<InspectorView>();
        treeView.OnNodeSelected = OnNodeSelectionChanged;
        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        DialogueTree  tree = Selection.activeObject as DialogueTree;
        if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
        {
            treeView.PopulateView(tree);
        }
    }

    private void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateSelection(node);
    }
}
