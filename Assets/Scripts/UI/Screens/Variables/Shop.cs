using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : BasicScreen
{
    [SerializeField] private TMP_Text _energyText;
    [SerializeField] private TMP_Text _coinsText;

    [SerializeField] private Button[] _buttons;


    [SerializeField] private Button _backButton;
    [SerializeField] private Button _bascketButton;
    [SerializeField] private Button _energyButton;

    public void Start()
    {
        GameEvents.OnEnergyUpdate += UpdateEnergy;
        GameEvents.OnCoinsUpdate += UpdateCoins;

        _backButton.onClick.AddListener(BackPressed);
        _bascketButton.onClick.AddListener(BascketPressed);
        _energyButton.onClick.AddListener(EnergyPressed);

        for(int i = 0; i < _buttons.Length; i++)
        {
            int index = i;
            _buttons[index].onClick.AddListener(() => BuyNewSweet(index));
        }
    }

    private void OnDestroy()
    {
        GameEvents.OnEnergyUpdate -= UpdateEnergy;
        GameEvents.OnCoinsUpdate -= UpdateCoins;

        _backButton.onClick.RemoveListener(BackPressed);
        _bascketButton.onClick.RemoveListener(BascketPressed);
        _energyButton.onClick.RemoveListener(EnergyPressed);

        for (int i = 0; i < _buttons.Length; i++)
        {
            int index = i;
            _buttons[index].onClick.RemoveListener(() => BuyNewSweet(index));
        }
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

    private void BuyNewSweet(int index)
    {
        ShopPopup shopPopup =(ShopPopup) UIManager.Instance.GetPopup(PopupTypes.ShopPopup);
        shopPopup.SetIndex(index);
        shopPopup.Show();
    }

    private void BackPressed()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game);
    }

    private void EnergyPressed()
    {

    }

    private void BascketPressed()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Bascket);
    }
}
