using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : BasicScreen
{
    public Button p;
    public Button H;

    public TMP_Text coins;
    public TMP_Text reward;
    public TMP_Text timeToHome;
    private TextManager textManager = new TextManager();
    public Button[] read;
    public Button[] buy;

    void Start()
    {
        textManager.SetText(PlayerPrefs.GetInt("Coins"), coins, true);
        textManager.SetText(PlayerPrefs.GetInt("Reward"), reward, true);
        textManager.SetText("10", timeToHome);
        p.onClick.AddListener(Profile);
        H.onClick.AddListener(Home);

        GameEvents.OnNewCoins += UpdateCoins;
        GameEvents.OnNewReward += UpdateReward;
        GameEvents.OnNewTime += UdpateTimer;

        for(int i = 0; i < read.Length; i++)
        {
            int index = i;
            read[index].onClick.AddListener(() => Read(index));
        }
        for (int i = 0; i < buy.Length; i++)
        {
            int index = i;
            buy[index].onClick.AddListener(() => Buy(index));
        }
    }

    // Update is called once per frame
    void OnDestroy()
    {
        p.onClick.RemoveListener(Profile);
        H.onClick.RemoveListener(Home);
        GameEvents.OnNewCoins -= UpdateCoins;
        GameEvents.OnNewReward -= UpdateReward;
        GameEvents.OnNewTime -= UdpateTimer;
        for (int i = 0; i < read.Length; i++)
        {
            int index = i;
            read[index].onClick.RemoveListener(() => Read(index));
        }
        for (int i = 0; i < buy.Length; i++)
        {
            int index = i;
            buy[index].onClick.RemoveListener(() => Buy(index));
        }
    }

    public override void SetScreen()
    {


        SetButtons();
    }

    private void SetButtons()
    {
        foreach(Button button in buy)
        {
            button.interactable = true;
            button.gameObject.SetActive(true);
        }

        string key = "Bird" + 0;
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, 1);
        }
        int lastSaved = 0;

        for (int i = 0; i < buy.Length; i++)
        {
            key = "Bird" + i;
            if(PlayerPrefs.GetInt(key) == 1)
            {
                lastSaved = i;
                buy[i].gameObject.SetActive(false);
            }
        }
        for(int j = lastSaved+2; j < buy.Length; j++)
        {
            buy[j].interactable = false;
        }
    }

    private void Buy(int index)
    {
        if (PlayerPrefs.GetInt("Coins") >= 500)
        {
            int coins = PlayerPrefs.GetInt("Coins");
            coins -= 500;
            PlayerPrefs.SetInt("Coins", coins);
            string key = "Bird" + index;
            PlayerPrefs.SetInt(key, 1);
            SetScreen();

            if (PlayerPrefs.GetInt("Bought") < 5)
            {
                int a = PlayerPrefs.GetInt("Bought") + 1;
                PlayerPrefs.SetInt("Bought", a);
            }
            else if (PlayerPrefs.GetInt("Bought") >= 5)
            {
                PlayerPrefs.SetInt("Achieve5", 1);
            }
        }

    }
    private void Read(int index)
    {

    }

    public void UpdateCoins(int Coins)
    {
        textManager.SetText(Coins, coins, true);
    }
    public void UpdateReward(int newReward)
    {
        textManager.SetText(newReward, reward, true);
    }
    public void UdpateTimer(int time)
    {
        textManager.SetText(time, timeToHome);
    }

    public override void ResetScreen()
    {
    }

    private void Profile()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Profile);
    }

    private void Home()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Game);
    }
}
