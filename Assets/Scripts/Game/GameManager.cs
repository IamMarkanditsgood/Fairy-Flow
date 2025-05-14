using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spawning")]
    [SerializeField] private GameObject chickenPrefab;
    [SerializeField] private Transform spawnArea;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private int maxChickens = 10;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text incomeText;

    private HashSet<int> unlockedLevels = new();
    [SerializeField] private GameObject modalPrefab;
    [SerializeField] private Sprite[] chickenSprites;
    public HashSet<int> GetUnlockedLevels() => unlockedLevels;

    public List<MergeChicken> Chickens => chickensOnScene;

    private float timer;
    private int coins;

    [SerializeField] private float incomeInterval = 1f; // кожну 1 секунду
    private float incomeTimer = 0f;

    private List<MergeChicken> chickensOnScene = new();

    bool isGameStarted;

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        isGameStarted = true;
        coins = PlayerPrefs.GetInt("Coins");
        Instance = this;
        LoadChickens(); // <== нове

        StartCoroutine(AutoSpawnChicken());
        UpdateCoinText();
        UpdateIncomeText();
    }
    private void LoadChickens()
    {
        var savedChickens = SaveLoadManager.LoadChickens();
        var savedLevels = SaveLoadManager.LoadUnlockedLevels();

        unlockedLevels = new HashSet<int>(savedLevels);

        if (savedChickens == null || savedChickens.Count == 0)
        {
            SpawnChicken(1);
            return;
        }

        foreach (var data in savedChickens)
        {
            SpawnChicken(data.Level, data.GetPosition());
        }
    }
    private void OnApplicationQuit()
    {
        SaveLoadManager.SaveChickens(chickensOnScene);
        PlayerPrefs.SetInt("Coins", coins);
    }
    private void Update()
    {
        if (isGameStarted)
        {
            timer += Time.deltaTime;
            float remaining = spawnInterval - (timer % spawnInterval);

            GameEvents.NewTime((int)remaining);

            HandleIncome();
        }
    }

    private void HandleIncome()
    {
        incomeTimer += Time.deltaTime;

        if (incomeTimer >= incomeInterval)
        {
            incomeTimer = 0f;

            int totalIncome = 0;
            foreach (var chicken in chickensOnScene)
            {
                totalIncome += chicken.GetProfitPerSecond();
            }
            coins = PlayerPrefs.GetInt("Coins");
            coins += totalIncome;
            PlayerPrefs.SetInt("Coins", coins);
            UpdateCoinText();
            UpdateIncomeText(); // <== Оновлюємо прибуток у UI
        }
    }

    private IEnumerator AutoSpawnChicken()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (chickensOnScene.Count < maxChickens)
            {
                SpawnChicken(1);
            }
        }
    }

    public void SpawnChicken(int level, Vector3? position = null)
    {
        Vector3 spawnPos = position ?? new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 0);
        GameObject newChicken = Instantiate(chickenPrefab, spawnPos, Quaternion.identity);
        var merge = newChicken.GetComponent<MergeChicken>();
        merge.Init(level);

        chickensOnScene.Add(merge);

        if (!unlockedLevels.Contains(level))
        {
            unlockedLevels.Add(level);
            if(level != 1 && level != 7)
            {
                NewChickenPopup newChickenPopup = (NewChickenPopup) UIManager.Instance.GetPopup(PopupTypes.NewChicken);
                newChickenPopup.Init(level);
                UIManager.Instance.ShowPopup(PopupTypes.NewChicken);
            }
            else if(level == 7)
            {
                Destroy(newChicken);
                UIManager.Instance.ShowPopup(PopupTypes.MaxChickecn);
                AddCoins(200);
            }
        }
    }

    public void RemoveChicken(MergeChicken chicken)
    {
        chickensOnScene.Remove(chicken);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        GameEvents.NewCoins(coins);
    }
    private void UpdateIncomeText()
    {
        int totalIncome = 0;
        foreach (var chicken in chickensOnScene)
        {
            totalIncome += chicken.GetProfitPerSecond();
        }

        GameEvents.NewReward(totalIncome);
    }
    public int GetMaxLevel() => chickenPrefab.GetComponent<MergeChicken>().MaxLevel;
}
