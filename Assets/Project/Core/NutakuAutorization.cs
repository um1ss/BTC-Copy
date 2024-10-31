using Balancy;
using Balancy.Platforms.Nutaku;
using System;
using System.Threading.Tasks;
using UnityEngine;


public class NutakuAutorization : MonoBehaviour
{
    private void Awake()
    {
        
        Main.Init(new AppConfig
        {
            ApiGameId = "53c64844-6c47-11ef-ba01-066676c39f77",
            PublicKey = "M2YwYzQxMDRmMTczNWM2ZmJkOWJmOW",
            Environment = Constants.Environment.Development,
            Platform = Constants.Platform.NutakuAndroid,
            PreInit = PreInitType.None,
            AutoLogin = true,
            UpdateType = UpdateType.FullUpdate,
            NutakuConfig = new NutakuConfig() { ConsumerKey = "M5ecLs2UXs43n5Pn", ConsumerSecret = "1IyXk2tqYQq0#R8JM?MkpE-bZ6=@1QlK" },
            OnInitProgress = progress =>
            {
                Debug.Log($"***=> STATUS {progress.Status}");
                switch (progress.Status)
                {
                    case BalancyInitStatus.PreInitFromResourcesOrCache:
                        //CMS, loaded from resource or cache is ready, invoked only if PreInit >= PreInitType.LoadStaticData
                        break;
                    case BalancyInitStatus.PreInitLocalProfile:
                        //Local profile is loaded, invoked only if PreInit >= PreInitType.LoadStaticDataAndLocalProfile
                        break;
                    case BalancyInitStatus.DictionariesReady:
                        //CMS is updated and ready
                        break;
                    case BalancyInitStatus.Finished:
                        OnInit(progress);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            },
        });
    }

#pragma warning disable CS1998 // � ����������� ������ ����������� ��������� await, ����� �������� ���������� �����
    private async void OnInit(InitProgress progress)
#pragma warning restore CS1998 // � ����������� ������ ����������� ��������� await, ����� �������� ���������� �����
    {
        #if UNITY_EDITOR
        Debug.Log("OnInit");
        NutakuNetwork.NutakuInstance.Login(result =>
                {
                    Debug.Log("Login Success");
                    if (!result.Success)
                        Debug.LogError("Nutaku auth error: " + result.Error.Code + " => " + result.Error.Message);
                });
        #else
                await Task.Yield();
                Auth.SignOut("53c64844-6c47-11ef-ba01-066676c39f77", Constants.Environment.Development, onBalansyOut);
                void onBalansyOut()
                {
                    NutakuNetwork.NutakuInstance.Login(result =>
                    {
                        Debug.Log("Login Success");
                        if (!result.Success)
                            Debug.LogError("Nutaku auth error: " + result.Error.Code + " => " + result.Error.Message);
                    });
                }
        #endif
    }
}