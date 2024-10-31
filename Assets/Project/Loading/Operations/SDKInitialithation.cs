using Balancy.Platforms.Nutaku;
using Balancy;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SDKInitialithation : ILoadingOperation
{
    private bool _isInitReady = false;
    public async UniTask Load()
    {
        ExternalEvents.RegisterSmartObjectsListener(new BalancyShopSmartObjectsEvents());

        bool autologin = true;

        var platform = Constants.Platform.NutakuPCBrowser;

#if UNITY_ANDROID
        platform = Constants.Platform.NutakuAndroid;
#endif

        var appConfig = new AppConfig
        {
            ApiGameId = AppConstants.API_GAME_ID,
            PublicKey = AppConstants.PUBLIC_KEY,
            Environment = GetEnvironment(),
            AutoLogin = autologin,
            Platform = platform,
            NutakuConfig = new NutakuConfig() { ConsumerKey = AppConstants.CONSUMER_KEY, ConsumerSecret = AppConstants.CONSUMER_SECRET },
            OnInitProgress = progress =>
            {
                if (progress.Status == BalancyInitStatus.Finished)
                {
                    OnInitNutaku();
                }
            },
            UpdateType = UpdateType.FullUpdate,
            UpdatePeriod = 300,
        };

        if (!autologin)
        {
            Auth.SignOut(AppConstants.API_GAME_ID, GetEnvironment(), null);
        }

        Main.Init(appConfig);

        while (!_isInitReady)
        {
            await UniTask.Yield();
        }
    }
    async void OnInitNutaku()
    {
        var wait = true;

        NutakuNetwork.NutakuInstance.Login(result => {
            Debug.Log("Login Success");
            if (!result.Success)
                Debug.LogError("Nutaku auth error: " + result.Error.Code + " => " + result.Error.Message);
            else
                wait = false;
            _isInitReady = true;
        });

        while (wait)
            await UniTask.Yield();
        //#if UNITY_EDITOR
        //        NutakuNetwork.NutakuInstance.Login(result =>
        //        {
        //            Debug.Log("Login Success");
        //            if (!result.Success)
        //                Debug.LogError("Nutaku auth error: " + result.Error.Code + " => " + result.Error.Message);
        //            else
        //            {
        //                _isInitReady = true;
        //            }
        //        });
        //#else
        //                    await UniTask.Yield();
        //                    Auth.SignOut(AppConstants.API_GAME_ID, Constants.Environment.Development, onBalansyOut);
        //                    void onBalansyOut()
        //                    {
        //                        NutakuNetwork.NutakuInstance.Login(result =>
        //                        {
        //                            Debug.Log("Login Success");
        //                            if (!result.Success)
        //                                Debug.LogError("Nutaku auth error: " + result.Error.Code + " => " + result.Error.Message);
        //                                            else
        //                                                {
        //                                                    _isInitReady = true;
        //                                                }
        //                        });
        //                    }
        //#endif
    }
    Constants.Environment GetEnvironment()
    {
#if GAME_SERVER
                            return Constants.Environment.Production;
#elif GAME_STAGE
                            return Constants.Environment.Stage;
#else
        return Constants.Environment.Development;
#endif
    }
}
