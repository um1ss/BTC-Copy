using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Image))]
public class FadeAnimation : AbstractAnimation
{
    private Image _image;

    private Color _fadeColor;
    private Color _normalColor;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    private void Start()
    {
        _normalColor = new Color(_image.color.r, _image.color.g, _image.color.b);

        _normalColor.a = 1;
        _fadeColor = new Color(_normalColor.r, _normalColor.g, _normalColor.b);
        _fadeColor.a = 0;
    }

    public async UniTask FadeIn()
    {
        _image.color = _fadeColor;

        _animTween.Kill();
        _animIsPlayind = true;
        _animTween = _image.DOFade(1, _duration).SetEase(Ease.Linear).OnComplete(() => 
        {
            _animIsPlayind = false;
            _image.color = _normalColor;
        });

        while (_animIsPlayind) await UniTask.Yield();
    }
    public async UniTask FadeOut()
    {
        _image.color = _normalColor;

        _animTween.Kill();
        _animIsPlayind = true;
        _animTween = _image.DOFade(0, _duration).SetEase(Ease.Linear).OnComplete(() => 
        {
            _animIsPlayind = false;
            _image.color = _fadeColor;
        });

        while (_animIsPlayind) await UniTask.Yield();
    }
}
