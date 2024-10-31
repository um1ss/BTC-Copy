using Cysharp.Threading.Tasks;

public class LoadAudio : ILoadingOperation
{
    private MusicAudioManager _audioManager;
    private bool _isLoop;

    public LoadAudio(MusicAudioManager audioManager, bool isLoop)
    {
        _audioManager = audioManager;
        _isLoop = isLoop;
    }
    public async UniTask Load()
    {
        await _audioManager.LoadAndPlayMainClip(_isLoop);
    }
}
