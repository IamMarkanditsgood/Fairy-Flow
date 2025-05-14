using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance;

    [SerializeField] private GameObject chickenPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void TryMerge(MergeChicken a, MergeChicken b)
    {
        if (a.Level != b.Level) return;

        Vector3 spawnPos = (a.transform.position + b.transform.position) / 2f;

        GameManager.Instance.RemoveChicken(a);
        GameManager.Instance.RemoveChicken(b);
        Destroy(a.gameObject);
        Destroy(b.gameObject);

        GameManager.Instance.SpawnChicken(a.Level + 1, spawnPos);
    }
}
