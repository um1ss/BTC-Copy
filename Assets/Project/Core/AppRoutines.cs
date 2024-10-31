using Cysharp.Threading.Tasks;
using Project.Tutorial;
using VContainer;

namespace Project.Core
{
    /// <summary>
    /// Модуль запуска "рутинных задач", т.е. различных автоматических действий: попапы наград, отображение прогресса, туториалы и т.д..
    /// </summary>
    public class AppRoutines
    {
        [Inject] private readonly MissionsModel missionsModel;
        [Inject] private readonly CleaningModel cleaningModel;
        [Inject] private readonly TutorialCtl tutorialCtl;

        private readonly State state = new State();

        class State
        {
            /// <summary>
            /// В данный момент выполняется цепочка задач.
            /// </summary>
            internal bool inRoutineChain;
        }

        public async UniTask ViewPeriodicallyUpdate() {
            if (!state.inRoutineChain) {
                await RunHotelViewRoutines();
            }
        }

        public async UniTask RunHotelViewRoutines() {
            var tutorialModuleIsBusy = tutorialCtl.IsTutorialInProgress;
            if (state.inRoutineChain || tutorialModuleIsBusy) {
                return;
            }

            state.inRoutineChain = true;

            // begin

            missionsModel.UpdateMissionsStates(false);
            await cleaningModel.RunRoutineUpdate();

            // end

            state.inRoutineChain = false;
        }
    }
}