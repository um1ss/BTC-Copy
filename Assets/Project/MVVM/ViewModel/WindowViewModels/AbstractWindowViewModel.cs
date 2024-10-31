using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public abstract class AbstractWindowViewModel<T> where T : AbstractWindowView 
{
    [Inject] private IAssetProvider _assetProvider;
    [Inject] private SoundAudioManager _soundAudioManager;

    protected T _view;

    private T _viewPrefab;
    protected abstract void OpenAnotherWindowView(WindowTypes type);
    protected virtual void CreateView()
    {
        if (_viewPrefab != null)
        {
            _view = GameObject.Instantiate(_viewPrefab);
            _view.Initialize();
            _view.OnClickButton += _soundAudioManager.PlayButtonClip;
            SubscribeView();
        }
    }
    protected abstract void SubscribeView();
    protected async UniTask LoadView(string windowName)
    {
        var viewObj = await _assetProvider.Load<GameObject>(windowName);
        _viewPrefab = viewObj.GetComponent<T>();
    }
    protected virtual void DestroyView()
    {
        _view.OnClickButton -= _soundAudioManager.PlayButtonClip;
        _view.DestroyView();
        _view = null;
    }
}
