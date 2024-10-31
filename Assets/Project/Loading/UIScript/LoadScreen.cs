using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class LoadScreen : MonoBehaviour
{
    private FadeAnimation _fadeAnimation;

    private Canvas _canvas;

    private void Awake()
    {
        _fadeAnimation = GetComponentInChildren<FadeAnimation>();
        _canvas = GetComponent<Canvas>();
    }
    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        canvas.worldCamera = cam;

        DontDestroyOnLoad(this);
    }
    public async UniTask Load(Queue<ILoadingOperation> loadingOperations)
    {
        //await _fadeAnimation.FadeIn();

        foreach (var operation in loadingOperations)
        {
            await operation.Load();
        }

        //await _fadeAnimation.FadeOut();
    }
}
