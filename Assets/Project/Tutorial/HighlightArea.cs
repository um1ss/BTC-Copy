using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Tutorial
{
    public class HighlightArea : MonoBehaviour
    {
        [SerializeField] public RectTransform selfRect;
        [SerializeField] public Button selfBtn;
        [SerializeField] public GameObject pointerIconObj;
        [SerializeField] private GameObject[] messageContainers;
        [SerializeField] private GameObject messageObj;
        [SerializeField] private TMP_Text messageText;

        internal int index;
        public event Action<HighlightArea> onClick;

        private void Awake() {
            selfBtn.onClick.AddListener(() => onClick?.Invoke(this));
        }

        public void UpdateText([CanBeNull] string text, AreaDirection direction = AreaDirection.left) {
            messageObj.SetActive(text != null);
            if (text != null) {
                messageText.text = text;
                messageObj.transform.SetParent(messageContainers[(int)direction].transform);
                messageObj.transform.localPosition = Vector3.zero;
            }
        }
    }

    public enum AreaDirection
    {
        left = 0,
        leftTop = 1,
        top = 2,
        rightTop = 3,
        right = 4,
        rightBottom = 5,
        bottom = 6,
        leftBottom = 7,
    }
}