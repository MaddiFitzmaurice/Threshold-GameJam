using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] SoundType _titleMusic; 
    [SerializeField] SoundType _buttonSFX01;
    [SerializeField] SoundType _buttonSFX02;
    [SerializeField] SoundType _buttonSFX03;

    private bool _buttonPressed = false; // Stops multiple clicking of same button

    #region Init

    private void Start()
    {
        _buttonPressed = false;
        EventManager.EventTrigger(EventType.MUSIC, _titleMusic);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion
    // Button to signal exiting the game
    public void QuitButton()
    {
        EventManager.EventTrigger(EventType.QUIT_GAME, null);
    }

    public void LevelSelectButton(int levelNum)
    {
        if (!_buttonPressed)
        {
            EventManager.EventTrigger(EventType.LEVEL_SELECTED, levelNum);
            _buttonPressed = true;
        }
    }

    public void ButtonSFX01()
    {
        EventManager.EventTrigger(EventType.SFX, _buttonSFX01);
    }
    public void ButtonSFX02()
    {
        EventManager.EventTrigger(EventType.SFX, _buttonSFX02);
    }
    public void ButtonSFX03()
    {
        EventManager.EventTrigger(EventType.SFX, _buttonSFX03);
    }
}