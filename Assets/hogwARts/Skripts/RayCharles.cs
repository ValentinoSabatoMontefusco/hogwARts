using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCharles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("Sta succerenno quaccosa?");
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Destroy(gameObject);
                    
                }
                Debug.Log("Hit!");
            } else
            {
                Debug.Log("Missed"!);
            }
        }
    }
}
