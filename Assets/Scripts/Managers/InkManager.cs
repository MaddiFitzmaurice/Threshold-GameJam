using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkManager : MonoBehaviour
{
    private Story _currentScript;

    private void Awake()
    {
        EventManager.EventInitialise(EventType.INK_SPEAKER);
        EventManager.EventInitialise(EventType.INK_TEXTSEND);
        EventManager.EventInitialise(EventType.INK_TEXTEND);
    }

    private void OnEnable()
    {
        EventManager.EventSubscribe(EventType.INK_INTRO, SetScript);
        EventManager.EventSubscribe(EventType.GAMEPLAYUI_NEXTLINE, NextLineHandler);
        EventManager.EventSubscribe(EventType.GAMEPLAYUI_QUESTIONSELECTED, QuestionSelectedHandler);
    }

    private void OnDisable()
    {
        EventManager.EventUnsubscribe(EventType.INK_INTRO, SetScript);
        EventManager.EventUnsubscribe(EventType.GAMEPLAYUI_NEXTLINE, NextLineHandler);
        EventManager.EventUnsubscribe(EventType.GAMEPLAYUI_QUESTIONSELECTED, QuestionSelectedHandler);
    }

    // Set the Ink script to process
    public void SetScript(object data)
    {
        if (data == null)
        {
            Debug.LogError("InkManager SetScript has not received a text file!");
        }

        TextAsset script = (TextAsset)data;

        _currentScript = new Story(script.text);

        StartScript();
    }

    public void StartScript()
    {
        NextLineHandler(null);
    }

    public void NextLineHandler(object data)
    {
        if (_currentScript.canContinue)
        {
            string line = _currentScript.Continue();

            // If no questions are to be displayed
            if (_currentScript.currentChoices.Count == 0)
            {
                string speaker = HandleTag(_currentScript.currentTags);
                EventManager.EventTrigger(EventType.INK_SPEAKER, speaker);
                EventManager.EventTrigger(EventType.INK_TEXTSEND, line);
            }
            // Display questions
            else
            {
                EventManager.EventTrigger(EventType.INK_QUESTIONS, _currentScript.currentChoices);
            }
        }
        else
        {
            EventManager.EventTrigger(EventType.INK_TEXTEND, null);
        }
    }

    public void QuestionSelectedHandler(object data)
    {
        if (data == null)
        {
            Debug.LogError("QuestionSelectedHandler has not received an int!");
        }

        _currentScript.ChooseChoiceIndex((int)data);
        NextLineHandler(null);
    }

    public string HandleTag(List<string> currentTags)
    {
        if (currentTags.Count != 0)
        {
            string speaker = currentTags[0].Trim();
            switch(speaker)
            {
                case "gravedigger":
                    return "Gravedigger";
                case "grim":
                    return "Church Grim";
            }
        }

        return null;
    }
}
