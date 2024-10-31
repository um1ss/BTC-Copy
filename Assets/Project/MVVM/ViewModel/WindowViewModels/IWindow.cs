using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public interface IWindow
{
    event UnityAction<WindowTypes> OnAnotherWindowOpen;
    WindowTypes Type { get; }
    void Show();
    UniTask Initialize();
}
