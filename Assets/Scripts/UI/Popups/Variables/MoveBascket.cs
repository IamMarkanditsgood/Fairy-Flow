using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBascket : BasicPopup
{
    [SerializeField] private Game game;
    [SerializeField] private Sprite[] _sweetsImage;

    public Image sweetImage;
    public Button _okButton;

    private int sweetIndex;

    public void SetIndex(int index)
    {
        sweetIndex = index;
        sweetImage.sprite = _sweetsImage[index];
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
        UIManager.Instance.ShowScreen(ScreenTypes.Bascket);
    }

    private void PlaceToBascket()
    {
        SweetTypes sweet = (SweetTypes)sweetIndex;
        game.MoveToBascket(sweet);
        Hide();
    }
}
