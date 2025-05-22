using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Home : BasicScreen
{
    [SerializeField] private TMP_Text _coins;

    public Button playButton;
    public Button profileButton;
    public Button storiesButton;

    private void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        profileButton.onClick.AddListener(Profile);
        storiesButton.onClick.AddListener(Stories);
    }
    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(PlayGame);
        profileButton.onClick.RemoveListener(Profile);
        storiesButton.onClick.RemoveListener(Stories);
    }

    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
        ConfigScreen();
    }
    private void ConfigScreen()
    {
        _coins.text = PlayerPrefs.GetInt("Coins").ToString();
    }

    private void PlayGame()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game);
    }

    private void Stories()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Stories);
    }

    private void Profile()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Profile);
    }
}
