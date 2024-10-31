using System.Collections.Generic;
using Project.Tutorial;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CollectionPopupView : AbstractWindowView
{
    [SerializeField] private RectTransform _window;
    [SerializeField] private CollectionCard _collectionCardPrefab;

    [SerializeField] private Button _closeWindowButton;
    [SerializeField] private TextMeshProUGUI _openObjectText;

    [SerializeField] private RectTransform _cardContentRect;

    [SerializeField] private Color _offColor;
    [SerializeField] private Color _oddColor;
    [SerializeField] private Color _evenColor;

    private Dictionary<string, CollectionCard> _createdCards = new();
    private int _offCardNumber;

    public override void Initialize()
    {
        _buttons = new()
        {
            { AppConstants.CloseWindowButtonName, _closeWindowButton },
        };
    }
    public void CreateCard(IGenerator generator, UnityAction<string> cardUpgradeCallback)
    {
        var card = Instantiate(_collectionCardPrefab, _cardContentRect);
        generator.OnActivateGenerate += UnlockCard;
        _createdCards.Add(generator.Name, card);

        int cardLvl = generator.CurrentLvl;
        bool isOdd = _createdCards.Count % 2 == 0;
        Color cardColor = isOdd ? _oddColor : _evenColor;

        if (cardLvl == 0)
        {
            _offCardNumber++;
            cardColor = _offColor;
        }

        card.InitCard(generator.Icon, cardColor, generator.Name, cardLvl, IntFormatConverter.FormatInt(generator.UpdateCost.ToString()), isOdd);

        card.SubscribeLvlUpButton(() => cardUpgradeCallback?.Invoke(generator.Name));

        _openObjectText.text = $"Open Object {_createdCards.Count - _offCardNumber}/{_createdCards.Count}";

        if (generator.Id == 0) {
            var elementId = card.AddComponent<HighlightElementId>();
            elementId.elementId = HighlightElemId.generatorBuyBtn;
            elementId.onActivate += () => cardUpgradeCallback?.Invoke(generator.Name);
        }
    }
    public void ChangeCardView(string cardName, string lvlText, string updateText, float fill)
    {
        if (_createdCards.TryGetValue(cardName, out var card))
        {
            card.SetLvl(lvlText, fill);
            card.SetUpdateCost(IntFormatConverter.FormatInt(updateText));
        }
    }
    public void UnsubscribeCard(IGenerator generator)
    {
        generator.OnActivateGenerate -= UnlockCard;
    }
    private void UnlockCard(string cardName)
    {
        var card = _createdCards[cardName];
        card.SetColor(card.IsOdd ? _oddColor : _evenColor);
        _offCardNumber--;
        _openObjectText.text = $"Open Object {_createdCards.Count - _offCardNumber}/{_createdCards.Count}";
    }
}
