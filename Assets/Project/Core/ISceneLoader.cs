using Cysharp.Threading.Tasks;

public interface ISceneLoader 
{
    public UniTask LoadSceneAsync(int sceneId);
}
