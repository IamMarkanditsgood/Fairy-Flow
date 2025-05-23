using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private Dictionary<SweetTypes, int> _sweetCounts = new();

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        foreach (SweetTypes sweet in Enum.GetValues(typeof(SweetTypes)))
        {
            _sweetCounts[sweet] = 0;
        }
    }
    public void AddSweet(SweetTypes sweet, int amount = 1)
    {
        if (!_sweetCounts.ContainsKey(sweet))
            _sweetCounts[sweet] = 0;

        _sweetCounts[sweet] += amount;
    }

    public void RemoveSweet(SweetTypes sweet, int amount = 1)
    {
        if (!_sweetCounts.ContainsKey(sweet))
            return;

        _sweetCounts[sweet] = Mathf.Max(0, _sweetCounts[sweet] - amount);
    }

    public int GetCountOfSweet(SweetTypes sweet)
    {
        return _sweetCounts.TryGetValue(sweet, out int count) ? count : 0;
    }
}
