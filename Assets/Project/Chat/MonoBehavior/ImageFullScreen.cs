using UnityEngine;
using UnityEngine.UI;

public class ImageFullScreen : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    private void Awake()
    {
        _button.onClick.AddListener(DisableImageFullScreen);
    }
    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void DisableImageFullScreen()
    {
        Destroy(gameObject);
    }

    public void SetImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}
