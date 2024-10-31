using UnityEngine.UI;
using UnityEngine;
using Balancy.Models.BigTitShop;
using Balancy.Models.LiveOps.Store;
using Balancy;
using TMPro;
using UnityEngine.Events;

public class GirlShopSlot : MonoBehaviour
{
    [SerializeField] private ShopButton _buyButton;
    [SerializeField] private Button _openGirlImageButton;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Image _background;
    [SerializeField] private Image _buttonIcon;
    [SerializeField] private TMP_Text _buyText;

    private Slot _slot;
    private Sprite _girlImage;
    public UnityAction<Sprite> OnOpenGirl;
    public UnityAction OnClick;

    private void Start()
    {
        _buyButton.OnSuccessBuy += () =>
        {
            InitActiveSlot();
            OnOpenGirl?.Invoke(_girlImage);
            OnClick?.Invoke();
        };
        _buyButton.OnUnsuccessBuy += () =>
        {
            OnClick?.Invoke();
        };
        _openGirlImageButton.onClick.AddListener(() => OnOpenGirl?.Invoke(_girlImage));
    }
    public void Init(Slot slot)
    {
        _slot = slot;
        var storeItem = slot.GetStoreItem();
        if (storeItem is HardGirlStoreItem girlItem)
        {
            girlItem.GirlItem.Icon.LoadSprite(sprite => _girlImage = sprite);
            var item = LiveOps.Profile.Inventories.Items.GetTotalAmountOfItems(girlItem.GirlItem);
            if (item > 0)
            {
                InitActiveSlot();
            }
            else
            {
                _buyText.gameObject.SetActive(true);
                _buttonImage.raycastTarget = false;
                _openGirlImageButton.gameObject.SetActive(false);
                girlItem.BlureSprite.LoadSprite(sprite => _background.sprite = sprite);
                var ui = (slot as BigTitsShopSlot)?.UIData;
                ui.CoinIcon.LoadSprite(sprite => _buttonIcon.sprite = sprite);
                _buyButton.Init(slot);
            }
        }
    }
    private void InitActiveSlot()
    {
        if (_slot != null)
        {
            _buyText.gameObject.SetActive(false);
            _buttonImage.raycastTarget = true;
            _openGirlImageButton.gameObject.SetActive(true);
            _buyButton.SetInteractble(false);
            var storeItem = _slot.GetStoreItem();
            _buttonIcon.gameObject.SetActive(false);
            if (storeItem is HardGirlStoreItem girlItem)
            {
                girlItem.Sprite.LoadSprite(sprite => _background.sprite = sprite);
            }
        }
    }
}
