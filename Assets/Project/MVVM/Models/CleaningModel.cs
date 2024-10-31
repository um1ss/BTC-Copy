using System;
using Balancy;
using Balancy.Data;
using Balancy.Models.BigTitShop;
using Cysharp.Threading.Tasks;
using Project.MVVM.Models;
using VContainer;

public class CleaningModel : AbstractModel<int>, ILoadingOperation
{
    [Inject] private MainValuesModel _mainValuesModel;
    public CleaningData PlayerCleaning { get; private set; }
    public GeneratorManager GeneratorManager { get; private set; }
    public bool IsInitialized { get; private set; }
    public bool IsIndicatorAvailable { get; private set; }

    private CleaningState state = CleaningState.inactive;

    public event Action<float> OnGirlsSpeedChanged;

    public async UniTask Load() {
        await LoadPlayerCleaning();
    }

    public void Init(GeneratorManager generatorManager) {
        GeneratorManager = generatorManager;
        IsInitialized = true;
    }

    public async UniTask RunRoutineUpdate() {
        var prevState = state;
        UpdateState();

        if (prevState != state) {
            OnGirlsSpeedChanged?.Invoke(GetCleaningValue());
        }

        IsIndicatorAvailable = CanActivateCleaning();
    }

    public bool CanActivateCleaning() {
        var now = DateTimeOffset.UtcNow;
        var lastStartTime = DateTimeOffset.FromUnixTimeMilliseconds(PlayerCleaning.CleaningInfo.LastStartDateTime);
        var isDelayPassed = (now - lastStartTime).TotalHours > CleaningConstants.DelayHours;

        var likes = _mainValuesModel.GetValue(AppConstants.LikeTextsName);
        var hasEnoughLikes = likes >= CleaningConstants.ActivationLikesPrice;
        var allGeneratorsOpened = GeneratorManager.IsAllGeneratorsOpened();

        return state == CleaningState.inactive && isDelayPassed && hasEnoughLikes && allGeneratorsOpened;
    }

    public bool TryToActivateCleaning() {
        if (CanActivateCleaning()) {
            var now = DateTimeOffset.UtcNow;
            var like = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().LIKE.UnnyId);
            LiveOps.Profile.Inventories.Currencies.RemoveItem(like, CleaningConstants.ActivationLikesPrice);
            PlayerCleaning.CleaningInfo.LastStartDateTime = now.ToUnixTimeMilliseconds();
            SmartStorage.ForceSaveSmartObject(PlayerCleaning);
            return true;
        }

        return false;
    }

    public bool TryGetTimerEndValue(out DateTimeOffset endTime) {
        var now = DateTimeOffset.Now;
        var lastStartTime = DateTimeOffset.FromUnixTimeMilliseconds(PlayerCleaning.CleaningInfo.LastStartDateTime);
        switch (state) {
            case CleaningState.active: {
                endTime = lastStartTime.AddMinutes(CleaningConstants.DurationMinutes);
                var timeTillEnd = endTime - now;
                return timeTillEnd.TotalSeconds > 0;
            }
            case CleaningState.inactive: {
                endTime = lastStartTime.AddMinutes(CleaningConstants.DurationMinutes)
                    .AddHours(CleaningConstants.DelayHours);
                var timeTillEnd = endTime - now;
                return timeTillEnd.TotalSeconds > 0;
            }
            default:
                endTime = DateTimeOffset.MinValue;
                return false;
        }
    }

    public float GetCleaningValue() {
        return state == CleaningState.active ? CleaningConstants.CleaningEarnValue : 1;
    }

    private void UpdateState() {
        var now = DateTimeOffset.Now;
        var lastStartTime = DateTimeOffset.FromUnixTimeMilliseconds(PlayerCleaning.CleaningInfo.LastStartDateTime);
        var timeUntilStart = now - lastStartTime;

        state = timeUntilStart.TotalMinutes <= CleaningConstants.DurationMinutes
            ? CleaningState.active
            : CleaningState.inactive;
    }

    private async UniTask LoadPlayerCleaning() {
        var loaded = false;
        SmartStorage.LoadSmartObject<CleaningData>(response => {
            PlayerCleaning = response.Data;
            loaded = true;
        });
        await UniTask.WaitUntil(() => loaded);
    }

    public override void Initialize() {
        throw new NotImplementedException();
    }

    public override void SetDelegate(string name, Action<int> del) {
        throw new NotImplementedException();
    }

    public override void ChangeValue(string name, int newValue) { }
}

public enum CleaningState
{
    active,
    inactive
}