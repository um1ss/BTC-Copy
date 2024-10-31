using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Yurowm.ContentManager;
using Yurowm.Coroutines;
using Yurowm.Extensions;
using Yurowm.Nodes;
using Yurowm.Serialization;
using Yurowm.UI;
using Yurowm.Utilities;

namespace LustTicTitsToe {
    public class LustChatReply : LustChat.Node {
        public readonly Port triggerPort = new(0, "Trigger", Port.Info.Input, Side.Top);
        
        public int cost = 0;
        
        public List<string> replies = new ();
        
        Dictionary<int, Port> replyPorts = new();

        public Port GetReplyPort(int index) {
            if (replyPorts.TryGetValue(index, out var result))
                return result;
            
            result = new Port(index + 1, (index + 1).ToString(), Port.Info.Output, Side.Right);
            replyPorts[index] = result;
            return result;
        }
        
        public override IEnumerable GetPorts() {
            yield return triggerPort;
            for (int i = 0; i < 5; i++)
                yield return GetReplyPort(i);
        }
        
        public override void OnPortPushed(Port sourcePort, Port targetPort, object[] args) {
            if (targetPort == triggerPort)
                OnTrigger(args);   
        }
        
        public virtual void OnTrigger(object[] args) {
            Logic(args).Run(coroutine);
        }
        
        public IEnumerator Logic(object[] args) {
            var view = chat.context.GetArgument<ChatPopupView>();
            
            view.ClearReplies();
            
            var story = PlayerData.Instance.GetChat(chat.ID);
            
            if (story == null)
                yield break;
         
            var messageAsset = view.myMessagePrefab;
            
            var index = GetShownIndex();
            var immediate = index.HasValue;
            
            if (!immediate) {
                
                var replyAsset = view.myReplyPrefab;
                if (replyAsset) {
                    var waitTime = (chat.lastMessageTime ?? DateTime.MinValue) 
                                   + TimeSpan.FromSeconds(0.3f);
                    
                    var rewind = waitTime < DateTime.Now;
                    
                    yield return new WaitTimeSpan(waitTime);
                    
                    for (int i = 0; i < replies.Count; i++) {
                        var reply = view.Emit(replyAsset, chat.context);
                        if (reply.SetupChildComponent(out TMP_Text replyText)) {
                            var t = replies.Get(i) ?? "";
                            if (cost > 0)
                                t = $"{cost.ToString().Style("Condom")} {t}";
                            replyText.text = t;
                        }
            
                        view.NewReply(reply, i, rewind);
                    }

                    while (true) {
                        yield return view.WaitReply(i => index = i);

                        if (cost <= 0) break;
                        
                        if (!PlayerData.Instance.SpendCondom(cost)) {
                            view.OpenShop();
                        } else {
                            story.Spend += cost;
                            UIRefresh.InvokeDelayed();
                            break;
                        }
                    }
                    
                    view.ClearReplies();
                    
                    chat.lastMessageTime = DateTime.Now;
                } else 
                    index = 0;
            }
            
            var entry = view.Emit(messageAsset, chat.context);
            if (entry.SetupChildComponent(out TMP_Text text))
                text.text = replies.Get(index.Value) ?? "";
            
            view.NewMessage(entry, immediate);
            story.Messages.AddIfNew(ID.ToString());
            story.Messages.AddIfNew($"{ID}:{index.Value}");

            Push(GetReplyPort(index.Value));
        }
        
        int? GetShownIndex() {
            var story = PlayerData.Instance.GetChat(chat.ID);
            
            if (!story.IsShown(ID.ToString()))
                return null;
            
            var replyID = story.Messages
                .Find(id => id.StartsWith($"{ID}:"));
            
            if (replyID.IsNullOrEmpty())
                return null;
                
            if (!int.TryParse(replyID[(replyID.IndexOf(":") + 1)..], out var index))
                return null;
            
            return index;
        }

        public override void ClearMessage() {
            replies = Enumerator
                .For(1, replies.Count, 1)
                .Select(_ => "")
                .ToList();
        }

        public override void FillMessageTemplate(string[] lines) {
            for (int i = 0; i < replies.Count; i++) {
                if (int.TryParse(replies[i], out var index)) {
                    var line = lines.Get(index); 
                    if (!line.IsNullOrEmpty())
                        replies[i] = line;
                }
            }
        }

        public override void Serialize(IWriter writer) {
            base.Serialize(writer);
            writer.Write("cost", cost);
            writer.Write("replies", replies);
        }

        public override void Deserialize(IReader reader) {
            base.Deserialize(reader);
            reader.Read("cost", ref cost);
            replies.Reuse(reader.ReadCollection<string>("replies"));
        }
    }
}