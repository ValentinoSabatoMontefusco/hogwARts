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
    public static bool swipeEnabled = false;
    public TextMeshProUGUI debugText;

    public GameObject debugMenu;

    public GameObject eventSystem;

    Vector2 lineStart;
    Vector2 lineEnd;
    public GameObject hitVFX;
    
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
    


}
