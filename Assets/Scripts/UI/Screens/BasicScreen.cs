using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicScreen : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private ScreenTypes _screenType;

    public bool isActive;

    public ScreenTypes ScreenType => _screenType;

    public virtual void Init()
    {
    }

    public virtual void Show()
    {
        SetScreen();
        _view.SetActive(true);
        isActive = true;
    }
    public virtual void Hide() 
    {
        ResetScreen();
        isActive = false;
        _view.SetActive(false);
    }

    public abstract void SetScreen();

    public abstract void ResetScreen();
}
