using System.Collections.Concurrent;
using System.Collections.Generic;

public class MemoryStorage : IStorage
{
    private ConcurrentDictionary<string, byte> _storage = new ConcurrentDictionary<string, byte>();

    public IList<CardItem> GetAll()
    {
        var result = new List<CardItem>();
        foreach (var item in _storage)
        {
            result.Add(new CardItem
            {
                Id = item.Key,
                Value = item.Value
            });
        }
        return result;
    }

    public void Reset()
    {
        _storage.Clear();
    }

    public void TryAdd(string id, byte value)
    {
        var isExistingItem = _storage.ContainsKey(id);
        byte oldValue = 0;
        if (isExistingItem)
        {
            _storage.Remove(id, out oldValue);
        }
        _storage.TryAdd(id, value);
    }
}