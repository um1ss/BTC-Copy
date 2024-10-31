using Cysharp.Threading.Tasks;

public class InitializeWindows : ILoadingOperation
{
    private UIWindowManager _uIManager;

    public InitializeWindows(UIWindowManager uIManager)
    {
        _uIManager = uIManager;
    }
    public async UniTask Load()
    {
        await _uIManager.InitWindows();
    }
}
