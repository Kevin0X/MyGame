using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> {
    List<T> pool = new List<T>();

    public void Push(T obj)
    {
        if (obj != null)
        {
            pool.Add(obj);
        }
    }
	
    public T Pop()
    {
        T obj = default(T);
        obj = pool[pool.Count - 1];
        pool.RemoveAt(pool.Count - 1);
        return obj;
    }

    public int GetCount()
    {
        return pool.Count;
    }
}
