namespace IntelligenceReporting.WebApp;

public class Tracked<T> where T : struct
{
    public T? Value { get; private set; }

    public async Task SetValue(T? value)
    {
        Value = value;
        if (OnChanged != null) await OnChanged.Invoke();
    }

    public event Func<Task>? OnChanged;
}