using Balancy.Models;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using VContainer;

public class UIBackgroundManager 
{
    private Dictionary<BackgroundType, IBackground> _backMap;

    [Inject] private BackgroundsModel _backgroundsModel;

    [Inject]
    public UIBackgroundManager(IReadOnlyList<IBackground> backgrounds)
    {
        _backMap = backgrounds.ToDictionary(e => e.Type, e => e);
    }
    public async UniTask OpenBackground(BackgroundType type)
    {
        if (_backgroundsModel.CurrentBackgroundType != type)
        {
            if (_backMap.TryGetValue(type, out var background))
            {
                await background.Show();
                ClosePreviosBackground();
                _backgroundsModel.SetType(type);
            }
        }
    }
    private void ClosePreviosBackground()
    {
        if (_backgroundsModel.CurrentBackgroundType != BackgroundType.None)
        {
            if (_backMap.TryGetValue(_backgroundsModel.CurrentBackgroundType, out var background))
            {
                background.Close();
            }
        }
    }
}
