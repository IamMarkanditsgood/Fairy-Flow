using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : BasicScreen
{
    [SerializeField] private Button _closeButton;
    private void Start()
    {
        _closeButton.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveListener(Close);
    }

    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
    }
    private void Close()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Profile);
    }
}
