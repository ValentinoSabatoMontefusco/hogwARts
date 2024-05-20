using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using KKSpeech;
using TMPro;

public class CatScript : MonoBehaviour
{

    [SerializeField]
    SpeechRecognizerListener audioListener;
    [SerializeField]
    GameObject noseBoop;
    TextMeshPro catText;
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
        }

        if (!SpeechRecognizer.ExistsOnDevice())
        {
            Debug.Log("Federica Panicucci!");
            //Destroy(gameObject);
        }
        else
        {
            Debug.Log("All fair with the speech recognizer");
        }
        catText = GetComponentInChildren<TextMeshPro>();

        //audioListener.onAuthorizationStatusFetched.AddListener(OnAuthorizationStatusFetched);
        //audioListener.onAvailabilityChanged.AddListener(OnAvailabilityChange);
        //audioListener.onErrorDuringRecording.AddListener(OnError);
        //audioListener.onErrorOnStartRecording.AddListener(OnError);
        //audioListener.onFinalResults.AddListener(OnFinalResult);
        audioListener.onPartialResults.AddListener(OnPartialResult);
        audioListener.onEndOfSpeech.AddListener(OnEndOfSpeech);
        SpeechRecognizer.StartRecording(true);
        catText.text = "Listening?";
        if (SpeechRecognizer.IsRecording())
        {
            catText.text += " Y\n";
        } else
        {
            catText.text += " N :(\n";
        }
        catText.text += "Auth (" + SpeechRecognizer.GetAuthorizationStatus().ToString() + ")\n";
        
    }

   private void OnPartialResult(string eventFecies)
    {
        catText.text = eventFecies;
    }
    
    private void OnEndOfSpeech()
    {
        StopAllCoroutines();
        StartCoroutine(EndListenage());
    }

    private IEnumerator EndListenage()
    {
        int count = 0;
        
        while (count++ < 3)
        {
            catText.text += ".";
            yield return new WaitForSeconds(1);
        }

        catText.text += "\n Done!";
        yield break;
    }

    private void booped()
    {
        if(SpeechRecognizer.IsRecording())
        {
            catText.text += "\n Boopity stop!";
            SpeechRecognizer.StopIfRecording();
        }
        else
        {
            catText.text = "Booped!";
            SpeechRecognizer.StartRecording(true);
            if (SpeechRecognizer.IsRecording())
                catText.text += ("Recording...\n");
            else
            {
                catText.text += "Still not recording :(\n";
            }
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
                    booped();
                Debug.Log("Booped");
            }
        }
    }
}
