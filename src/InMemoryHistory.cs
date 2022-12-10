using Microsoft.Extensions.Configuration;

namespace GetBusy.ChuckNorrisApi;

public class InMemoryHistory<T> : IHistoryService<T>
{
    private int index = 0;
    private int? maxBuffer;
    private IList<T> storage = new List<T>();
    public bool HasNext => index < storage.Count - 1;
    public bool HasPrevious => index > 0;

    public InMemoryHistory(IConfiguration configuration)
    {
        this.maxBuffer = configuration.GetValue<int?>("maxHistory");
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
        if (storage.Count == 0)
        {
            return default(T);
        }
        return storage[this.HasNext ? ++index : storage.Count - 1];
    }

    public T? Previous()
    {
        if (storage.Count == 0)
        {
            return default(T);
        }
        return storage[this.HasPrevious ? --index : 0];
    }
}