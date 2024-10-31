using UnityEditor;
using UnityEngine;
using Yurowm;
using Yurowm.GUIHelpers;
using Yurowm.ObjectEditors;
using Yurowm.Nodes.Editor;

namespace LustTicTitsToe.Editor {
    public class LustChatReplyEditor : NodeEditor<LustChatReply> {
        public override void OnNodeGUI(LustChatReply node, NodeSystemEditor editor = null) {
            using var cc = GUIHelper.ContentColor.Start(LustChatEditor.MyReplyColor);

            node.cost = EditorGUILayout.IntField("Cost", node.cost.ClampMin(0));
            
            using (GUIHelper.EditorLabelWidth.Start(50)) 
                ObjectEditor.EditStringList(null, node.replies);
        }

        public override void OnParametersGUI(LustChatReply node, NodeSystemEditor editor = null) {
            OnNodeGUI(node);
        }
    }
}