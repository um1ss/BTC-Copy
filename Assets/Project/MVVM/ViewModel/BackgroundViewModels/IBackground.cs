using Balancy.Models;
using Cysharp.Threading.Tasks;

public interface IBackground 
{
    BackgroundType Type { get; }
    UniTask Show();
    void Close();
}
