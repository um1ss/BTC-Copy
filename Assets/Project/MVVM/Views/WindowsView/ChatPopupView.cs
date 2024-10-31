using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LustTicTitsToe;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yurowm;
using Yurowm.ContentManager;
using Yurowm.Coroutines;
using Yurowm.Extensions;
using Button = UnityEngine.UI.Button;

public class ChatPopupView : AbstractWindowView {
    [SerializeField] private RectTransform _userButtons;
    [SerializeField] private TMP_Text _chatTitle;
    [SerializeField] private RectTransform _chatView;
    [SerializeField] private RectTransform _replyRect;
    [SerializeField] private GameObject _photoViewer;
    [SerializeField] private Image _photoViewerPhoto;

    [Header("Buttons")] 
    [SerializeField] private Button _closeWindowButton;

    [Header("Prefabs")]
    public LustChatEntry herMessagePrefab;
    public LustChatEntry herPhotoPrefab;
    public LustChatEntry herStatusPrefab;
    public LustChatEntry myMessagePrefab;
    public LustChatEntry myReplyPrefab;

    [SerializeField] private LustChatUserButton _userButtonPrefab;
    
    ContentAnimator _animator;
    
    public event Action OnShopRequired;
    
    public override void Initialize() {
        _buttons = new Dictionary<string, Button> {
            { AppConstants.CloseWindowButtonName, _closeWindowButton }
        };

        this.SetupComponent(out _animator);
        
        _photoViewer?.SetActive(false);
        
        FillUsers();
    }

    public void HideView() {
        if (_animator) {
            _animator
                .PlayAndWait("Hide")
                .ContinueWith(() => gameObject.SetActive(false))
                .Run();
        } else
            gameObject.SetActive(false);
    }

    public void ShowView() {
        gameObject.SetActive(true);
        _animator.Play("Show");
        
        ClearReplies();
        UpdateUserButtons();
        ShowFirstChat();
    }

    public void OpenShop() {
        OnShopRequired?.Invoke();
    }

    #region Users

    Dictionary<string, LustChatUserButton> userButtons = null;

    void FillUsers() {
        if (userButtons != null)
            return;

        userButtons = new();
        
        if (!_userButtons || !_userButtonPrefab)
            return;

        foreach (var chat in ChatData.Instance.GetChats()) {
            var userButton = _userButtonPrefab.Clone();
            userButton.transform.SetParent(_userButtons);
            userButton.transform.Reset();

            chat.cloudData.UserPic.LoadSprite(s => userButton.picture.sprite = s);

            userButtons.Add(chat.ID, userButton);
        }
    }

    public void UpdateUserButtons() {
        if (userButtons == null)
            FillUsers();

        foreach (var pair in userButtons) {
            var available = LustTicTitsToe.PlayerData.Instance.GetChat(pair.Key) != null;
            pair.Value.gameObject.SetActive(ChatData.Instance.GetChat(pair.Key).IsAvailable());
            pair.Value.SetAvailable(available);
            pair.Value.SetAction(() => ShowChat(pair.Key));
        }

        int siblingIndex = 0;

        userButtons.OrderByDescending(b => b.Value.isAvailable).ThenBy(b => ChatData.Instance.GetChat(b.Key).order)
            .ForEach(b => b.Value.transform.SetSiblingIndex(siblingIndex++));
    }
    
    void ShowFirstChat() {
        userButtons.Values
            .FirstOrDefault(b => b.isActiveAndEnabled && b.isAvailable)?
            .button?
            .onClick?
            .Invoke();
    }

    #endregion

    public void ShowChat(string ID) {
        ChatData.Instance.GetChats().ForEach(c => c.Stop());
        ChatData.Instance.GetChat(ID)?.Launch(this);

        if (userButtons == null)
            FillUsers();

        foreach (var pair in userButtons)
            pair.Value.SetSelected(pair.Key == ID);
    }
    
    public void SetTitle(string text) {
        if (_chatTitle)
            _chatTitle.text = text;
    }

    #region Status

    LustChatEntry status;

    public void SetStatus(string message, LiveContext context) {
        if (!herStatusPrefab || !_chatView)
            return;

        if (message.IsNullOrEmpty()) {
            if (status) {
                status.Kill();
                status = null;
            }

            return;
        }

        if (!status) {
            status = Emit(herStatusPrefab, context);

            if (_chatView) {
                status.transform.SetParent(_chatView);
                status.transform.Reset();

                LayoutRebuilder.ForceRebuildLayoutImmediate(_chatView);
            }
            else
                status.Kill();
        }

        if (status.SetupChildComponent(out TMP_Text text))
            text.text = message;
    }

    #endregion

    #region Messages

    public void NewMessage(LustChatEntry entry, bool immediate) {
        if (_chatView) {
            entry.transform.SetParent(_chatView);
            entry.transform.Reset();

            LayoutRebuilder.ForceRebuildLayoutImmediate(_chatView);

            if (entry.SetupComponent(out ContentAnimator animator)) {
                if (immediate)
                    animator.RewindEnd("Show");
                else
                    animator.Play("Show");
            }
        }
        else
            entry.Kill();
    }

    #endregion

    #region Replies

    List<LustChatEntry> replies = new();
    Action<int> onReply = delegate { };

    public void NewReply(LustChatEntry entry, int index, bool immediate) {
        if (_replyRect) {
            entry.transform.SetParent(_replyRect);
            entry.transform.Reset();

            replies.Add(entry);

            if (entry.SetupComponent(out Yurowm.Button button))
                button.SetAction(() => onReply?.Invoke(index));

            if (entry.SetupComponent(out ContentAnimator animator)) {
                if (immediate)
                    animator.RewindEnd("Show");
                else
                    animator.Play("Show");
            }

            _replyRect.gameObject.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_replyRect);
        }
        else
            entry.Kill();
    }

    public void ClearReplies() {
        replies.ForEach(r => r.Kill());
        replies.Clear();

        status?.Kill();
        status = null;

        _replyRect.gameObject.SetActive(false);
    }

    public IEnumerator WaitReply(Action<int> func) {
        var wait = true;

        void Reply(int index) {
            func?.Invoke(index);
            wait = false;
        }

        onReply += Reply;

        while (wait)
            yield return null;

        onReply -= Reply;
    }

    #endregion

    #region Photo Viewer
    
    public void ShowPhoto(Sprite photo) {
        if (!photo || !_photoViewer || !_photoViewerPhoto) 
            return;
        
        _photoViewerPhoto.sprite = photo;
        
        _photoViewer.GetComponent<Button>()?.onClick.SetSingleListner(HidePhoto);
        
        _photoViewer.SetActive(true);
        
        if (_photoViewer.SetupComponent(out ContentAnimator animator))
            animator.Play("Show");
    }

    public void HidePhoto() {
        if (!_photoViewer) 
            return;

        if (_photoViewer.SetupComponent(out ContentAnimator animator))
            animator.PlayAndWait("Hide")
                .ContinueWith(() => _photoViewer.SetActive(false))
                .Run();
        else
            _photoViewer.SetActive(false);
    }
    
    #endregion

    public LustChatEntry Emit(LustChatEntry prefab, LiveContext context) {
        var result = prefab.Clone();
        context.Add(result);
        return result;
    }

}