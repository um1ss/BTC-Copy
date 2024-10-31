using System.Linq;
using UnityEditor;
using UnityEngine;
using Yurowm.Extensions;
using Yurowm.Nodes.Editor;
using Yurowm.ObjectEditors;
using Yurowm.Serialization;
using Yurowm.Spaces;
using Yurowm.Utilities;

namespace LustTicTitsToe.Editor {
    public class LustChatEditor: ObjectEditor<LustChat> {
        public override void OnGUI(LustChat chat, object context = null) {
            chat.order = EditorGUILayout.IntField("Order", chat.order);
            if (GUILayout.Button("Edit"))
                NodeSystemEditorWindow.Show(chat, e =>
                    e.onContextMenu += m => {
                        OnContextMenu(m, chat);
                    });
            if (GUILayout.Button("To Balancy Script")) {
                EditorGUIUtility.systemCopyBuffer = chat.ToRaw().Encrypt();
            }
            if (GUILayout.Button("From Balancy Script")) {
                var model = Serializator.FromTextData<LustChat>(EditorGUIUtility.systemCopyBuffer.Decrypt());
                if (model == null)
                    return;
                chat.nodes = model.nodes;
                chat.connections = model.connections;
            }
        }

        private void OnContextMenu(GenericMenu menu, LustChat chat) {
            menu.AddItem(new GUIContent("Clear messages"), false, () => 
                chat.nodes.CastIfPossible<LustChat.Node>().ForEach(n => n.ClearMessage()));
            menu.AddItem(new GUIContent("Fill template"), false, () => 
                FillTemplate(chat));
        }
        
        void FillTemplate(LustChat chat) {
            var raw = EditorGUIUtility.systemCopyBuffer;
            if (raw.IsNullOrEmpty())
                return;
            
            var lines = raw
                .Split('\n', '\t')
                .Select(l => l.Trim())
                .Where(l => !l.IsNullOrEmpty())
                .ToArray();
            
            chat.nodes.CastIfPossible<LustChat.Node>().ForEach(n => n.FillMessageTemplate(lines));
        }

        public static Color HerReplyColor = new(1f, 0.58f, 0.88f);
        public static Color MyReplyColor = new(0.48f, 0.89f, 1f);
    }
}