using Balancy;
using Balancy.Data;
using Balancy.Localization;
using Balancy.Models.BigTitShop;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Tutorial
{
    public class FirstTutorial
    {
        private TutorialCtl ctl;
        private GeneratorManager generatorManager;
        private TutorialOverlay overlay;
        private bool isGeneratorEarned;

        public FirstTutorial(TutorialCtl ctl, GeneratorManager generatorManager) {
            this.ctl = ctl;
            this.generatorManager = generatorManager;
        }

        public async UniTask StartTutorial(TutorialData tutorialData) {
            ctl.IsTutorialInProgress = true;
            overlay = Object.Instantiate(ctl.TutorialOverlay).GetComponent<TutorialOverlay>();

            generatorManager.GetGenerator(AppConstants.SlotMaschines).OnEndGenerate += _ => isGeneratorEarned = true;
            AddStartCurrency();
            
            await PlayTutorial();

            tutorialData.TutorialInfo.IsStartTutorialShown = true;
            SmartStorage.ForceSaveSmartObject(tutorialData);

            overlay.Destroy();
            ctl.IsTutorialInProgress = false;
        }

        private async UniTask PlayTutorial() {
            overlay.Highlights.AddElement(
                HighlightElemId.chipsPane,
                false,
                Manager.Get("DEFAULT/Tutorial_Chips"),
                AreaDirection.rightBottom);
            await overlay.ShowAndWaitForClose(ExitRule.tapOnScreen);

            overlay.Highlights.AddElement(
                HighlightElemId.condomsPane,
                false,
                Manager.Get("DEFAULT/Tutorial_Condoms"),
                AreaDirection.bottom);
            await overlay.ShowAndWaitForClose(ExitRule.tapOnScreen);

            overlay.Highlights.AddElement(
                HighlightElemId.energyPane,
                false,
                Manager.Get("DEFAULT/Tutorial_Energy"),
                AreaDirection.bottom);
            await overlay.ShowAndWaitForClose(ExitRule.tapOnScreen);

            overlay.Highlights.AddElement(
                HighlightElemId.collectionsOpenBtn,
                true,
                Manager.Get("DEFAULT/Tutorial_OpenCollectionWindow"));
            await overlay.ShowAndWaitForClose();

            overlay.Highlights.AddElement(
                HighlightElemId.generatorBuyBtn,
                true,
                Manager.Get("DEFAULT/Tutorial_OpenGenerator"),
                AreaDirection.right);
            await overlay.ShowAndWaitForClose();

            overlay.Highlights.AddElement(
                HighlightElemId.collectionsCloseBtn,
                true);
            await overlay.ShowAndWaitForClose();

            overlay.Highlights.AddElement(
                HighlightElemId.collectMoneyBtn,
                true,
                Manager.Get("DEFAULT/Tutorial_CollectChips"),
                AreaDirection.right);
            await ShowAndWaitForCollect();

            overlay.Highlights.AddElement(
                HighlightElemId.missionsOpenBtn,
                true, 
                Manager.Get("DEFAULT/Tutorial_OpenMissionWindow"));
            await overlay.ShowAndWaitForClose();

            overlay.Highlights.AddElement(
                HighlightElemId.missionCollectBtn,
                true);
            await overlay.ShowAndWaitForClose();

            overlay.Highlights.AddElement(
                HighlightElemId.missionCloseBtn,
                true);
            await overlay.ShowAndWaitForClose();
        }

        private async UniTask ShowAndWaitForCollect() {
            overlay.Show();
            overlay.SetBlockerEnabled(true);
            while (!isGeneratorEarned)
            {
                await UniTask.Yield();
            }
            overlay.SetBlockerEnabled(false);
            await UniTask.WaitUntil(() => overlay.state == TutorialOverlay.TutorialOverlayState.closed);
        }

        private void AddStartCurrency() {
            LiveOps.Profile.Inventories.Currencies.Clear();
            
            var chip = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().CHIP.UnnyId);
            LiveOps.Profile.Inventories.Currencies.AddItems(chip, 5);
            
            var condom = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().CONDOM.UnnyId);
            LiveOps.Profile.Inventories.Currencies.AddItems(condom, 10);
            
            var angela = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().Angela.UnnyId);
            LiveOps.Profile.Inventories.Currencies.AddItems(angela, 1);
            var bonny = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().Bonny.UnnyId);
            LiveOps.Profile.Inventories.Currencies.AddItems(bonny, 1);
            var jhinny = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().Jhinny.UnnyId);
            LiveOps.Profile.Inventories.Currencies.AddItems(jhinny, 1);
            var anna = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().Anna.UnnyId);
            LiveOps.Profile.Inventories.Currencies.AddItems(anna, 1);
            var michelle = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().Michelle.UnnyId);
            LiveOps.Profile.Inventories.Currencies.AddItems(michelle, 1);
        }
    }
}