using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Balancy.Models.BigTitShop;

public class ShopSectionButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;

    private ShopSectionType _type;
    private UnityAction<ShopSectionType> _buttonAction;
    public void Init(BigTitsShopSectionButton data, UnityAction<ShopSectionType> action)
    {
        _button.onClick.AddListener(OnClick);
        _buttonAction = action;
        _type = data.Type;
        data.Icon.LoadSprite(sprite => _icon.sprite = sprite);
        data.Background.LoadSprite(sprite => _background.sprite = sprite);
    }
    private void OnClick()
    {
        _buttonAction?.Invoke(_type);
    }
}
