using Balancy.Models.BigTitShop;
using Balancy.Models.LiveOps.Store;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private ShopButton _buyButton;

    [SerializeField] private TextMeshProUGUI _rewardText;

    [SerializeField] private Image _mainIcon;
    [SerializeField] private Image _miniIcon;
    [SerializeField] private Image _background;
    [SerializeField] private Image _buttonIcon;

    public UnityAction OnClick;

    private void Start()
    {
        _buyButton.OnSuccessBuy += () =>
        {
            OnClick?.Invoke();
        };
        _buyButton.OnUnsuccessBuy += () =>
        {
            OnClick?.Invoke();
        };
    }
    public void Init(Slot slot)
    {
        _buyButton.Init(slot);
        var ui = (slot as BigTitsShopSlot)?.UIData;
        ui.Background.LoadSprite(sprite => _background.sprite = sprite);
        ui.CoinIcon.LoadSprite(sprite => _buttonIcon.sprite = sprite);

        var item = slot.GetStoreItem();
        item.Sprite.LoadSprite(sprite => _mainIcon.sprite = sprite);
        var firstRewardItem = item.Reward.Items[0];
        _rewardText.text = firstRewardItem.Count.ToString();

        var storeItem = slot.GetStoreItem();
        if (storeItem is BigTitsStoreItem bigTitsItem)
        {
            bigTitsItem.RewardIcon.LoadSprite(sprite => _miniIcon.sprite = sprite);
        }
    }
}
