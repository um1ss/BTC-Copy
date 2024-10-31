using UnityEditor;
using UnityEngine;
using Yurowm;
using Yurowm.GUIHelpers;
using Yurowm.ObjectEditors;
using Yurowm.Nodes.Editor;

namespace LustTicTitsToe.Editor {
    public class LustChatReplyShortEditor : NodeEditor<LustChatReplyShort> {
        public override void OnNodeGUI(LustChatReplyShort node, NodeSystemEditor editor = null) {
            using var cc = GUIHelper.ContentColor.Start(LustChatEditor.MyReplyColor);
       
            node.message = EditorGUILayout.TextArea(node.message, GUILayout.MinHeight(40));
        }

        public override void OnParametersGUI(LustChatReplyShort node, NodeSystemEditor editor = null) {
            OnNodeGUI(node);
        }

        public override void OnContextMenu(LustChatReplyShort node, GenericMenu menu, NodeSystemEditor editor = null) {
            base.OnContextMenu(node, menu, editor);
            if (editor == null)
                return;
            
            menu.AddItem(new GUIContent("To Message"), false, () => {
                editor.Transform(node, new LustChatMessage {
                    message = node.message
                });
            });
        }
    }
}