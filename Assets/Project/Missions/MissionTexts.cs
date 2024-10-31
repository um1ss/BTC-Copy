using System.Collections.Generic;
using Balancy.Localization;
using Project.Shared;
using UnityEngine;

namespace Project.Missions
{
    public class MissionTexts
    {
        private MissionsModel _model;
        public Dictionary<int, MissionText> Texts;

        public MissionTexts(MissionsModel model) {
            _model = model;

            Texts = new Dictionary<int, MissionText>(9);
            UpdateTexts();
        }

        public void UpdateTexts() {
            Texts[0] = Get1TargetText(0);
            Texts[1] = GetGeneratorNameText(1);
            Texts[2] = Get1TargetText(2);
            Texts[3] = GetGeneratorNameText(3);
            Texts[4] = Get1TargetText(4);
            Texts[5] = GetGeneratorNameText(5);
            Texts[6] = Get1TargetText(6);
            Texts[7] = GetDefaultMissionText(7);
            Texts[8] = GetDefaultMissionText(8);
        }

        private MissionText GetGeneratorNameText(int id) {
            var meta = _model.GetMissionsMetaById(id);
            var info = _model.GetPlayerMissionById(id);
            var stage = Mathf.Min(info.MissionStage, meta.Stages.Length - 1);
            
            var target = meta.Stages[stage].TargetCount - 1;
            var generatorName = GetTextByGeneratorId(target);

            var descriptionText = new Text1(meta.MissionText.Value).GetText(generatorName);
            var btnText = $"{generatorName}";
            return new MissionText(descriptionText, btnText);
        }

        private MissionText Get1TargetText(int id) {
            var meta = _model.GetMissionsMetaById(id);
            var info = _model.GetPlayerMissionById(id);
            var stage = Mathf.Min(info.MissionStage, meta.Stages.Length - 1);
            
            var target = meta.Stages[stage].TargetCount;
            var targetRemains = meta.Stages[stage].TargetCount - info.MissionProgress;

            var descriptionText = new Text1(meta.MissionText.Value).GetText(target);
            var btnText = targetRemains.ToString();
            return new MissionText(descriptionText, btnText);
        }

        private MissionText GetDefaultMissionText(int id) {
            var meta = _model.GetMissionsMetaById(id);

            var descriptionText = new Text0(meta.MissionText.Value).GetText();
            var btnText = "";
            return new MissionText(descriptionText, btnText);
        }

        private string GetTextByGeneratorId(int id) {
            return id switch {
                0 => Manager.Get("DEFAULT/SlotMachines"),
                1 => Manager.Get("DEFAULT/Bar"),
                2 => Manager.Get("DEFAULT/Dj"),
                3 => Manager.Get("DEFAULT/Sofa"),
                4 => Manager.Get("DEFAULT/Lunch"),
                5 => Manager.Get("DEFAULT/Roulette"),
                6 => Manager.Get("DEFAULT/PokerTable"),
                7 => Manager.Get("DEFAULT/LunchLeft"),
                8 => Manager.Get("DEFAULT/LunchRight"),
                9 => Manager.Get("DEFAULT/Vending"),
                _ => null
            };
        }
    }

    public class MissionText
    {
        public readonly string descriptionText;
        public readonly string btnText;

        public MissionText(string descriptionText, string btnText) {
            this.descriptionText = descriptionText;
            this.btnText = btnText;
        }
    }
}