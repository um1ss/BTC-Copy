using Yurowm.Dashboard;
using Yurowm.Extensions;
using Yurowm.Serialization;

namespace LustTicTitsToe.Editor {
    [DashboardGroup("Content")]
    [DashboardTab("Chats")]
    public class LustChatStorageEditor : StorageEditor<LustChat> {
        public override string GetItemName(LustChat item) {
            return $"{item.order}. {item.ID}";
        }

        public override Storage<LustChat> OpenStorage() => LustChat.storage;

        protected override void Sort() {
            storage.items.SortBy(i => i.order);
        }
    }
}