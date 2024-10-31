using System.Collections.Generic;
using Project.Tutorial;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractBackgroundView : MonoBehaviour
{
    protected Dictionary<string, GeneratorButton> _generatorButtons;

    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        canvas.worldCamera = cam;
    }
    public void SubscribeButton(string buttonName, UnityAction action, bool shouldSubscribeHighlight = false)
    {
        if (_generatorButtons.TryGetValue(buttonName, out var button))
        {
            button.CollectButton.onClick.AddListener(action);
            
            if (shouldSubscribeHighlight && button.CollectButton.TryGetComponent<HighlightElementId>(out var highlight)) {
                highlight.onActivate += action.Invoke;
            }
        }
    }
    public abstract void CreateDictionaries();
    public abstract void Initialize(GeneratorManager generatorManager);
    protected void UnsubscribeGenerators()
    {
        foreach (var item in _generatorButtons)
        {
            item.Value.Unsubscribe();
        }
    }
    public void DestroyView()
    {
        UnsubscribeGenerators();
        Destroy(gameObject);
    }
}
