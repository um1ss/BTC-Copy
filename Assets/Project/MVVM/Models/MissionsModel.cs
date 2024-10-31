using System;
using System.Collections.Generic;
using System.Linq;
using Balancy;
using Balancy.Data;
using Balancy.Models;
using Cysharp.Threading.Tasks;
using Project.Missions;
using Project.MVVM.Models;
using UnityEngine;

// TODO восклицательный знак на иконке заданий
public class MissionsModel : AbstractModel<int>, ILoadingOperation
{
    public MissionsData PlayerMissions { get; private set; }
    public List<MissionsMeta> Metas => DataEditor.MissionsMetas;
    public MissionTexts Texts { get; private set; }
    public GeneratorManager GeneratorManager { get; private set; }
    public bool IsInitialized { get; private set; }
    public MissionProgressManager ProgressManager { get; private set; }

    public bool IsIndicatorAvailable { get; private set; }

    public async UniTask Load() {
        await LoadPlayerMissions();
    }

    public void Init(GeneratorManager generatorManager) {
        GeneratorManager = generatorManager;
        Texts = new MissionTexts(this);
        ProgressManager = new MissionProgressManager(this);
        IsInitialized = true;
    }

    public void UpdateMissionsStates(bool updateTexts = true) {
        if (updateTexts) {
            Texts.UpdateTexts();
        }

        ProgressManager.UpdateMissions();

        var hasMissionsToClaim = false;

        foreach (var meta in Metas) {
            var playerMission = GetPlayerMissionById(meta.Id);

            if (playerMission.MissionStage >= meta.Stages.Length) {
                playerMission.MissionState = (int) MissionState.Finished;
            }
            else if (playerMission.MissionProgress >= meta.Stages[playerMission.MissionStage].TargetCount) {
                playerMission.MissionState = (int) MissionState.ClaimReward;
                hasMissionsToClaim = true;
            }
            else {
                playerMission.MissionState = (int) MissionState.InProgress;
            }
        }

        IsIndicatorAvailable = hasMissionsToClaim;
    }

    private async UniTask LoadPlayerMissions() {
        var loaded = false;
        SmartStorage.LoadSmartObject<MissionsData>(response => {
            PlayerMissions = response.Data;
            loaded = true;
        });
        await UniTask.WaitUntil(() => loaded);

        var list = PlayerMissions.PlayerMissions.PlayerMissionsList;

        if (list.Count < Metas.Count) {
            for (var i = 0; i < Metas.Count; i++) {
                var meta = Metas[i];
                var containsInfoForMeta = list.FindIndex(info => info.MissionId == meta.Id) != -1;

                if (!containsInfoForMeta) {
                    var newPlayerMission = new PlayerMissionInfo {
                        MissionId = Metas[i].Id,
                        MissionProgress = 0,
                        MissionStage = 0,
                        MissionState = 0
                    };
                    list.Add(newPlayerMission);
                }
            }
        }
    }

    public PlayerMissionInfo GetPlayerMissionById(int id) {
        return PlayerMissions.PlayerMissions.PlayerMissionsList.ToList().First(info => info.MissionId == id);
    }

    public MissionsMeta GetMissionsMetaById(int id) {
        return Metas.First(meta => meta.Id == id);
    }

    public void OnCollectBtnClick(int missionId) {
        var playerMission = GetPlayerMissionById(missionId);
        if ((MissionState) playerMission.MissionState == MissionState.ClaimReward) {
            var meta = GetMissionsMetaById(missionId);
            var reward = meta.Stages[playerMission.MissionStage].Reward;
            LiveOps.Profile.Inventories.AddReward(reward);
            playerMission.MissionStage++;
            playerMission.MissionProgress = 0;
        }
    }

    public override void Initialize() {
        throw new NotImplementedException();
    }

    public override void ChangeValue(string name, int newValue) { }

    public override void SetDelegate(string name, Action<int> del) { }
}