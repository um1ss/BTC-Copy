using System;
using System.Collections;
using TMPro;
using Yurowm.ContentManager;
using Yurowm.Coroutines;
using Yurowm.Extensions;
using Yurowm.Serialization;

namespace LustTicTitsToe {
    public class LustChatReplyShort : LustChat.ActionNode {
        public string message;
        
        public override IEnumerator Logic(object[] args) {
            var view = chat.context.GetArgument<ChatPopupView>();
            
            view.ClearReplies();
            
            var story = PlayerData.Instance.GetChat(chat.ID);
            
            if (story == null)
                yield break;
            
            bool IsShown() => PlayerData.Instance.GetChat(chat.ID).IsShown(ID.ToString());
         
            var messageAsset = view.myMessagePrefab;
            
            if (!IsShown()) {
                var replyAsset = view.myReplyPrefab;
                
                if (replyAsset) {
                    var waitTime = (chat.lastMessageTime ?? DateTime.MinValue) 
                                   + TimeSpan.FromSeconds(0.3f);
                    
                    var rewind = waitTime < DateTime.Now;
                    
                    yield return new WaitTimeSpan(waitTime);
                    
                    var reply = view.Emit(replyAsset, chat.context);
                    if (reply.SetupChildComponent(out TMP_Text replyText)) 
                        replyText.text = message;

                    view.NewReply(reply, 0, rewind);

                    yield return view.WaitReply(null);

                    view.ClearReplies();
                    
                    chat.lastMessageTime = DateTime.Now;
                }
            }
            
            var entry = view.Emit(messageAsset, chat.context);
            if (entry.SetupChildComponent(out TMP_Text text))
                text.text = message;
            
            view.NewMessage(entry, IsShown());
            
            story.Messages.AddIfNew(ID.ToString());

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