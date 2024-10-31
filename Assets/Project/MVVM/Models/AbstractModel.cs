using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;

public abstract class AbstractModel<T> 
{
    protected Dictionary<string, ReactiveProperty<T>> _values = new();
    protected Dictionary<string, IDisposable> _disposes = new();

    public abstract void SetDelegate(string name, Action<T> del);
    public abstract void ChangeValue(string name, T newValue);
    public T GetValue(string name)
    {
        if (_values.TryGetValue(name, out var value))
        {
            return value.Value;
        }
        return default;
    }
    public virtual void Initialize()
    {

    }
}
