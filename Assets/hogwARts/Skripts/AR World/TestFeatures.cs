using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestFeatures : MonoBehaviour
{

    private int tapCount = 0;
    private float tapDelay = 0.2f;
    private float lastTapTime;
    public GameObject testPrefab;
    private bool debugged;
    public static bool swipeEnabled;
    public TextMeshProUGUI[] debugText;
    

    public GameObject debugMenu;

    public GameObject eventSystem;

    Vector2 lineStart;
    Vector2 lineEnd;
    public GameObject hitVFX;

    [SerializeField]
    GameObject swordPrefab;
    [SerializeField]
    GameObject toothPrefab;
    static bool equipped;
    public static Action OnTripleTap;

    [SerializeField]
    Image localDestruction;
    static Image destruction;

    [SerializeField]
    List<GameObject> debugButtons = new();

    Dictionary<ARDebugOptions, GameObject> debugOptions;


    private void Awake()
    {
        debugOptions = new() { { ARDebugOptions.Diario, debugButtons[0] }, { ARDebugOptions.Boccino, debugButtons[1] }, 
            { ARDebugOptions.Medaglione, debugButtons[2] }, { ARDebugOptions.Coppa, debugButtons[3] }, { ARDebugOptions.Diadema, debugButtons[4] }, 
            { ARDebugOptions.Nagini, debugButtons[5] } };

        if (InterSceneData.currentOption != ARDebugOptions.Nendi)
        {
            debugMenu.SetActive(true);
            for (int i = 0; i < 6; i++)
            {
                GameObject child = debugMenu.transform.GetChild(i).gameObject;
                if (child.Equals(debugOptions[InterSceneData.currentOption]))
                    continue;
                child.SetActive(false);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        debugged = !InterSceneData.debugMode;
        foreach (TextMeshProUGUI dbTxt in debugText)
        {
            dbTxt.gameObject.SetActive(InterSceneData.debugMode);
        }
        if (localDestruction != null) 
            destruction = localDestruction;
        swipeEnabled = false;
        equipped = false;
        VoiceScript.accioing += SpawnEquip;
        if (GameObject.Find("EventSystem") == null)
        {
            Instantiate(eventSystem);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (tripleTap())
        {
            OnTripleTap?.Invoke();
            if (!debugged)
            {
                debugMenu.SetActive(true);

                debugged = true;
                debugText[0].color = Color.red;
            }
            
            Debug.Log("Triple tap felt");
        }

        if (swipeEnabled) 
            SwipeBehaviour();
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

    public void SwipeBehaviour()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            lineStart = Input.mousePosition;
        }

        //if (Input.GetMouseButton(0))
        //{
        //    lineEnd = Input.mousePosition;
        //}

        if(Input.GetMouseButtonUp(0))
        {
            lineEnd = Input.mousePosition;
            CheckSwipe(lineStart, lineEnd);
        }


    }

    private void CheckSwipe(Vector2 lineStart, Vector2 lineEnd)
    {
        Debug.Log("Checking swipe distance");
        Debug.Log("Swipe Distance = " + Vector2.Distance(lineStart, lineEnd));
        float swipeDistance = Vector2.Distance(lineStart, lineEnd);
        if ( swipeDistance <= 50 || lineStart == Vector2.zero)
        {

            return;
        }

        Vector3 startWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(lineStart.x, lineStart.y, 1));
        Vector3 endWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(lineEnd.x, lineEnd.y, 1));

        Collider objectHit = RecursiveSwipeCheck(lineStart, lineEnd, "Horcrux");
        if (objectHit == null)
            return;

        Debug.Log("Hit!!!!!");
        GameObject.Instantiate(hitVFX, objectHit.transform.position, Quaternion.identity);
        GameObject.Destroy(objectHit.gameObject);

    }

    private Collider RecursiveSwipeCheck(Vector2 startPos, Vector2 endPos, string tag)
    {
        Debug.Log("Recursive Swipe Check called with startPos = " + startPos + " and endPos = " + endPos);
        float distance = Vector2.Distance(startPos, endPos);
        if (distance < 120)
            return null;
        Collider returnCollider = null;

        Ray ray = Camera.main.ScreenPointToRay(Vector2.Lerp(startPos, endPos, 0.5f));
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, 15) || !hit.collider.CompareTag(tag))
        {
            returnCollider = RecursiveSwipeCheck(startPos, Vector2.Lerp(startPos, endPos, 0.5f), tag);
            if (returnCollider != null)
                return returnCollider;

            returnCollider = RecursiveSwipeCheck(Vector2.Lerp(startPos, endPos, 0.5f), endPos, tag);
            if (returnCollider != null)
                return returnCollider;
        }

        return hit.collider;
    }

    public void SpawnEquip(string equip)
    {
        if (equipped)
            return;

        if (string.Compare(equip, "spada", StringComparison.OrdinalIgnoreCase) == 0)
        {
            Instantiate(swordPrefab, GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>());
            equipped = true;
        }
        if (string.Compare(equip, "dente", StringComparison.OrdinalIgnoreCase) == 0)
        {
            Instantiate(toothPrefab, GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>());
            equipped = true;
        }
        

    }

    public static void DestroyApp()
    {
        if (destruction != null)
        {
            destruction.gameObject.SetActive(true);
            //destruction.enabled = true;

        }
    }



}
