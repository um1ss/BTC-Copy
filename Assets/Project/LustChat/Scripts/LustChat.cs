using System;
using System.Collections;
using System.Collections.Generic;
using Yurowm;
using Yurowm.ContentManager;
using Yurowm.Core;
using Yurowm.Coroutines;
using Yurowm.Extensions;
using Yurowm.Nodes;
using Yurowm.Serialization;
using Yurowm.UI;
using Yurowm.Utilities;

namespace LustTicTitsToe {
    public class LustChat: NodeSystem, ISerializableID {
        
        [PreloadStorage]
        public static readonly Storage<LustChat> storage = new("LustChats", TextCatalog.StreamingAssets);

        public string ID { get; set; }
        public int order;
        
        public override IEnumerable<Type> GetSupportedNodeTypes() {
            yield return typeof(BasicNode);
            yield return typeof(Node);
        }

        public LiveContext context;
        public CoroutineCore coroutine;
        public DateTime? lastMessageTime = null;
        
        public Balancy.Models.ChatData cloudData;

        public bool IsAvailable() => true;

        public void Stop() {
            coroutine?.Destroy();
            context?.Destroy();
        }

        public void Launch(ChatPopupView view) {
            coroutine?.Destroy();
            coroutine = new ();
            coroutine.Logic(CoroutineCore.Loop.Update).Run();
            
            context?.Destroy();
            context = new LiveContext($"Chat: {ID}");
            context.SetArgument(view);
            context.SetArgument(cloudData);
            
            
            foreach (var node in nodes.CastIfPossible<Node>()) 
                node.chat = this;

            nodes
                .CastOne<LustChatStart>()?
                .Start();
            
            UIRefresh.InvokeDelayed();
        }

        public override void Serialize(IWriter writer) {
            writer.Write("ID", ID);
            writer.Write("order", order);
            base.Serialize(writer);
        }

        public override void Deserialize(IReader reader) {
            ID = reader.Read<string>("ID");
            reader.Read("order", ref order);
            base.Deserialize(reader);
        }
        
        public abstract class Node : Yurowm.Nodes.Node {
            public LustChat chat;
            public CoroutineCore coroutine => chat?.coroutine;

            public virtual void ClearMessage() { }

            public virtual void FillMessageTemplate(string[] lines) { }
        }

        public abstract class ActionNode : Node {
            public readonly Port triggerPort = new(0, "Trigger", Port.Info.Input, Side.Top);
            public readonly Port outputPort = new(1, "Output", Port.Info.Output, Side.Bottom);
        
            public override IEnumerable GetPorts() {
                yield return triggerPort;
                yield return outputPort;
            }
        
            public override void OnPortPushed(Port sourcePort, Port targetPort, object[] args) {
                if (targetPort == triggerPort)
                    OnTrigger(args);   
            }
        
            public abstract IEnumerator Logic(object[] args);

            public virtual void OnTrigger(object[] args) {
                Logic(args).Run(coroutine);
            }
        }
    }
}