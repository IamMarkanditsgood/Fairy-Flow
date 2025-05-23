using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bascket : BasicScreen
{
    [SerializeField] private TMP_Text[] _amountText;
    [SerializeField] private TMP_Text _energyText;
    [SerializeField] private TMP_Text _coinsText;

    [SerializeField] private Button[] candyButtons;

    [SerializeField] private Button _backButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _energyButton;

    public void Start()
    {
        GameEvents.OnEnergyUpdate += UpdateEnergy;
        GameEvents.OnCoinsUpdate += UpdateCoins;

        for (int i = 0; i < candyButtons.Length; i++)
        {
            int index = i;
            candyButtons[index].onClick.AddListener(() => PlaceToBascket(index));
        }

        _backButton.onClick.AddListener(BackPressed);
        _shopButton.onClick.AddListener(ShopPressed);
        _energyButton.onClick.AddListener(EnergyPressed);
    }

    private void OnDestroy()
    {

        GameEvents.OnEnergyUpdate -= UpdateEnergy;
        GameEvents.OnCoinsUpdate -= UpdateCoins;

        for (int i = 0; i < candyButtons.Length; i++)
        {
            int index = i;
            candyButtons[index].onClick.RemoveListener(() => PlaceToBascket(index));
        }

        _backButton.onClick.RemoveListener(BackPressed);
        _shopButton.onClick.RemoveListener(ShopPressed);
        _energyButton.onClick.RemoveListener(EnergyPressed);
    }
    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
        ConfigureScreen();
    }

    private void ConfigureScreen()
    {
        SweetTypes[] sweetTypes = (SweetTypes[])Enum.GetValues(typeof(SweetTypes));

        for (int i = 0; i < _amountText.Length; i++)
        {
            _amountText[i].text = InventoryManager.Instance.GetCountOfSweet(sweetTypes[i]).ToString();
        }
        UpdateCoins();
        UpdateEnergy();
    }
    private void UpdateCoins()
    {
        _coinsText.text = PlayerPrefs.GetInt("Coins").ToString();
    }

    private void UpdateEnergy()
    {
        _energyText.text = PlayerPrefs.GetInt("Energy", 120).ToString();
    }

    private void PlaceToBascket(int index)
    {
        SweetTypes[] sweetTypes = (SweetTypes[])Enum.GetValues(typeof(SweetTypes));

        if (InventoryManager.Instance.GetCountOfSweet(sweetTypes[index]) > 0)
        {
            MoveBascket moveBascket = (MoveBascket)UIManager.Instance.GetPopup(PopupTypes.BascketPopup);
            moveBascket.Show();
            moveBascket.SetIndex(index);
        }
    }

    private void BackPressed()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game);
    }

    private void EnergyPressed()
    {

    }

    private void ShopPressed()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Shop);
    }
}
