using System;
using UnityEngine;

// Use on Prefab
public class PoolObject : MonoBehaviour, IPoolable<PoolObject>
{
    private Action<PoolObject> _returnToPool;

    // OnDisable is called when the object is disabled
    private void OnDisable()
    {
        ReturnToPool();
    }

    // Initialize the object with a return action
    public void Initialize(Action<PoolObject> returnAction)
    {
        // Cache reference to the return action
        this._returnToPool = returnAction;
    }

    // Return the object to the pool
    public void ReturnToPool()
    {
        // Invoke and return this object to the pool
        _returnToPool?.Invoke(this);
    }
}
