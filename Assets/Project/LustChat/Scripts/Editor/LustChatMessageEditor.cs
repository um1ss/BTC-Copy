using UnityEditor;
using UnityEngine;
using Yurowm.GUIHelpers;
using Yurowm.Nodes.Editor;

namespace LustTicTitsToe.Editor {
    public class LustChatMessageEditor : NodeEditor<LustChatMessage> {
        public override void OnNodeGUI(LustChatMessage node, NodeSystemEditor editor = null) {
            using var cc = GUIHelper.ContentColor.Start(LustChatEditor.HerReplyColor);

            node.message = EditorGUILayout.TextArea(node.message, GUILayout.MinHeight(40));
        }

        public override void OnParametersGUI(LustChatMessage node, NodeSystemEditor editor = null) {
            OnNodeGUI(node);
        }

        public override void OnContextMenu(LustChatMessage node, GenericMenu menu, NodeSystemEditor editor = null) {
            base.OnContextMenu(node, menu, editor);
            if (editor == null)
                return;
            
            menu.AddItem(new GUIContent("To Reply Short"), false, () => {
                editor.Transform(node, new LustChatReplyShort {
                    message = node.message
                });
            });
        }
    }
}