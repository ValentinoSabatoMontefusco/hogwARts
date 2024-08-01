using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SpeechRecognizerPlugin;

public class VoiceScript : MonoBehaviour, ISpeechRecognizerPlugin
{

    private SpeechRecognizerPlugin SRP = null;
    [SerializeField]
    TextMeshProUGUI debugText;
    

    public static Action<string> accioing;
    public static Action<string> sprach;



    public void OnError(string recognizedError)
    {
        debugText.text = "Errore rilevato: " + recognizedError;
    }

    public void OnResult(string recognizedResult)
    {
        char[] delimiterChars = { '~' };
        Debug.Log(recognizedResult);
        string[] results = recognizedResult.Split(delimiterChars);

        debugText.text = results[0];

        bool matchFound = false;

        foreach(string s in results)
        {
            if (s.ToLower().Contains("echidna"))
            {
                sprach?.Invoke("echidna");
            }

            if (s.ToLower().Contains("accio boccino"))
            {
                accioing?.Invoke("boccino");
                matchFound = true;
            }
            if (matchFound)
                break;

            if (s.ToLower().Contains("accio spada") || Utilities.LevenshteinDistance(s.ToLower(), "accio ensis avis aureus") <= 3)
            {
                accioing?.Invoke("spada");
                //GetComponent<TestFeatures>().SpawnEquip("Spada");
                matchFound = true;
            }

            if (matchFound)
                break;

            if (s.ToLower().Contains("accio dente") || Utilities.LevenshteinDistance(s.ToLower(), "accio morsus venenum draconis") <= 4)
            {
                accioing?.Invoke("dente");
                matchFound = true;
            }

            if (matchFound)
                break;

            if (Utilities.LevenshteinDistance(s.ToLower(), "veniat ardemonium metus mundi") <= 6)
            {
                sprach?.Invoke("ardemonio");
                matchFound = true;
                
            }

            if (matchFound)
                break;

            if (Utilities.LevenshteinDistance(s.ToLower(), "questa è l'ultima caccia") <= 2)
            {
                sprach?.Invoke("ultimacaccia");
                break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        
        SRP = SpeechRecognizerPlugin.GetPlatformPluginVersion(this.gameObject.name);
        SRP.SetContinuousListening(true);
        SRP.StartListening();
        //debugText.text = "Tentativo di avvio fatto";
        //debugText.text = "Ascolto in corso? " + (SRP.isListening() ? "Yes!" : "Nay... :c");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {
        //if(Application.platform == RuntimePlatform.Android)
        //    SRP.StopListening();
        
    }
}
