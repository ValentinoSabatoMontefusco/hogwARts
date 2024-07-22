using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SpeechRecognizerPlugin;

public class NuCatScript : MonoBehaviour, ISpeechRecognizerPlugin
{

    private SpeechRecognizerPlugin SRP = null;
    [SerializeField]
    TextMeshPro catText;
    [SerializeField]
    GameObject noseBoop;
    [SerializeField]
    TestFeatures testScript;

    public void OnError(string recognizedError)
    {
        Debug.LogError("Speech Recognition Plugin (error): " + recognizedError);
    }
    private void resultCheck(string result)
    {
        
    }
    public void OnResult(string recognizedResult)
    {
        char[] delimiterChars = { '~' };
        Debug.Log(recognizedResult);
        string[] results = recognizedResult.Split(delimiterChars);

        catText.text = "Listening: ";
        //foreach (string result in results)
        //{
        //    catText.text += result;
        //    if (result.ToLower().Contains("gianluca") || result.ToLower().Contains("umberto"))
        //        goto SettePuntoCinque;
        //}
        catText.text += results[0];

        if (results[0].ToLower().Contains("gianluca") || results[0].ToLower().Contains("umberto"))
                goto SettePuntoCinque;

        if (InterSceneData.currentNearestLocation.Name != "PAM nderro casa")
        {
            foreach (string s in results)
            {
                if (s.ToLower().Contains("accio spada"))
                {
                    testScript.SpawnEquip("Sword");
                    break;
                }
                    
            }
        }
        


        SRP.StopListening();
        return;
    SettePuntoCinque:
        catText.color = Color.yellow;
        SRP.StopListening();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        SRP = SpeechRecognizerPlugin.GetPlatformPluginVersion(this.gameObject.name);
        if (SRP != null)
            catText.text += " SRP not null";
        GetComponentInChildren<BoopScript>().OnBoop.AddListener(booped);
        SRP.OnListenStart.AddListener(OnListenStart);
        SRP.OnListenFinish.AddListener(OnListenFinish);
        if (testScript == null)
            testScript = GameObject.Find("AR Session Origin").GetComponent<TestFeatures>();
    }


    private void OnListenStart()
    {
        noseBoop.GetComponent<Renderer>().material.color = Color.red;
        //StartCoroutine(finishBozza());
    }
    private void OnListenFinish()
    {
        noseBoop.GetComponent<Renderer>().material.color = Color.black;
    }

    IEnumerator finishBozza()
    {
        do
        {
            yield return new WaitForSeconds(0.75f);
        } while (Microphone.IsRecording(null));
        OnListenFinish();
        yield break;
    }
    private void booped()
    {

        if (SRP.isListening()) {
            Debug.Log("SRP was listening. Stopping it");
            SRP.StopListening();
        } else
        {
            Debug.Log("SRP weren't lissenin'. Startin' it");
            SRP.StartListening();
        }
        //Color noseColor = noseBoop.GetComponent<Renderer>().material.color;

        //if(SRP.isListening())

        //if (noseColor == new Color(0, 0, 0, 0.1137255f) || noseColor == Color.black) 
        //{
        //    SRP.SetLanguageForNextRecognition("it-IT");
        //    SRP.StartListening();
        //    noseBoop.GetComponent<Renderer>().material.color = Color.red;
        //    Debug.Log("Allegedly started listening on nose boop");
        //}
        //else if (noseColor == Color.red)
        //{
        //    SRP.StopListening();
        //    noseBoop.GetComponent<Renderer>().material.color = Color.black;
        //    Debug.Log("Allegedly stopped listening on nose boop");
        //}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
