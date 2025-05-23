using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Game : BasicScreen
{
    [SerializeField] private GamePlayManager _gamePlayManager;

    [SerializeField] private Button _bascketButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _energyButton;

    [SerializeField] private TMP_Text _energyText;
    [SerializeField] private TMP_Text _coinsText;

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

        GameEvents.OnEnergyUpdate += UpdateEnergy;
        GameEvents.OnCoinsUpdate += UpdateCoins;

        _gamePlayManager.StartGame();
    }

    private void OnDestroy()
    {
        _gamePlayManager.UnSubscribe();

        _bascketButton.onClick.RemoveListener(BascketOpen);
        _shopButton.onClick.RemoveListener(ShopOpen);
        _energyButton.onClick.RemoveListener(EnergyPressed);

        GameEvents.OnEnergyUpdate -= UpdateEnergy;
        GameEvents.OnCoinsUpdate -= UpdateCoins;
    }

    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {

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
        _energyText.text = PlayerPrefs.GetInt("Energy",120).ToString();
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


}

[Serializable]
public class GamePlayManager
{
    [Serializable]
    public class SweetData
    {
        public SweetTypes sweetType;
        public Sprite image;
    }

    [Serializable]
    public class CellData
    {
        public int colum;
        public int row;
        public GameObject cell;
        public SweetTypes currentSweet = SweetTypes.None;
    }

    [SerializeField] private SweetData[] _sweets;
    [SerializeField] private CellData[] _cells;


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
                ToggleCell(cell, sweet);
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
                ToggleCell(_cells[i], randomSweet);       
            }
        }

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
    private void ToggleCell(CellData cell, SweetTypes sweet = SweetTypes.None)
    {
        InventoryManager.Instance.RemoveSweet(cell.currentSweet);
        InventoryManager.Instance.AddSweet(sweet);

        Debug.Log("Remove " + cell.currentSweet);
        Debug.Log("Add " + sweet);

        if(sweet != SweetTypes.None)
        {
            cell.cell.GetComponent<Button>().enabled = true;
            cell.cell.GetComponent<Image>().enabled = true;
            cell.currentSweet = sweet;
            cell.cell.GetComponent<Image>().sprite = GetSweetSprite(cell.currentSweet);
        }
        else
        {
            cell.cell.GetComponent<Button>().enabled = false;
            cell.cell.GetComponent<Image>().enabled = false;
            cell.currentSweet = sweet;
        }

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