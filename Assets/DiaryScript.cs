using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiaryScript : MonoBehaviour
{
    [SerializeField]
    List<GameObject> diaryPages;

    [SerializeField]
    GameObject hintGO;

    [SerializeField]
    GameObject greenScreen;

    [SerializeField]
    AudioSource poisonousHit_sfx;

    GameObject myGreenScreen;


    
    private void Start()
    {
        foreach(GameObject d in diaryPages)
        {
            d.AddComponent<PageBehaviour>();
            d.GetComponent<PageBehaviour>().onDestroy += PageDestroyed;
        }
        myGreenScreen = Instantiate(greenScreen, GameObject.Find("Canvas").transform);
    }
    
   
    private class PageBehaviour : MonoBehaviour
    {
        public Action onDestroy;

        private void OnDestroy()
        {
            onDestroy?.Invoke();
        }
    }

    void PageDestroyed()
    {
        diaryPages.RemoveAt(0);
        if (poisonousHit_sfx != null)
        {
            poisonousHit_sfx.Play();
        }
        StartCoroutine(FlashScreen(5 - diaryPages.Count));
        if (diaryPages.Count <= 0)
        {
            GameObject hint = GameObject.Instantiate(hintGO, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
            hint.transform.LookAt(Camera.main.transform.position);
            hint.GetComponentInChildren<TextMeshPro>().text = "Dell'ultimo, che cela un dono dei tre 0";
        }


    }

    IEnumerator FlashScreen(int value)
    {
        Color newColor = myGreenScreen.GetComponent<Image>().color;
        float totalDuration = value * 0.75f;
        float durationLeft = totalDuration;

        while (durationLeft > totalDuration / 2)
        {
            newColor.a = Mathf.Lerp(value * 0.2f, 0, (durationLeft - totalDuration / 2) / (totalDuration / 2));
            myGreenScreen.GetComponent<Image>().color = newColor;
            durationLeft -= Time.deltaTime;
            yield return null;
        }
        while (durationLeft > 0)
        {
            newColor.a = Mathf.Lerp(0, value * 0.2f, (durationLeft) / (totalDuration / 2));
            myGreenScreen.GetComponent<Image>().color = newColor;
            durationLeft -= Time.deltaTime;
            yield return null;
        }

    }
}
