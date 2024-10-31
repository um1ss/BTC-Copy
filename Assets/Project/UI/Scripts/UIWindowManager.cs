using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using VContainer;

public class UIWindowManager 
{
    private Dictionary<WindowTypes, IWindow> _screensMap;

    [Inject]
    public UIWindowManager(IReadOnlyList<IWindow> screens)
    {
        foreach (var screen in screens)
        {
            screen.OnAnotherWindowOpen += ShowWindow;
        }
        _screensMap = screens.ToDictionary(e => e.Type, e => e);
    }
    public async UniTask InitWindows()
    {
        foreach (var screen in _screensMap)
        {
            await screen.Value.Initialize();
        }
        ShowWindow(WindowTypes.Main);
    }
    private void ShowWindow(WindowTypes type)
    {
        if (_screensMap.TryGetValue(type, out var screen))
        {
            screen.Show();
        }
    }
}
