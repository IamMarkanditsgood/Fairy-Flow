using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Story : BasicScreen
{
    public Image storyImage;
    public TMP_Text title;
    public TMP_Text description;
    public Button close;

    public StoryData[] _stories;

    [Serializable]
    public class StoryData
    {
        public Sprite image;
        public string title;
        [SerializeField, TextArea]
        public string description;
    }

    private int _currentStoryIndex;

    private void Start()
    {
        close.onClick.AddListener(Stories);
    }

    private void OnDestroy()
    {
        close.onClick.RemoveListener(Stories);
    }

    public void SetIndex(int index)
    {
        _currentStoryIndex = index;
    }

    public override void SetScreen()
    {
        StoryData currentStory = _stories[_currentStoryIndex];
        storyImage.sprite = currentStory.image;
        title.text = currentStory.title;
        description.text = currentStory.description;
    }

    public override void ResetScreen()
    {
    }

    private void Stories()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Stories);
    }
}
