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

    public void OnError(string recognizedError)
    {
        Debug.LogError("Speech Recognition Plugin (error): " + recognizedError);
    }

    public void OnResult(string recognizedResult)
    {
        char[] delimiterChars = { '~' };
        Debug.Log(recognizedResult);
        string[] results = recognizedResult.Split(delimiterChars);

        catText.text = "Listening: ";
        foreach (string result in results)
        {
            catText.text += result;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SRP = SpeechRecognizerPlugin.GetPlatformPluginVersion(this.gameObject.name);
        if (SRP != null)
            catText.text += "SRP not null";
    }

    private void booped()
    {
        Color noseColor = noseBoop.GetComponent<Renderer>().material.color;

        if (noseColor == new Color(0, 0, 0, 0.1137255f) || noseColor == Color.black) 
        {
            SRP.SetLanguageForNextRecognition("it-IT");
            SRP.StartListening();
            noseBoop.GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("Allegedly started listening on nose boop");
        }
        else if (noseColor == Color.red)
        {
            SRP.StopListening();
            noseBoop.GetComponent<Renderer>().material.color = Color.black;
            Debug.Log("Allegedly stopped listening on nose boop");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == noseBoop)
                {
                    booped();
                    Debug.Log("Booped");
                }
                    
                
            }
        }
    }
}
