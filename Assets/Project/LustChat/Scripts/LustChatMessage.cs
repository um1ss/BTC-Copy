using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Yurowm.ContentManager;
using Yurowm.Coroutines;
using Yurowm.Extensions;
using Yurowm.Serialization;
using Yurowm.Utilities;

namespace LustTicTitsToe {
    public class LustChatMessage : LustChat.ActionNode {
        public string message;
        
        public override IEnumerator Logic(object[] args) {
            var view = chat.context.GetArgument<ChatPopupView>();
            
            var story = PlayerData.Instance.GetChat(chat.ID);
            
            if (story == null)
                yield break;
         
            var asset = view.herMessagePrefab;
            
            if (asset) {

                var immediate = story.IsShown(ID.ToString());
                
                if (!immediate) {
                    yield return new Wait(0.2f);
                    
                    view.SetStatus($"{chat.ID} is typing...", chat.context);
                    
                    var waitTime = (chat.lastMessageTime ?? DateTime.MinValue) 
                                + TimeSpan.FromSeconds(YRandom.main.Range(3, 5));
                    
                    yield return new WaitTimeSpan(waitTime);
                    
                    view.SetStatus(null, chat.context);
                    
                    chat.lastMessageTime = DateTime.Now;
                }
                
                var entry = view.Emit(asset, chat.context);
                if (entry.SetupChildComponent(out TMP_Text text))
                    text.text = message;
                
                view.NewMessage(entry, immediate);
                story.Messages.AddIfNew(ID.ToString());
            }

            Push(outputPort);
        }

        public override void ClearMessage() {
            message = "";
        }
        
        public override void FillMessageTemplate(string[] lines) {
            if (int.TryParse(message, out var index)) {
                var line = lines.Get(index); 
                if (!line.IsNullOrEmpty())
                    message = line;
            }
        }

        public override void Serialize(IWriter writer) {
            base.Serialize(writer);
            writer.Write("message", message);
        }

        public override void Deserialize(IReader reader) {
            base.Deserialize(reader);
            reader.Read("message", ref message);
        }
    }
}