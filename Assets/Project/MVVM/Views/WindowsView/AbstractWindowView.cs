using System.Collections.Generic;
using Project.Tutorial;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class AbstractWindowView : MonoBehaviour
{
    public UnityAction OnClickButton;

    protected Dictionary<string, Button> _buttons;
    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        canvas.worldCamera = cam;
    }
    public abstract void Initialize();
    public void SubscribeButton(string buttonName, UnityAction action, bool shouldSubscribeHighlight = false)
    {
        if (_buttons.TryGetValue(buttonName, out var button))
        {
            button.onClick.AddListener(() => OnClickButton?.Invoke());
            button.onClick.AddListener(action);
            
            if (shouldSubscribeHighlight && button.TryGetComponent<HighlightElementId>(out var highlight)) {
                highlight.onActivate += action.Invoke;
            }
        }
    }
    public virtual void DestroyView()
    {
        Destroy(gameObject);
    }
}
