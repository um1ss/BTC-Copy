using System;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Tutorial
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class HighlightsOverlay : MonoBehaviour
    {
        [SerializeField] private float borderWidth = 1f;
        [SerializeField] private bool softBorder = true;
        [SerializeField] private bool drawBorder = true;
        [SerializeField] private Color borderColor = new Color(1, 1, 1, 1f);
        [SerializeField] private Color baseColor = new Color(0, 0, 0, 0.75f);
        [SerializeField] private Color transparentColor = new Color(0, 0, 0, 0);
        [SerializeField] public HighlightArea[] areas;
        [SerializeField] public float padding = 0f;

        public event Action<int> onAreaClick;

        private MaskableGraphic mGraphic;
        private Material mat;
        private RectTransform selfRect;

        [CanBeNull] private string messageText;
        private AreaDirection messageDirection;

        /// <summary>
        /// Если спсок не создан, то вложенные HighlightArea не обновляются
        /// </summary>
        [CanBeNull] private List<ElementRect> elementsAndRects;

        /// <summary>
        /// Список id элементов, которые должны быть отображены.
        /// При создании/включении элементы, у которых id есть в этом списке, попадают в elementsAndRects.
        /// </summary>
        public readonly HashSet<HighlightElemId> elementsToShow = new();

        private static readonly int[] posAndSizeProperty = {
            Shader.PropertyToID("_PosAndSize1"),
            Shader.PropertyToID("_PosAndSize2")
        };

        private static readonly int canvasSizeProperty = Shader.PropertyToID("_CanvasSize");
        private static readonly int softBorderProperty = Shader.PropertyToID("_SoftBorder");
        private static readonly int borderWidthProperty = Shader.PropertyToID("_BorderWidth");
        private static readonly int borderColorProperty = Shader.PropertyToID("_BorderColor");
        private static readonly int baseColorProperty = Shader.PropertyToID("_BaseColor");
        private static readonly int transparentColorProperty = Shader.PropertyToID("_TransparentColor");

        private static readonly Vector4 offScreenEllipse = new(-100, -100, 1, 1);

        class ElementRect
        {
            public readonly HighlightElementId element;
            public Rect rect;

            public ElementRect(HighlightElementId element, Rect rect) {
                this.element = element;
                this.rect = rect;
            }
        }

        private void Awake() {
            selfRect = GetComponent<RectTransform>();
            for (int i = 0; i < areas.Length; i++) {
                areas[i].index = i;
                areas[i].onClick += a => onAreaClick?.Invoke(a.index);
            }
        }

        private void OnEnable() {
            SetMaterial();
        }

        private void Update() {
            UpdateAllElements();
            UpdateMaterialParams();
        }

        private void OnValidate() {
            SetMaterial();
        }

        private void SetMaterial() {
            mGraphic = this.GetComponent<MaskableGraphic>();

            if (mGraphic != null) {
                if (mGraphic.material == null || mGraphic.material.name == "Default UI Material") {
                    var material = new Material(Shader.Find("Tutorial/DrawCircle"));
                    material.hideFlags = material.hideFlags | HideFlags.HideAndDontSave;
                    mGraphic.material = material;
                }

                mat = mGraphic.material;
            }
            else {
                Debug.LogError("Please attach component to a Graphical UI component");
            }
        }

        private void UpdateMaterialParams() {
            var idx = 0;
            if (areas != null) {
                foreach (var s in areas) {
                    if (s != null && s.gameObject.activeSelf) {
                        var size = s.selfRect.sizeDelta;
                        var p = s.selfRect.localPosition;
                        var scale = s.selfRect.localScale;
                        var posAndSize = new Vector4(p.x, p.y, scale.x * size.x, scale.y * size.y);
                        mat.SetVector(posAndSizeProperty[idx], posAndSize);
                        idx++;
                    }
                }
            }

            for (; idx < posAndSizeProperty.Length; idx++) {
                mat.SetVector(posAndSizeProperty[idx], offScreenEllipse);
            }

            if (selfRect != null) {
                var sr = selfRect.rect;
                mat.SetVector(canvasSizeProperty, new Vector4(sr.width, sr.height));
            }
            else {
                mat.SetVector(canvasSizeProperty, Vector4.zero);
            }

            mat.SetFloat(softBorderProperty, softBorder ? 1f : 0f);
            mat.SetFloat(borderWidthProperty, borderWidth);
            mat.SetColor(borderColorProperty, borderColor);
            mat.SetColor(baseColorProperty, baseColor);
            mat.SetColor(transparentColorProperty, transparentColor);

            if (drawBorder) {
                mat.EnableKeyword("DRAW_BORDER");
            }
            else {
                mat.DisableKeyword("DRAW_BORDER");
            }
        }

        public void Clear() {
            if (areas != null) {
                foreach (var area in areas) {
                    area.gameObject.SetActive(false);
                }
            }

            elementsToShow.Clear();

            if (elementsAndRects != null) {
                elementsAndRects.Clear();
            }
        }

        public void AddElement(
            HighlightElemId elemId,
            bool showPointer,
            string messageText = null,
            AreaDirection messageDirection = AreaDirection.left
        ) {
            elementsToShow.Add(elemId);
            this.messageText = messageText;
            this.messageDirection = messageDirection;

            var elementId = HighlightElementId.GetElementOnScreen(elemId);
            if (elementId != null) {
                RegisterElement(elementId, showPointer);
            }
        }

        private void RegisterElement(HighlightElementId elementId, bool showPointer) {
            if (elementsAndRects == null) {
                elementsAndRects = new List<ElementRect>();
            }

            var rect = GetElementRect(elementId);
            elementsAndRects.Add(new ElementRect(elementId, rect));
            var rectIndex = elementsAndRects.Count - 1;
            UpdateRectByIndex(rectIndex);

            var area = areas[rectIndex];
            area.pointerIconObj.SetActive(showPointer);
            area.UpdateText(messageText, messageDirection);
        }

        private void UpdateAllElements() {
            if (elementsAndRects != null) {
                foreach (var e in elementsAndRects) {
                    e.rect = GetElementRect(e.element);
                }

                for (int i = 0; i < areas.Length; i++) {
                    UpdateRectByIndex(i);
                }
            }
        }

        private void UpdateRectByIndex(int rectIndex) {
            if (rectIndex >= areas.Length || areas == null || elementsAndRects == null) {
                return;
            }

            var highlightArea = areas[rectIndex];
            if (highlightArea != null) {
                if (rectIndex >= elementsAndRects.Count) {
                    highlightArea.gameObject.SetActive(false);
                    return;
                }

                var rect = elementsAndRects[rectIndex].rect;
                highlightArea.gameObject.SetActive(true);
                highlightArea.selfRect.localPosition = rect.center - rect.size / 2f;
                highlightArea.selfRect.sizeDelta = rect.size;

                highlightArea.UpdateText(messageText, messageDirection);
            }
        }

        private Rect GetElementRect(HighlightElementId element) {
            var rect = GetLocalRect(element.selfRect);
            rect.size += 2 * padding * Vector2.one;
            return rect;
        }

        private Rect GetLocalRect(RectTransform hintRect) {
            var selfCorners = new Vector3[4];
            selfRect.GetWorldCorners(selfCorners);

            var hintCorners = new Vector3[4];
            if (hintRect != null) {
                hintRect.GetWorldCorners(hintCorners);
            }

            var selfSize = selfCorners[2] - selfCorners[0];
            var hintSize = hintCorners[2] - hintCorners[0];
            var center = (hintCorners[2] + hintCorners[0]) / 2.0f - selfCorners[0];

            var size = selfRect.rect.size;
            Rect rect = new Rect();
            var scaleX = size.x / selfSize.x;
            var scaleY = size.y / selfSize.y;
            rect.center = new Vector2(scaleX * center.x, scaleY * center.y);
            rect.size = new Vector2(scaleX * hintSize.x, scaleY * hintSize.y);

            return rect;
        }
    }
}