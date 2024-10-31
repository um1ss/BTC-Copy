using Balancy.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Project.Tutorial
{
    public class TutorialCtl : ILoadingOperation
    {
        [Inject] protected IAssetProvider assetProvider;
        [Inject] protected GeneratorManager generatorManager;

        private TutorialData tutorialData;
        private FirstTutorial firstTutorial;

        public bool IsTutorialInProgress;

        public GameObject TutorialOverlay;

        public async UniTask Load() {
            await LoadPlayerTutorial();
            if (!tutorialData.TutorialInfo.IsStartTutorialShown) {
                await LoadOverlay();
            }
        }

        public void MaybeStartFirstTutorial() {
            if (!tutorialData.TutorialInfo.IsStartTutorialShown && !IsTutorialInProgress) {
                firstTutorial = new FirstTutorial(this, generatorManager);
                firstTutorial.StartTutorial(tutorialData).Forget();
            }
        }

        private async UniTask LoadPlayerTutorial() {
            var loaded = false;
            SmartStorage.LoadSmartObject<TutorialData>(response => {
                tutorialData = response.Data;
                loaded = true;
            });
            await UniTask.WaitUntil(() => loaded);
        }

        private async UniTask LoadOverlay() {
            TutorialOverlay = await assetProvider.Load<GameObject>(AssetsConstants.TutorialOverlay);
        }
    }
}