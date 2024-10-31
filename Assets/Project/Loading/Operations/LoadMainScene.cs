using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class LoadMainScene : ILoadingOperation
{
    public async UniTask Load()
    {
        await SceneManager.LoadSceneAsync(AppConstants.MainSceneID).ToUniTask();
    }
}
