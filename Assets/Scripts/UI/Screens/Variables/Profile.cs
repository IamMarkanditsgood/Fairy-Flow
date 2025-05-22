using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile : BasicScreen
{
    [SerializeField] private AvatarManager avatarManager;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _infoButton;
    [SerializeField] private Button _stories;
    [SerializeField] private Button _avatar;


    [SerializeField] private TMP_InputField _name;
    [SerializeField] private TMP_Text _coins;

    [SerializeField] private Image _ahcieve;
    [SerializeField] private Sprite _openedAchieve;

    public Button[] popupsButton;
    public GameObject _popupsView;
    public GameObject[] popups;
    public Button[] okButtons;

    private void Start()
    {
        _homeButton.onClick.AddListener(HomeButton);
        _infoButton.onClick.AddListener(InfoButton);
        _stories.onClick.AddListener(Stories);
        _avatar.onClick.AddListener(avatarManager.PickFromGallery);

        for(int i = 0; i < popupsButton.Length; i++)
        {
            int index = i;
            popupsButton[index].onClick.AddListener(() => OpenPopup(index));
        }
        for (int i = 0; i < okButtons.Length; i++)
        {
            int index = i;
            okButtons[index].onClick.AddListener(() => ClosePopups());
        }
    }
    private void OnDestroy()
    {
        _homeButton.onClick.RemoveListener(HomeButton);
        _infoButton.onClick.RemoveListener(InfoButton);
        _stories.onClick.RemoveListener(Stories);
        _avatar.onClick.RemoveListener(avatarManager.PickFromGallery);

        for (int i = 0; i < popupsButton.Length; i++)
        {
            int index = i;
            popupsButton[index].onClick.RemoveListener(() => OpenPopup(index));
        }
        for (int i = 0; i < okButtons.Length; i++)
        {
            int index = i;
            okButtons[index].onClick.RemoveListener(() => ClosePopups());
        }
    }

    private void OnApplicationQuit()
    {
        if(_name.text != PlayerPrefs.GetString("Name", "User Name"))
        {
            PlayerPrefs.SetString("Name", _name.text);
        }
    }

    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
        avatarManager.SetSavedPicture();
        ConfigScreen();
    }

    private void ConfigScreen()
    {
        _name.text = PlayerPrefs.GetString("Name", "User Name");
        _coins.text = PlayerPrefs.GetInt("Coins").ToString();

        if (PlayerPrefs.HasKey("Achieve"))
        {
            _ahcieve.sprite = _openedAchieve;
        }
    }

    private void HomeButton()
    {
        if (_name.text != PlayerPrefs.GetString("Name", "User Name"))
        {
            PlayerPrefs.SetString("Name", _name.text);
        }
        UIManager.Instance.ShowScreen(ScreenTypes.Home);
    }
    private void InfoButton()
    {
        if (_name.text != PlayerPrefs.GetString("Name", "User Name"))
        {
            PlayerPrefs.SetString("Name", _name.text);
        }
        UIManager.Instance.ShowScreen(ScreenTypes.Info);
    }

    private void Stories()
    {
        if (_name.text != PlayerPrefs.GetString("Name", "User Name"))
        {
            PlayerPrefs.SetString("Name", _name.text);
        }
        UIManager.Instance.ShowScreen(ScreenTypes.Stories);
    }

    private void OpenPopup(int index)
    {
        popups[index].SetActive(true);
        _popupsView.SetActive(true);
    }

    private void ClosePopups()
    {
        foreach(var popup in popups)
        {
            popup.SetActive(false); 
        }
        _popupsView.SetActive(false);
    }
}
