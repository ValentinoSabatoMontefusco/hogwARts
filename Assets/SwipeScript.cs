using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0))
        {

            Plane mediumPlane = new(Camera.main.transform.forward * -1, Camera.main.transform.localPosition + Vector3.forward*2);
            float hitDistance;
            mediumPlane.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitDistance);
            this.transform.position = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(hitDistance);
        }

    }
}
