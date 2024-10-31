using System.Collections;
using Yurowm.Coroutines;
using Yurowm.Nodes;
using Yurowm.Utilities;

namespace LustTicTitsToe {
    public class LustChatStart : LustChat.Node {
        public readonly Port startPort = new Port(0, "Start", Port.Info.Output, Side.Bottom);

        public override IEnumerable GetPorts() {
            yield return startPort;
        }

        public void Start() {
            Logic().Run(coroutine);
        }

        public IEnumerator Logic() {
            chat.context.GetArgument<ChatPopupView>()?.SetTitle(chat.ID);
            
            Push(startPort);
            yield break;   
        }
    }
}