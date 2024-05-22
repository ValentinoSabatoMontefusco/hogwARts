using UnityEngine;

public class SpeechRecognizerPlugin_Android : SpeechRecognizerPlugin
{
    public SpeechRecognizerPlugin_Android(string gameObjectName) : base(gameObjectName) { }

    private string javaClassPackageName = "com.example.eric.unityspeechrecognizerplugin.SpeechRecognizerFragment";
    private AndroidJavaClass javaClass = null;
    private AndroidJavaObject instance = null;
    private bool listenage = false;

    protected override void SetUp()
    {
        Debug.Log("SetUpAndroid " + gameObjectName);
        javaClass = new AndroidJavaClass(javaClassPackageName);
        javaClass.CallStatic("SetUp", gameObjectName);
        instance = javaClass.GetStatic<AndroidJavaObject>("instance");
    }

    public override void StartListening()
    {
        instance.Call("StartListening", this.isContinuousListening, this.language, this.maxResults);
        listenage = true;
        TriggerOnListenStart();
    }

    public override void StartListening(bool isContinuous = false, string newLanguage = "it", int newMaxResults = 10)
    {
        instance.Call("StartListening", isContinuous, language, maxResults);
        listenage = true;
        TriggerOnListenStart();
    }

    public override void StopListening()
    {
        instance.Call("StopListening");
        listenage = false;
        TriggerOnListenFinish();
    }

    public override bool isListening()
    {
        return listenage;
    }

    public override void SetContinuousListening(bool isContinuous)
    {
        this.isContinuousListening = isContinuous;
        instance.Call("SetContinuousListening", isContinuous);
    }

    public override void SetLanguageForNextRecognition(string newLanguage)
    {
        this.language = newLanguage;
        instance.Call("SetLanguage", newLanguage);
    }

    public override void SetMaxResultsForNextRecognition(int newMaxResults)
    {
        this.maxResults = newMaxResults;
        instance.Call("SetMaxResults", newMaxResults);
    }    
}