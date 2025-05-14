using System.Collections;
using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject chickenPrefab;

    bool isGameStarted;
    int houses;

    int time = 10;
    int timer;
    

    public void StartGame()
    {
        isGameStarted = true;
    }

    private void Update()
    {
        if (houses < 10)
        {
            houses++;
            Vector3 pos = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 0);
            GameObject go = Instantiate(chickenPrefab, pos, Quaternion.identity);
            go.GetComponent<MergeChicken>().Init(Random.Range(1, 3));
        }
    }
}
