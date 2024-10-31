using UnityEditor;
using UnityEngine;
using Yurowm;
using Yurowm.GUIHelpers;
using Yurowm.ObjectEditors;
using Yurowm.Nodes.Editor;
using Yurowm.Spaces;

namespace LustTicTitsToe.Editor {
    public class LustChatPhotoEditor : NodeEditor<LustChatPhoto> {
        public override void OnNodeGUI(LustChatPhoto node, NodeSystemEditor editor = null) {
            using var cc = GUIHelper.ContentColor.Start(LustChatEditor.HerReplyColor);
            
            node.photoID = EditorGUILayout.IntField("PhotoID", node.photoID);
        }

        public override void OnParametersGUI(LustChatPhoto node, NodeSystemEditor editor = null) {
            OnNodeGUI(node);
        }
    }
}