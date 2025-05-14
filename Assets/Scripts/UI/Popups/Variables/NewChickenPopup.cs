using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewChickenPopup : BasicPopup
{
    private int _level;

    public Image mainChicken;
    public Image[] chickens;

    public Sprite[] openedChickens;
    public Sprite[] closedChickens;

    public void Init(int level)
    {
        _level = level;
        if (_level == 2)
        {
            PlayerPrefs.SetInt("Achieve0", 1);
        }
        if (_level == 5)
        {
            PlayerPrefs.SetInt("Achieve2", 1);
        }
        if (_level == 7)
        {
            PlayerPrefs.SetInt("Achieve6", 1);
        }
    }
    public override void ResetPopup()
    {
    }

    public override void SetPopup()
    {
        Debug.Log(_level);
        mainChicken.sprite = openedChickens[_level - 1]; 
        if (_level == 2)
        {
            chickens[0].sprite = openedChickens[_level - 1];
            chickens[1].sprite = closedChickens[_level ];
            chickens[2].sprite = closedChickens[_level +1];
        }
        else if(_level == 7)
        {
            chickens[0].sprite = openedChickens[_level -3];
            chickens[1].sprite = openedChickens[_level -2];
            chickens[2].sprite = openedChickens[_level - 1];
        }
        else
        {
            chickens[0].sprite = openedChickens[_level - 2];
            chickens[1].sprite = openedChickens[_level - 1];
            chickens[2].sprite = closedChickens[_level];
        }
    }
}
