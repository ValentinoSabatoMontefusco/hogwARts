using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Femininomicon : MonoBehaviour
{
    
    public VoiceScript voiceScript;
    [SerializeField]
    Sprite recording;
    [SerializeField]
    Sprite notRecording;
    
    // Start is called before the first frame update
    void Start()
    {
        voiceScript.onRecordingToggle += ToggleIcon;
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("Android non captato, bacchetta listenata");
            GetComponent<Button>().onClick.AddListener(() => { Debug.Log("Bacchetta Clicked"); });
        }
        GetComponent<Button>().onClick.AddListener(voiceScript.ToggleListening);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleIcon(bool isRecording)
    {
        
        if (isRecording)
        {
            GetComponent<Image>().sprite = recording;
            StopCoroutine(InactiveAnimation(transform));
            StartCoroutine(ActiveAnimation(transform));

        }
            
        else
        {
            GetComponent<Image>().sprite = notRecording;
            StopCoroutine(ActiveAnimation(transform));
            StartCoroutine(InactiveAnimation(transform));
        }
    }

    public IEnumerator ActiveAnimation(Transform target)
    {
        Vector3 initialScale = target.localScale;
        while(true)
        {
            while (target.localScale != initialScale * 1.1f)
            {
                target.localScale = Vector3.Lerp(target.localScale, initialScale * 1.11f, 2.0f * Time.deltaTime);
                yield return null;
            }
            while (target.localScale != initialScale * 0.9f)
            {
                target.localScale = Vector3.Lerp(target.localScale, initialScale * 0.89f, 2.0f * Time.deltaTime);
            }
        }
        
    }

    public IEnumerator InactiveAnimation(Transform target)
    {
        while(target.localScale != Vector3.one)
        {
            target.localScale = Vector3.Lerp(target.localScale, Vector3.one, 2.0f * Time.deltaTime);
            yield return null;
        }
    }
}
