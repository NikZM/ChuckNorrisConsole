namespace GetBusy.ChuckNorrisApi;

public class InMemoryHistory<T> : IHistoryService<T>
{
    private int index = 0;
    private int? maxBuffer;
    private IList<T> storage = new List<T>();
    public bool HasNext => index < storage.Count - 1;
    public bool HasPrevious => index > 0;

    public InMemoryHistory() { }
    public InMemoryHistory(int maxBuffer)
    {
        this.maxBuffer = maxBuffer;
    }

    public void Append(T data)
    {
        storage.Add(data);
        if (maxBuffer != null && storage.Count > maxBuffer)
        {
            storage.RemoveAt(0);
        }
        index = storage.Count - 1;
    }

    public T? Next()
    {
        if (!HasNext)
        {
            return default(T);
        }
        return storage[++index];
    }

    public T? Previous()
    {
        if (!HasPrevious)
        {
            return default(T);
        }
        return storage[--index];
    }
}