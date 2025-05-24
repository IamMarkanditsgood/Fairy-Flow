using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : BasicScreen
{
    [SerializeField] private GamePlayManager _gamePlayManager;

    [SerializeField] private Button _bascketButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _energyButton;
    [SerializeField] private Button _close;

    [SerializeField] private TMP_Text _energyText;
    [SerializeField] private TMP_Text _coinsText;

    private const int maxEnergy = 120;
    private const string energyKey = "Energy";
    private const string lastExitKey = "LastExitTime";

    public override void Init()
    {
        base.Init();

    }

    public void Start()
    {
        
        _gamePlayManager.Subscibe();

        _bascketButton.onClick.AddListener(BascketOpen);
        _shopButton.onClick.AddListener(ShopOpen);
        _energyButton.onClick.AddListener(EnergyPressed);
        _close.onClick.AddListener(Close);

        GameEvents.OnEnergyUpdate += UpdateEnergy;
        GameEvents.OnCoinsUpdate += UpdateCoins;
        RestoreEnergy();
        _gamePlayManager.StartGame();
    }

    private void OnDestroy()
    {
        _gamePlayManager.UnSubscribe();

        _bascketButton.onClick.RemoveListener(BascketOpen);
        _shopButton.onClick.RemoveListener(ShopOpen);
        _energyButton.onClick.RemoveListener(EnergyPressed);
        _close.onClick.RemoveListener(Close);

        GameEvents.OnEnergyUpdate -= UpdateEnergy;
        GameEvents.OnCoinsUpdate -= UpdateCoins;
    }
    private void RestoreEnergy()
    {
        int currentEnergy = PlayerPrefs.GetInt(energyKey, maxEnergy); // Якщо немає значення — максимум

        if (PlayerPrefs.HasKey(lastExitKey))
        {
            string lastExitString = PlayerPrefs.GetString(lastExitKey);
            DateTime lastExitTime = DateTime.Parse(lastExitString);
            TimeSpan timePassed = DateTime.UtcNow - lastExitTime;

            int hoursPassed = Mathf.FloorToInt((float)timePassed.TotalHours);
            if (hoursPassed > 0)
            {
                currentEnergy = Mathf.Min(currentEnergy + hoursPassed, maxEnergy);
                PlayerPrefs.SetInt(energyKey, currentEnergy);
            }
        }
        PlayerPrefs.SetInt(energyKey, currentEnergy);
        Debug.Log("Energy restored: " + currentEnergy);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetInt(energyKey, maxEnergy));
        GameEvents.UpdateEnergy();  
    }

    void OnApplicationQuit()
    {
        SaveExitTime();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveExitTime();
    }

    private void SaveExitTime()
    {
        PlayerPrefs.SetString(lastExitKey, DateTime.UtcNow.ToString());
        PlayerPrefs.Save();
    }

    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
        _gamePlayManager.SetScreen();
        UpdateCoins();
        UpdateEnergy();
    }

    public void MoveToBascket(SweetTypes sweet)
    {
        _gamePlayManager.RemoveSweet(sweet);
    }

    public void AddSweet(SweetTypes sweet)
    {
        _gamePlayManager.AddSweet(sweet);
    }

    private void UpdateCoins()
    {
        _coinsText.text = PlayerPrefs.GetInt("Coins").ToString();
    }

    private void UpdateEnergy()
    {
        Debug.Log(PlayerPrefs.GetInt(energyKey, maxEnergy));
        _energyText.text = PlayerPrefs.GetInt(energyKey, maxEnergy).ToString();
    }

    private void EnergyPressed()
    {

    }
    private void BascketOpen()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Bascket);
    }
    private void ShopOpen()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Shop);
    }

    private void Close()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Home);
    }


}

[Serializable]
public class GamePlayManager
{
    [Serializable]
    public class SweetData
    {
        public SweetTypes sweetType;
        public Sprite image;
        public int coinReward;
    }

    [Serializable]
    public class CellData
    {
        public int colum;
        public int row;
        public GameObject cell;
        public SweetTypes currentSweet = SweetTypes.None;
    }

    [Serializable]
    public class TaskData
    {
        public SweetTypes[] sweets;
        public Image[] sweetImges;
        public Image[] doneImages;
        public TMP_Text[] countText;
        public int[] amount;
        public bool[] isDone;

        public TMP_Text coinsRewardText;
        public TMP_Text energyRewardText;

        public int coinsReward;
        public int energyReward;
    }

    [SerializeField] private SweetData[] _sweets;
    [SerializeField] private CellData[] _cells;
    [SerializeField] private TaskData[] _tasks;

    private Vector2 _currentSelectedCell = new Vector2(-1, -1);


    public void Subscibe()
    {
        for(int i = 0; i < _cells.Length; i++)
        {
            int index = i;
            _cells[index].cell.GetComponent<Button>().onClick.AddListener(()=> CellPressed(new Vector2(_cells[index].colum, _cells[index].row)));
        }
    }

    public void UnSubscribe()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            int index = i;
            _cells[index].cell.GetComponent<Button>().onClick.RemoveListener(() => CellPressed(new Vector2(_cells[index].colum, _cells[index].row)));
        }
    }

    public void SetScreen()
    {
        CheckTasks();
    }

    public void StartGame()
    {
        if (PlayerPrefs.HasKey("FirstRun"))
        {
            LoadGame();
        }
        else
        {
            SetGame();
        }
    }

    public void RemoveSweet(SweetTypes sweet)
    {
        foreach(var cell in _cells)
        {
            if(cell.currentSweet == sweet)
            {
                ToggleCell(cell);
                return;
            }
        }
    }

    public void AddSweet(SweetTypes sweet)
    {
        foreach (var cell in _cells)
        {
            if (cell.currentSweet == SweetTypes.None)
            {
                ToggleCell(cell, sweet, true);
                return;
            }
        }
    }

    private void LoadGame()
    {
    }

    private void SetGame()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            if (UnityEngine.Random.value > 0.5f)
            {
                SweetTypes randomSweet = GetRandomSweet();
                ToggleCell(_cells[i], randomSweet, true);       
            }
        }
        SetTasks();

        CheckTasks();
    }

    // Tasks
    private void SetTasks()
    {
        foreach(var task in _tasks)
        {
            SetTask(task);
        }
    }

    private void SetTask(TaskData task)
    {
        task.coinsReward = 0;
        task.energyReward = 0;
        for (int i = 0; i < task.sweets.Length; i++)
        {
            task.sweets[i] = GetRandomSweet();
            task.sweetImges[i].sprite = GetSweetSprite(task.sweets[i]);
            task.doneImages[i].enabled = false;
            task.amount[i] = UnityEngine.Random.Range(1, 11);
            task.countText[i].text = task.amount[i].ToString();

            task.isDone[i] = false;

            task.coinsReward += task.amount[i];
            task.energyReward += task.amount[i] / 2;

            task.coinsReward *= 2;

            task.coinsRewardText.text = task.coinsReward.ToString();
            task.energyRewardText.text = task.energyReward.ToString();
        }
        if (UIManager.Instance.GetScreen(ScreenTypes.Game).isActive)
        {
            CheckTasks();
        }
    }

    private void CheckTasks()
    {
        for(int i = 0; i < _tasks.Length; i++)
        {
            for (int j =0; j < _tasks[i].sweets.Length; j++ )
            {
                if (InventoryManager.Instance.GetCountOfSweet(_tasks[i].sweets[j]) == _tasks[i].amount[j])
                {
                    _tasks[i].isDone[j] = true;
                    _tasks[i].doneImages[j].enabled = true;
                }
            }
        }

        for (int i = 0; i < _tasks.Length; i++)
        {
            int correct = 0;
            for (int j = 0; j < _tasks[i].sweets.Length; j++)
            {
                if (_tasks[i].isDone[j])
                {
                    correct++;
                }
            }
            if(correct == _tasks[i].sweets.Length && UIManager.Instance.GetScreen(ScreenTypes.Game).isActive)
            {
                GiveReward(_tasks[i]);
                SetTask(_tasks[i]);
            }
        }
    }

    private void GiveReward(TaskData taskData)
    {
        TaskPopup taskPopup = (TaskPopup)UIManager.Instance.GetPopup(PopupTypes.RewardPopup);

        List<Sprite> sweetSprites = new List<Sprite>();
        foreach(var sweet in taskData.sweets)
        {
            sweetSprites.Add(GetSweetSprite(sweet));
        }

        List<int> amounts = new List<int>();
        amounts.AddRange(taskData.amount);
        taskPopup.SetPopup(sweetSprites, amounts, taskData.coinsReward, taskData.energyReward);

        taskPopup.Show();

        int coins = PlayerPrefs.GetInt("Coins");
        coins += taskData.coinsReward;
        PlayerPrefs.SetInt("Coins", coins);
        GameEvents.UpdateCoins();

        int energy = PlayerPrefs.GetInt("Energy", 120);
        energy += taskData.energyReward;
        PlayerPrefs.SetInt("Energy", energy);
        GameEvents.UpdateEnergy();
    }

    private void CellPressed(Vector2 coordinates)
    {
        CellData cell = GetCell(coordinates);

        if ((_currentSelectedCell.x == -1 && _currentSelectedCell.y == -1) || coordinates == _currentSelectedCell)
        {
            _currentSelectedCell = coordinates;
            AnimateScaleUp(cell.cell.transform);
        }
        else
        {
            CellData prevCell = GetCell(_currentSelectedCell);
            if (cell.currentSweet == prevCell.currentSweet)
            {
                ToggleCell(prevCell);
                
                SweetTypes newSweet = GetNextEnumValue(cell.currentSweet);

                if(newSweet == SweetTypes.None)
                {
                    int reward = GetSweetReward(SweetTypes.Candy16);
                    reward += PlayerPrefs.GetInt("Coins");
                    PlayerPrefs.SetInt("Coins", reward);
                    GameEvents.UpdateCoins();

                    
                }

                ToggleCell(cell, newSweet);

                AnimateScale(cell.cell.transform);
            }
            else
            {
                AnimateScale(cell.cell.transform);
                AnimateScale(prevCell.cell.transform);
            }

            _currentSelectedCell = new Vector2(-1, -1);
        }
    }



    private void ToggleCell(CellData cell, SweetTypes sweet = SweetTypes.None, bool isBought = false)
    {
        InventoryManager.Instance.RemoveSweet(cell.currentSweet);
        InventoryManager.Instance.AddSweet(sweet);

        cell.cell.transform.localScale = Vector3.one;

        if (sweet != SweetTypes.None)
        {
            cell.cell.GetComponent<Button>().enabled = true;
            cell.cell.GetComponent<Image>().enabled = true;
            cell.currentSweet = sweet;
            cell.cell.GetComponent<Image>().sprite = GetSweetSprite(cell.currentSweet);
            
            if (!isBought)
            {
                PlayerPrefs.SetInt("Achieve", 1);
                int reward = GetSweetReward(sweet);
                reward += PlayerPrefs.GetInt("Coins");
                PlayerPrefs.SetInt("Coins", reward);
                ChangeEnergy();
                GameEvents.UpdateCoins();
            }

            if (UIManager.Instance.GetScreen(ScreenTypes.Game).isActive)
            {
                CheckTasks();
            }

        }
        else
        {
            cell.cell.GetComponent<Button>().enabled = false;
            cell.cell.GetComponent<Image>().enabled = false;
            cell.currentSweet = sweet;
        }

    }

    private void ChangeEnergy()
    {
        int energy = PlayerPrefs.GetInt("Energy", 120);
        energy--;
        PlayerPrefs.SetInt("Energy", energy);
        GameEvents.UpdateEnergy();
    }

    private Sprite GetSweetSprite(SweetTypes sweetType)
    {
        foreach (var sweet in _sweets)
        {
            if (sweetType == sweet.sweetType)
            {
                return sweet.image;
            }
        }

        return null;
    }
    private int GetSweetReward(SweetTypes sweetType)
    {
        foreach (var sweet in _sweets)
        {
            if (sweetType == sweet.sweetType)
            {
                return sweet.coinReward;
            }
        }

        return 0;
    }

    private SweetTypes GetRandomSweet()
    {
        SweetTypes[] sweetTypes = Enum.GetValues(typeof(SweetTypes))
        .Cast<SweetTypes>()
        .Where(s => s != SweetTypes.None)
        .ToArray();

        return sweetTypes[UnityEngine.Random.Range(0, sweetTypes.Length)];
    }

    private CellData GetCell(Vector2 coordinates)
    {
        foreach(var cell in _cells)
        {
            if(cell.colum == coordinates.x && cell.row == coordinates.y )
            {
                return cell;    
            }
        }
        return null;
    }
    public static TEnum GetNextEnumValue<TEnum>(TEnum current) where TEnum : Enum
    {
        TEnum[] values = (TEnum[])Enum.GetValues(typeof(TEnum));
        int index = Array.IndexOf(values, current);
        index = (index + 1) % values.Length;
        return values[index];
    }

    //Animations
    public void AnimateScale(Transform target)
    {
        CoroutineServices.instance.StartRoutine(ScaleUpDown(target));
    }

    public void AnimateScaleUp(Transform target)
    {
        CoroutineServices.instance.StartRoutine(ScaleUp(target, 1f, 1.3f, 0.2f));
    }

    public void AnimateScaleDown(Transform target)
    {
        CoroutineServices.instance.StartRoutine(ScaleDown(target, 1.3f, 1f, 0.2f));
    }

    private IEnumerator ScaleUpDown(Transform target)
    {
        yield return CoroutineServices.instance.StartRoutine(ScaleUp(target, 1f, 1.3f, 0.2f));
        yield return CoroutineServices.instance.StartRoutine(ScaleDown(target, 1.3f, 1f, 0.2f));
    }

    private IEnumerator ScaleUp(Transform target, float from, float to, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            float t = timer / duration;
            float scale = Mathf.Lerp(from, to, t);
            target.localScale = Vector3.one * scale;
            timer += Time.deltaTime;
            yield return null;
        }
        target.localScale = Vector3.one * to;
    }

    private IEnumerator ScaleDown(Transform target, float from, float to, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            float t = timer / duration;
            float scale = Mathf.Lerp(from, to, t);
            target.localScale = Vector3.one * scale;
            timer += Time.deltaTime;
            yield return null;
        }
        target.localScale = Vector3.one * to;
    }
}

public enum SweetTypes
{
    Candy1,
    Candy2,
    Candy3,
    Candy4,
    Candy5,
    Candy6,
    Candy7,
    Candy8,
    Candy9,
    Candy10,
    Candy11,
    Candy12,
    Candy13,
    Candy14,
    Candy15,
    Candy16,
    None,

}