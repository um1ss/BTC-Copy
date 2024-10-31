using System;
using UnityEngine;
using UnityEngine.UI;
using Yurowm.ContentManager;
using Yurowm.Extensions;

namespace LustTicTitsToe {
    public class LustChatUserButton : ContextedBehaviour {
        public Image picture;
        public GameObject borderInactive;
        
        public GameObject border;

        public Yurowm.Button button;
        
        bool selected = false;
        bool available = false;
        
        public bool isAvailable => available;

        public void SetSelected(bool selected) {
            this.selected = selected;
            SetPictureState(available, selected);
        }
        
        public void SetAvailable(bool available) {
            this.available = available;
            SetPictureState(available, selected);
            button.interactable = available;
            border.SetActive(available);
            borderInactive.SetActive(!available);
        }
        
        void SetPictureState(bool available, bool selected) {
            if (selected)
                picture.color = Color.white;
            else if (available)
                picture.color = Color.white.Transparent(0.7f);
            else
                picture.color = Color.white.Transparent(0.3f);
        }

        public void SetAction(Action action) { 
            if (action != null)
                button?.SetAction(action);
        }
    }
}