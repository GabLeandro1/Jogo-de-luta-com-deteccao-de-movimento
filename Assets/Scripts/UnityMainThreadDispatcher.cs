using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();
    private static UnityMainThreadDispatcher _instance;

    // Usar Awake para garantir que o objeto seja criado na thread principal
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Para que o objeto persista entre as cenas
        }
        else
        {
            Destroy(gameObject); // Destruir o objeto duplicado
        }
    }

    public static UnityMainThreadDispatcher Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UnityMainThreadDispatcher is not initialized. Please add it to a GameObject in the scene.");
            }
            return _instance;
        }
    }

    private void Update()
    {
        while (_executionQueue.Count > 0)
        {
            _executionQueue.Dequeue().Invoke();
        }
    }

    public void Enqueue(Action action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }
}
