using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Yurowm.ContentManager;
using Yurowm.Coroutines;
using Yurowm.Extensions;
using Yurowm.Serialization;
using Yurowm.UI;
using Yurowm.Utilities;

namespace LustTicTitsToe {
    public class LustChatPhoto : LustChat.ActionNode {
        public int photoID;
        
        Sprite photoCache;
        Action onPhotoDownloaded;

        public override IEnumerator Logic(object[] args) {
            var view = chat.context.GetArgument<ChatPopupView>();
            
            var story = PlayerData.Instance.GetChat(chat.ID);
            
            if (story == null)
                yield break;
         
            var photoAsset = view.herPhotoPrefab;
            
            if (photoAsset) {
                
                if (!photoCache)
                    chat.context.GetArgument<Balancy.Models.ChatData>()?
                        .Photos.Get(photoID - 1)?
                        .LoadSprite(s => {
                            photoCache = s;
                            onPhotoDownloaded?.Invoke();
                            onPhotoDownloaded = null;
                        });
                
                var immediate = story.IsShown(ID.ToString());

                if (!immediate) {
                    yield return new Wait(0.2f);
                    
                    view.SetStatus($"{chat.ID} is uploading a photo...", chat.context);
                    
                    var waitTime = (chat.lastMessageTime ?? DateTime.MinValue) 
                                   + TimeSpan.FromSeconds(YRandom.main.Range(3, 5));
                    
                    yield return new WaitTimeSpan(waitTime);
                    
                    view.SetStatus(null, chat.context);
                    
                    chat.lastMessageTime = DateTime.Now;
                }
                
                var entry = view.Emit(photoAsset, chat.context);
                var photoComponenet = entry
                    .GetComponentsInChildren<Image>()
                    .FirstOrDefault(i => i.name == "Photo");
                
                if (photoComponenet) {
                    photoComponenet.pixelsPerUnitMultiplier = 3;
                    if (photoCache)
                        photoComponenet.sprite = photoCache;
                    else 
                        onPhotoDownloaded += () => photoComponenet.sprite = photoCache;
                }
                
                if (entry.SetupChildComponent(out Yurowm.Button button)) {
                    button.SetAction(() => {
                        view.ShowPhoto(photoCache);
                    });
                }
                
                view.NewMessage(entry, immediate);
                story.Messages.AddIfNew(ID.ToString());
            }

            Push(outputPort);
        }

        public override void Serialize(IWriter writer) {
            base.Serialize(writer);
            writer.Write("photoID", photoID);
        }

        public override void Deserialize(IReader reader) {
            base.Deserialize(reader);
            reader.Read("photoID", ref photoID);
        }
    }
}