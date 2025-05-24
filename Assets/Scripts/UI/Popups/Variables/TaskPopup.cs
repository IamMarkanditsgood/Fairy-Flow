using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskPopup : BasicPopup
{
    [SerializeField] private Image[] _sweetsImage;
    [SerializeField] private TMP_Text[] _amountText;
    [SerializeField] private TMP_Text _coinReward;
    [SerializeField] private TMP_Text _energyReward;

    public Button _okButton;

    public void SetPopup(List<Sprite> sweets, List<int> amounts, int coins, int energy)
    {
        for(int i = 0; i < sweets.Count; i++)
        {
            _sweetsImage[i].sprite = sweets[i];
            _amountText[i].text =  amounts[i].ToString();
        }
        _coinReward.text = "+" + coins;
        _energyReward.text = "+" + energy;
    }

    public override void Subscribe()
    {
        base.Subscribe();
        _okButton.onClick.AddListener(Hide);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        _okButton.onClick.RemoveListener(Hide);
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
        UIManager.Instance.ShowScreen(ScreenTypes.Game);
    }

    
}
