using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CollectionCard : MonoBehaviour
{
    [SerializeField] private Button _lvlUpButton;
    [SerializeField] private Image _mainImage;
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _headText;
    [SerializeField] private TextMeshProUGUI _lvlProgressText;
    [SerializeField] private TextMeshProUGUI _earnText;

    [Space]
    [SerializeField] private Image _fillLvlProhress;
    [SerializeField] private Image _starsProgress;

    private bool _isOdd;

    public bool IsOdd => _isOdd;
    public void InitCard(Sprite mainImage, Color backgroundColor, string head, int lvl, string income, bool isOdd)
    {
        _mainImage.sprite = mainImage;
        _headText.text = head;
        _isOdd = isOdd;

        SetColor(backgroundColor);
        SetUpdateCost(income);
        SetLvl($"{lvl}/100", (float)lvl/100);
    }
    public void SetLvl(string lvl, float fill)
    {
        _lvlProgressText.text = lvl;
        _fillLvlProhress.fillAmount = fill;
        _starsProgress.fillAmount = fill;
    }
    public void SetUpdateCost(string income)
    {
        _earnText.text = income;
    }
    public void SetColor(Color color)
    {
        _background.color = color;
    }
    public void SubscribeLvlUpButton(UnityAction action)
    {
        _lvlUpButton.onClick.AddListener(action);
    }
}
