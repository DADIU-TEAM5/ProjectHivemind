using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class RuntimeSet<T> : ScriptableObject
{

    public List<T> Items = new List<T>();

    public abstract void Add(T t);
    

    public abstract void Remove(T t);
    

}