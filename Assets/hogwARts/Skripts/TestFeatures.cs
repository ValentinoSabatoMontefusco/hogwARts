using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TestFeatures : MonoBehaviour
{

    private int tapCount = 0;
    private float tapDelay = 0.2f;
    private float lastTapTime;
    public GameObject testPrefab;
    private bool debugged = false;
    public TextMeshProUGUI debugText;

    public GameObject debugMenu;

    public GameObject eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("EventSystem") == null)
        {
            Instantiate(eventSystem);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tripleTap() && !debugged)
        {
            debugMenu.SetActive(true);

            debugged = true;
            debugText.color = Color.red;
            Debug.Log("Triple tap felt");
        }
    }

    public bool tripleTap()
    {
        if (Input.GetMouseButtonDown(0)) {

            if (Time.time - lastTapTime < tapDelay)
            {
                tapCount++;
                if (tapCount == 3)
                {
                    tapCount = 0;
                    return true; // Handle triple tap
                     // Reset tap count
                }
            }
            else
            {
                tapCount = 1; // Start counting taps
            }
            lastTapTime = Time.time;
            
        }
        return false;
        
    }

    
}
