using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneLoaderService : ISceneLoader
{
    public async UniTask LoadSceneAsync(int sceneId)
    {
        await SceneManager.LoadSceneAsync(sceneId).ToUniTask();
    }
}
