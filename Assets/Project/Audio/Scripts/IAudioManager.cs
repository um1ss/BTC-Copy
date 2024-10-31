using Cysharp.Threading.Tasks;

public interface IAudioManager
{
    UniTask LoadAndPlayAudioClip(bool isLoop);
    void StopAudio();
    void SwitchAudioPause();
}
