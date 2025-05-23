using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopup : BasicPopup
{
    [SerializeField] private Game game;
    [SerializeField] private Sprite[] _sweetsImage;
    [SerializeField] private int[] _sweetsPrices;

    public Image sweetImage;
    public TMP_Text sweetPriceText;
    public Button _okButton;

    private int sweetIndex;

    public void SetIndex(int index)
    {
        sweetIndex = index;
        sweetImage.sprite = _sweetsImage[index];
        sweetPriceText.text = _sweetsPrices[index].ToString();
    }

    public override void Subscribe()
    {
        base.Subscribe();
        _okButton.onClick.AddListener(PlaceToBascket);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        _okButton.onClick.RemoveListener(PlaceToBascket);
    }
    public override void ResetPopup()
    {
    }

    public override void SetPopup()
    {
    }

    public override void Hide()
    {
        base.Hide();
        UIManager.Instance.ShowScreen(ScreenTypes.Shop);
    }

    private void PlaceToBascket()
    {
        if (PlayerPrefs.GetInt("Coins") >= _sweetsPrices[sweetIndex])
        {
            int newCoins = PlayerPrefs.GetInt("Coins");
            newCoins -= _sweetsPrices[sweetIndex];
            PlayerPrefs.SetInt("Coins", newCoins);

            SweetTypes sweet = (SweetTypes)sweetIndex;
            game.AddSweet(sweet);
            Hide();
        }
    }
}