using System;
using System.Collections.Generic;
using System.Linq;
using Balancy;
using Balancy.Data;
using Balancy.Models.BigTitShop;
using Balancy.Models.SmartObjects;
using Cysharp.Threading.Tasks;
using Yurowm.Core;
using Yurowm.DebugTools;
using Yurowm.Extensions;
using Yurowm.Serialization;
using Yurowm.UI;
using Yurowm.Utilities;
using BMChatData = Balancy.Models.ChatData;

namespace LustTicTitsToe {
    public class PlayerData {

        public static PlayerData Instance = new();
        
        ChatsData chatsData;
        GeneratorProgressData generatorData;
        
        public async UniTask Initialize() {
            SmartStorage.LoadSmartObject<ChatsData>(result => {
                chatsData = result.Data;
            });
            
            SmartStorage.LoadSmartObject<GeneratorProgressData>(result => {
                generatorData = result.Data;
            });

            await UniTask.WaitUntil(() => chatsData != null);

            energyItem = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().ENERGY.UnnyId);
            condomItem = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().CONDOM.UnnyId);
            
            DebugPanel.Log("Reset profile", "Cheats", Reset);
        }

        public void Reset() {
            LiveOps.Profile.Restart();
            App.data.Clear();
        }
        
        Item energyItem;
        Item condomItem;
        
        public int Energy {
            get => LiveOps.Profile.Inventories.Currencies.GetTotalAmountOfItems(energyItem);
            set => LiveOps.Profile.Inventories.Currencies.AddItems(energyItem, value - Energy);
        }
        
        public int Condom {
            get => LiveOps.Profile.Inventories.Currencies.GetTotalAmountOfItems(condomItem);
            set => LiveOps.Profile.Inventories.Currencies.AddItems(condomItem, value - Condom);
        }

        public bool SpendEnergy(int cost) {
            if (Energy >= cost) {
                Energy -= cost;
                return true;
            }

            return false;
        }

        public bool SpendCondom(int cost) {
            if (Condom >= cost) {
                Condom -= cost;
                return true;
            }

            return false;
        }

        #region Chats

        public bool HasNewChat =>
            chatsData.History.Chats.Any(c => c.Messages.Count == 0);

        public bool HasChats => chatsData.History.Chats.Count > 0;

        public Chat GetChat(string ID) => 
            chatsData.History.Chats.Find(c => c.ID == ID);

        public void NewChat(string ID) {
            var chat = new Chat { ID = ID };
            chat.Init();
            chatsData.History.Chats.Add(chat);
        }
        
        public void OpenNextChat() {
            var nextChat = ChatData.Instance.GetChats()
                .FirstOrDefault(c => Instance.GetChat(c.ID) == null);
                
            if (nextChat != null) 
                Instance.NewChat(nextChat.ID);
        }

        public void OnActivateGenerator(string id) {
            if (chatsData.History.GeneratorProgress.Contains(id))
                return;
            
            chatsData.History.GeneratorProgress.Add(id);
            
            var genCount = chatsData.History.GeneratorProgress.Count;
            
            if (genCount > 0 && chatsData.History.Chats.Count < genCount / 2)
                OpenNextChat();
        }

        #endregion
    }
    
    
    public class ChatData {

        public static ChatData Instance = new();
        
        BMChatData data;
        
        List<LustChat> chats;

        public void Initialize() {
            chats = new ();
            
            DataEditor.ChatData.ForEach(cd => {
                if (!cd.Available) return;

                // var chat = Serializator.FromTextData<LustChat>(cd.Script.Decrypt());
               var chat = LustChat.storage.GetItemByID(cd.Name);
                
                if (chat == null) return;
                
                chat.ID = cd.Name;
                chat.order = cd.Order;
                chat.cloudData = cd;
                
                chats.Add(chat);
            });
            
            chats.SortBy(c => c.order);
        }
        
        public LustChat GetChat(string ID) => chats?.FirstOrDefault(c => c.ID == ID);
        
        public IEnumerable<LustChat> GetChats() {
            foreach (var chat in chats)
                yield return chat;
        }
        
        public IEnumerable<LustChat> GetOpenChats() {
            return GetChats().Where(c => PlayerData.Instance.GetChat(c.ID) != null);
        }
    }
    
    public static class Extensions {
        public static bool IsShown(this Chat chat, string message) {
            return chat.Messages.Contains(message);
        }
        
        public static void AddIfNew<T>(this SmartList<T> list, T item) {
            if (!list.Contains(item))
                list.Add(item);
        }
        
        public static T Find<T>(this SmartList<T> list, Predicate<T> match) {
            var index = list.FindIndex(match);
            
            if (index < 0)
                return default;
            
            return list[index];
        }
        
        public static bool Any<T>(this SmartList<T> list, Predicate<T> match) {
            return list.FindIndex(match) >= 0;
        }
    }
}