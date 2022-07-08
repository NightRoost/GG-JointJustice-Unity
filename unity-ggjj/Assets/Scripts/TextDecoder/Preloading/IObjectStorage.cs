using System.Collections.Generic;
using UnityEngine;

public interface IObjectStorage
{
    T GetObject<T>(string objectName) where T : class;
    IEnumerable<T> GetObjectsOfType<T>() where T : class;

    void Add(Object obj);
    bool Contains(Object obj);
}