using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Tutorial
{
    public class TutorialOverlay : MonoBehaviour
    {
        [SerializeField] private CanvasGroup selfGroup;
        [SerializeField] private GameObject screenBlockerObj;
        [SerializeField] private HighlightsOverlay highlights;
        [SerializeField] private GameObject highlightsBlocker;

        [SerializeField] private Button selfBtn;
        [SerializeField] private RectTransform selfRect;

        public TutorialOverlayState state = TutorialOverlayState.closed;
        private ExitRule exitRule = ExitRule.tapOnElement;
        private HighlightElemId[] Elements => highlights.elementsToShow.ToArray();

        private readonly float showDuration = 0.4f;

        public HighlightsOverlay Highlights => highlights;

        public enum TutorialOverlayState
        {
            closed,
            opening,
            opened,
            closing
        }

        public readonly ReactiveProperty<bool> IsShowing = new(false);
        private void Start()
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
        }
        public void Show(ExitRule rule = ExitRule.tapOnElement) {
            ShowInternal(rule).Forget();
        }
        
        public void SetBlockerEnabled(bool isEnabled) {
            highlightsBlocker.SetActive(isEnabled);
        }
        
        public async UniTask ShowAndWaitForClose(ExitRule rule = ExitRule.tapOnElement) {
            ShowInternal(rule).Forget();
            await UniTask.WaitUntil(() => state == TutorialOverlayState.closed);
        }

        public void Destroy() {
            Destroy(gameObject);
        }

        private void Hide() {
            HideInternal().Forget();
        }

        // ------------------------------- Private & protected methods -------------------------------

        private async UniTask HideInternal() {
            state = TutorialOverlayState.closing;
            selfGroup.interactable = false;
            await HideAnim();
            highlights.Clear();
            state = TutorialOverlayState.closed;
        }

        private async UniTask ShowInternal(ExitRule rule) {
            state = TutorialOverlayState.opening;
            exitRule = rule;

            selfGroup.interactable = true;

            gameObject.SetActive(true);
            IsShowing.Value = true;

            highlights.gameObject.SetActive(true);
            screenBlockerObj.SetActive(true);

            selfBtn.enabled = exitRule == ExitRule.tapOnScreen;
            if (exitRule == ExitRule.tapOnScreen) {
                selfBtn.onClick.AddListener(Hide);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(selfRect);
            await ShowAnim();
            state = TutorialOverlayState.opened;
        }

        private async UniTask ShowAnim() {
            selfGroup.alpha = 0;
            selfGroup.DOFade(1, showDuration);
            await UniTask.WaitForSeconds(showDuration);
        }

        private async UniTask HideAnim() {
            selfGroup.DOFade(0, showDuration);
            await UniTask.WaitForSeconds(showDuration);
            gameObject.SetActive(false);
            IsShowing.Value = false;
        }

        private void Awake() {
            IsShowing.Value = false;
            highlights.onAreaClick += OnAreaClick;
        }

        private void OnAreaClick(int elementIdx) {
            if (exitRule == ExitRule.tapOnElement
                || exitRule == ExitRule.tapOnScreen
            ) {
                var elementId = Elements[elementIdx];
                var element = HighlightElementId.GetElementOnScreen(elementId);
                if (element) {
                    element.Activate();
                }

                Hide();
            }
        }
    }
}

public enum ExitRule
{
    /// <summary>
    /// Клик в любом есте экрана
    /// </summary>
    tapOnScreen,

    /// <summary>
    /// Клик по подсвеченному элементу
    /// </summary>
    tapOnElement,
}