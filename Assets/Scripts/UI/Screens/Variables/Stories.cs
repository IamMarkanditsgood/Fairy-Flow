using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stories : BasicScreen
{
    public Story story;
    public Button[] unlockButtons;
    public Button[] readButtons;
    public Button close;
    public Button home;
    public Button profile;

    public void Start()
    {
        close.onClick.AddListener(Home);
        home.onClick.AddListener(Home);
        profile.onClick.AddListener(Profile);


        for (int i = 0; i < unlockButtons.Length; i++)
        {
            int index = i;
            unlockButtons[index].onClick.AddListener(() => UnlockStory(index));
        }
        for (int i = 0; i < readButtons.Length; i++)
        {
            int index = i;
            readButtons[index].onClick.AddListener(() => ReadStory(index));
        }
    }

    public void OnDestroy()
    {
        close.onClick.RemoveListener(Home);
        home.onClick.RemoveListener(Home);
        profile.onClick.RemoveListener(Profile);

        for (int i = 0; i < unlockButtons.Length; i++)
        {
            int index = i;
            unlockButtons[index].onClick.RemoveListener(() => UnlockStory(index));
        }
        for (int i = 0; i < readButtons.Length; i++)
        {
            int index = i;
            readButtons[index].onClick.RemoveListener(() => ReadStory(index));
        }
    }

    public override void Show()
    {
        base.Show();
        SetScreen();
    }

    public override void SetScreen()
    {
        for (int i = 0; i < unlockButtons.Length; i++)
        {
            string key = "Story" + i;
            if (PlayerPrefs.GetInt(key) == 1)
            {
                unlockButtons[i].gameObject.SetActive(false);
            }
        }
    }
    public override void ResetScreen()
    {
    }
    private void UnlockStory(int index)
    {
        int score = PlayerPrefs.GetInt("Coins");
        if (score >= 2000)
        {
            score -= 2000;
            PlayerPrefs.SetInt("Coins", score);

            string key = "Story" + index;
            PlayerPrefs.SetInt(key, 1);

            SetScreen();
        }
    }
    private void ReadStory(int index)
    {
        story.SetIndex(index);
        story.Show();
    }

    private void Home ()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Home);
    }
    private void Profile()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Profile);
    }
}
