using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWindow : MonoBehaviour
{
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _closeWindowButton;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    private void Awake()
    {
        _closeWindowButton.onClick.AddListener(CloseWindow);
    }
    public Button UpdateButton => _upgradeButton;
    public Button CloseButton => _closeWindowButton;
    public void OpenWindow(bool interacteble, string title, string cost)
    {
        gameObject.SetActive(true);
        _upgradeButton.interactable = interacteble;
        _costText.text = cost;
        _descriptionText.text = title;
    }
    public void CloseWindow()
    {
        _upgradeButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
