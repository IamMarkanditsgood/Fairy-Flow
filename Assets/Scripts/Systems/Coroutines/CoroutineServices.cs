using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineServices : MonoBehaviour, ICoroutineServices
{
    public static CoroutineServices instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public Coroutine StartRoutine(IEnumerator enumerator)
    {
        return StartCoroutine(enumerator);
    }

    public void StopRoutine(Coroutine routine)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
    }
}
