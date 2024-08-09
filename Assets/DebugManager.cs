using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DebugManager : MonoBehaviour
{
    readonly float TOUCH_DOWN_INTERVAL = 4;
    readonly float MUTE_BUTTON_TRIPLE_TAP_INTERVAL = 2.5f;
    readonly float CONSOLE_ENABLED_LIFETIME = 15;

    [SerializeField]
    TMP_InputField console;
    [SerializeField]
    Button muteButton;
    [SerializeField]
    GameObject[] debugItems;

    int buttonTappedCount;

    float timerStart;

    

    Coroutine ConsoleLifespan;

    private void Start()
    {
        
        buttonTappedCount = 0;
        console.onEndEdit.AddListener((value) =>
        {
            ConsoleLifespan = StartCoroutine(TimedFunction(CONSOLE_ENABLED_LIFETIME, () => { console.enabled = false; muteButton.onClick.RemoveListener(Stercorario); }));
            if (string.Compare(value, "mercadante", StringComparison.OrdinalIgnoreCase) == 0)
            {
                EnableDebug();
            }
        });
        console.enabled = false;

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && console.enabled == false)
        {
            timerStart = Time.time;
        } else
        if (Input.GetMouseButton(0))
        {
            if (Time.time - timerStart > TOUCH_DOWN_INTERVAL && console.enabled == false)
            {
                console.enabled = true;
                MainMenuScript.Bumpie(muteButton.gameObject);
                muteButton.onClick.AddListener(Stercorario);
                Debug.Log("Console enabled");
                ConsoleLifespan = StartCoroutine(TimedFunction(CONSOLE_ENABLED_LIFETIME, () => { console.enabled = false; muteButton.onClick.RemoveListener(Stercorario); }));
            }
        }
    }

    void Stercorario()
    {
        if (!console.enabled)
            return;
        if (buttonTappedCount >= 4)
        {
            console.ActivateInputField();
            console.Select();
            StopCoroutine(ConsoleLifespan);
            return;
        }
        if (Time.time - timerStart > MUTE_BUTTON_TRIPLE_TAP_INTERVAL)
        {
            timerStart = Time.time;
            buttonTappedCount = 1;
        } else
        {
            buttonTappedCount++;
        }
            
    }
    IEnumerator TimedFunction(float timer, Action function)
    {
        float timeLeft = timer;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        function();
    }
   
    public void EnableDebug()
    {
        InterSceneData.debugMode = true;
        foreach (GameObject go in debugItems)
        {
            go.SetActive(true);
        }
    }
   
}
