using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalInfo : BasicScreen
{

    public Image bg;
    public TMP_Text name;
    public TMP_Text description;
    public Button _back;

    private void Start()
    {
        _back.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        _back.onClick.RemoveListener(Close);
    }

    public void Init()
    {
        base.Init();
    }

    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {

    }

    private void Close()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Shop);
    }
}
