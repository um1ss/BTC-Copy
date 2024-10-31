using UnityEngine;
using UnityEngine.UI;

public class GirlPhoto : MonoBehaviour
{
    [SerializeField] private GameObject _bluer;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Button _button;
    [SerializeField] private Image _imageButton;
    private RectTransform _parentRectTransform;

    private void Awake()
    {
        _button.onClick.AddListener(SetActiveImageFullScreen);
    }

    private void Start()
    {
        _parentRectTransform = transform.root.GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void SetActiveImageFullScreen()
    {
        var image = Instantiate(_prefab, _parentRectTransform);
        var imageFullScreen = image.GetComponent<ImageFullScreen>();
        imageFullScreen.SetImage(_imageButton.sprite);
    }

    public void SetButtonInteract(bool value)
    {
        _button.interactable = value;
    }

    public void TurnOffBluerPhotos(bool turnOff)
    {
        if (turnOff)
            _bluer.SetActive(false);
        else
            _bluer.SetActive(true);
    }
}