using System.Collections.Generic;

public interface IStorage
{
    void TryAdd(string id, byte Value);
    void Reset();
    IList<CardItem> GetAll();
}