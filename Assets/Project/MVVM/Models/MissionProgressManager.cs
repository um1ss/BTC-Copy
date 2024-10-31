using Balancy;
using Sirenix.Utilities;
using UnityEngine;

namespace Project.MVVM.Models
{
    public class MissionProgressManager
    {
        private MissionsModel model;

        public MissionProgressManager(MissionsModel model) {
            this.model = model;
            InitEvents();
        }
        
        public void UpdateMissions() {
            UpdateMission2();
            UpdateMission4();
            UpdateMission5();
            UpdateMission6();
        }

        private void InitEvents() {
            AddMission1Listener(0);
            AddMission3Listener(2);
            AddMission7Listener(6);
            AddMission8Listener(7);
            AddMission9Listener(8);
        }

        private void AddMission1Listener(int missionId) {
            var itemId = DataEditor.IdBalancyConstants.Get().CHIP.IntUnnyId;
            var playerMission = model.GetPlayerMissionById(missionId);

            LiveOps.Profile.Inventories.Currencies.OnNewItemWasAdded +=
                (item, count, slotIndex) => {
                    if (item.IntUnnyId == itemId) {
                        playerMission.MissionProgress += count;
                        model.UpdateMissionsStates();
                    }
                };
        }

        private void AddMission3Listener(int missionId) {
            // TODO 
        }

        private void AddMission7Listener(int missionId) {
            var itemId = DataEditor.IdBalancyConstants.Get().CHIP.IntUnnyId;
            var playerMission = model.GetPlayerMissionById(missionId);

            LiveOps.Profile.Inventories.Currencies.OnItemWasRemoved +=
                (item, count, slotIndex) => {
                    if (item.IntUnnyId == itemId) {
                        playerMission.MissionProgress += count;
                        model.UpdateMissionsStates();
                    }
                };
        }

        private void AddMission8Listener(int missionId) {
            // TODO  
        }

        private void AddMission9Listener(int missionId) {
            // TODO
        }
        
        private void UpdateMission2() {
            const int missionId = 1;
            var playerMission = model.GetPlayerMissionById(missionId);
            var meta = model.GetMissionsMetaById(missionId);
            var stage = Mathf.Min(playerMission.MissionStage, meta.Stages.Length - 1);
            var currTargetCount = meta.Stages[stage].TargetCount;
            var generator = model.GeneratorManager.GetGeneratorById(stage);

            if (generator.CurrentLvl > 0) {
                playerMission.MissionProgress = currTargetCount;
            }
        }

        private void UpdateMission4() {
            const int missionId = 3;
            var playerMission = model.GetPlayerMissionById(missionId);
            var meta = model.GetMissionsMetaById(missionId);
            var stage = Mathf.Min(playerMission.MissionStage, meta.Stages.Length - 1);
            var currTargetCount = meta.Stages[stage].TargetCount;
            var generator = model.GeneratorManager.GetGeneratorById(stage);

            if (generator.CurrentLvl >= 10) {
                playerMission.MissionProgress = currTargetCount;
            }
        }

        private void UpdateMission5() {
            const int missionId = 4;
            var playerMission = model.GetPlayerMissionById(missionId);
            var meta = model.GetMissionsMetaById(missionId);
            var stage = Mathf.Min(playerMission.MissionStage, meta.Stages.Length - 1);
            var currTargetCount = meta.Stages[stage].TargetCount;
            var generators = model.GeneratorManager.GetGenerators();

            var starsCount = 0;
            generators.ForEach(generator => {
                starsCount += generator.CurrentLvl / 20;
            });

            if (starsCount >= currTargetCount) {
                playerMission.MissionProgress = currTargetCount;
            }
        }
        
        private void UpdateMission6() {
            const int missionId = 5;
            var playerMission = model.GetPlayerMissionById(missionId);
            var meta = model.GetMissionsMetaById(missionId);
            var stage = Mathf.Min(playerMission.MissionStage, meta.Stages.Length - 1);
            var currTargetCount = meta.Stages[stage].TargetCount;
            var generator = model.GeneratorManager.GetGeneratorById(stage);

            if (generator.CurrentLvl == generator.LvlCount) {
                playerMission.MissionProgress = currTargetCount;
            }
        }
    }
}