using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
{
    public ObjectPool(GameObject pooledObject, int numToSpawn = 0)
    {
        this._prefab = pooledObject;
        Spawn(numToSpawn);
    }

    public ObjectPool(GameObject pooledObject, Action<T> pullObject, Action<T> pushObject, int numToSpawn = 0)
    {
        this._prefab = pooledObject;
        this._pullObject = pullObject;
        this._pushObject = pushObject;
        Spawn(numToSpawn);
    }

    private System.Action<T> _pullObject;
    private System.Action<T> _pushObject;
    private Stack<T> _pooledObjects = new Stack<T>();
    private GameObject _prefab;

    // Get the count of objects in the pool
    public int pooledCount
    {
        get
        {
            return _pooledObjects.Count;
        }
    }

    // Pull an object from the pool or instantiate a new one
    public T Pull()
    {
        T t;
        if (pooledCount > 0)
            t = _pooledObjects.Pop();
        else
            t = GameObject.Instantiate(_prefab).GetComponent<T>();

        t.gameObject.SetActive(true); // Ensure the object is on
        t.Initialize(Push);

        // Allow default behavior and turn the object back on
        _pullObject?.Invoke(t);

        return t;
    }

    // Pull an object from the pool and set its position
    public T Pull(Vector3 position)
    {
        T t = Pull();
        t.transform.position = position;
        return t;
    }

    // Pull an object from the pool and set its position and rotation
    public T Pull(Vector3 position, Quaternion rotation)
    {
        T t = Pull();
        t.transform.position = position;
        t.transform.rotation = rotation;
        return t;
    }

    // Pull an object from the pool and return it as a GameObject
    public GameObject PullGameObject()
    {
        return Pull().gameObject;
    }

    // Pull an object from the pool, set its position, and return it as a GameObject
    public GameObject PullGameObject(Vector3 position)
    {
        GameObject go = Pull().gameObject;
        go.transform.position = position;
        return go;
    }

    // Pull an object from the pool, set its position and rotation, and return it as a GameObject
    public GameObject PullGameObject(Vector3 position, Quaternion rotation)
    {
        GameObject go = Pull().gameObject;
        go.transform.position = position;
        go.transform.rotation = rotation;
        return go;
    }

    // Push an object back into the pool
    public void Push(T t)
    {
        _pooledObjects.Push(t);

        // Create default behavior to turn off objects
        _pushObject?.Invoke(t);

        t.gameObject.SetActive(false);
    }

    // Spawn a specified number of objects in the pool
    private void Spawn(int number)
    {
        T t;

        for (int i = 0; i < number; i++)
        {
            t = GameObject.Instantiate(_prefab).GetComponent<T>();
            _pooledObjects.Push(t);
            t.gameObject.SetActive(false);
        }
    }
}

// Interface for object pooling
public interface IPool<T>
{
    T Pull();
    void Push(T t);
}

// Interface for poolable objects
public interface IPoolable<T>
{
    void Initialize(System.Action<T> returnAction);
    void ReturnToPool();
}
