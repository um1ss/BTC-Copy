using R3;
using System;

public class SettingsModel : AbstractModel<float>
{
    public SettingsModel()
    {
        _values.Add(AppConstants.MusicValueName, new ReactiveProperty<float>());
        _values.Add(AppConstants.SoundValueName, new ReactiveProperty<float>());
    }
    public override void SetDelegate(string name, Action<float> del)
    {
        if (_values.TryGetValue(name, out var property))
        {
            if (_disposes.ContainsKey(name))
            {
                _disposes[name] = property.Subscribe(del);
            }
            else
            {
                _disposes.Add(name, property.Subscribe(del));
            }
        }
    }
    public override void ChangeValue(string name, float newValue)
    {
        if (_values.TryGetValue(name, out var property))
        {
            property.Value = newValue;
        }
    }
    public void SetVolumes(float soundVolume, float musicVolume)
    {
        ChangeValue(AppConstants.MusicValueName, musicVolume);
        ChangeValue(AppConstants.SoundValueName, soundVolume);
    }
    public string GetUserId()
    {
        return Nutaku.Unity.SdkPlugin._loginInfo.userId;
    }
}
