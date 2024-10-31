using R3;
using System;
using System.Collections.Generic;

public abstract class AbstractChatModel : AbstractModel<int>
{
    protected Dictionary<string, ReactiveProperty<string>> _currentSelectSection = new();
    public abstract void SetDelegateString(string name, Action<string> del);
    public abstract void ChangeValueString(string name, string newValue);
}
