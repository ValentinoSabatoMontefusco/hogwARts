//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Android;
//using KKSpeech;
//using TMPro;

//public class CatScript : MonoBehaviour
//{

//    [SerializeField]
//    SpeechRecognizerListener audioListener;
//    [SerializeField]
//    GameObject noseBoop;
//    TextMeshPro catText;
//    // Start is called before the first frame update
//    void Start()
//    {
//        if (audioListener == null)
//            audioListener = GameObject.FindObjectOfType<SpeechRecognizerListener>();
//        if (Application.platform == RuntimePlatform.Android)
//        {
//            Debug.Log("Android check: fatto");
//            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
//            {
//                Permission.RequestUserPermission(Permission.Microphone);
//            }
//        }

//        if (!SpeechRecognizer.ExistsOnDevice())
//        {
//            Debug.Log("Federica Panicucci! Niente speech recognition!");
//            //Destroy(gameObject);
//        }
//        else
//        {
//            Debug.Log("All fair with the speech recognizer");
            
//        }
//        catText = GetComponentInChildren<TextMeshPro>();

//        //audioListener.onAuthorizationStatusFetched.AddListener(OnAuthorizationStatusFetched);
//        //audioListener.onAvailabilityChanged.AddListener(OnAvailabilityChange);
//        audioListener.onErrorDuringRecording.AddListener(OnErrorDuring);
//        audioListener.onErrorOnStartRecording.AddListener(OnErrorFirst);
//        //audioListener.onFinalResults.AddListener(OnFinalResult);
//        audioListener.onPartialResults.AddListener(OnPartialResult);
//        audioListener.onEndOfSpeech.AddListener(OnEndOfSpeech);
//        audioListener.onDesperation.AddListener(OnDesperation);
//        SpeechRecognizer.StartRecording(true);

//        catText.text = "Listening?";
//        if (SpeechRecognizer.IsRecording())
//        {
//            catText.text += " Y\n";
//        } else
//        {
//            catText.text += " N :(\n";
//        }
//        catText.text += "Auth (" + SpeechRecognizer.GetAuthorizationStatus().ToString() + ")\n";
        
//    }

//   private void OnDesperation()
//    {
//        Debug.Log("SpeechRecognizerListener alive and well, yet in full despair");
//    }
//   public void OnPartialResult(string eventFecies)
//    {
//        Debug.Log("Partial result listened");
//        catText.text = eventFecies;
//    }
    
//    public void OnEndOfSpeech()
//    {
//        Debug.Log("End of speech triggered");
//        StopAllCoroutines();
//        StartCoroutine(EndListenage());
//    }

//    private IEnumerator EndListenage()
//    {
//        int count = 0;
        
//        while (count++ < 3)
//        {
//            catText.text += ".";
//            yield return new WaitForSeconds(1);
//        }

//        catText.text += "\n Done!";
//        yield break;
//    }

//    private void booped()
//    {
//        if(SpeechRecognizer.IsRecording())
//        {
//            Debug.Log("Device found recording on boop");
//            catText.text += "\n Boopity stop!";
//            SpeechRecognizer.StopIfRecording();
//        }
//        else
//        {
//            Debug.Log("Device found not recording on boop");
//            catText.text = "Booped!";
//            SpeechRecognizer.StartRecording(true);
//            if (SpeechRecognizer.IsRecording())
//                catText.text += ("Recording...\n");
//            else
//            {
//                catText.text += "Still not recording :(\n";
//            }
//        }
//    }

//    private void OnErrorDuring(string error)
//    {
//        Debug.Log("OnErrorDuring says: " + error);
//    }
//    private void OnErrorFirst(string error)
//    {
//        Debug.Log("OnErrorFirst says: " + error);
//    }



//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            RaycastHit hit;
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            if (Physics.Raycast(ray, out hit)) 
//            {
//                if (hit.collider.gameObject == noseBoop)
//                    booped();
//                Debug.Log("Booped");
//            }
//        }
//    }
//}
