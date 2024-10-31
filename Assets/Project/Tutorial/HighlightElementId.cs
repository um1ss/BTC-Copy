using System;
using UnityEngine;

namespace Project.Tutorial
{
    public class HighlightElementId : MonoBehaviour
    {
        /// <summary>
        /// Уникальный строковый идентификатор элемента
        /// </summary>
        [SerializeField] public HighlightElemId elementId = HighlightElemId.none;

        /// <summary>
        /// Область для подсветки в TutorialOverlay
        /// </summary>
        public RectTransform selfRect { get; private set; }

        /// <summary>
        /// Активированно в процессе прохождения туториала
        /// </summary>
        public event Action onActivate;

        private void Awake() {
            selfRect = GetComponent<RectTransform>();
        }

        public void Activate() {
            onActivate?.Invoke();
        }

        public static HighlightElementId GetElementOnScreen(HighlightElemId elementId) {
            if (elementId == HighlightElemId.none) {
                return null;
            }
            foreach (var element in FindObjectsOfType<HighlightElementId>()) {
                if (element.elementId == elementId) {
                    return element;
                }
            }
            return null;
        }
    }
}