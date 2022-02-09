public class TrackedCollection<T>
{
    public IList<T> Values { get; private set; } = new List<T>();

    public async Task SetValues(params T[] values)
    {
        Values = values.ToList();
        if (OnChanged != null) await OnChanged.Invoke();
    }

    public event Func<Task>? OnChanged;
}