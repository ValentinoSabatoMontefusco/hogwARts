using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCharles : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject hitVFX;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))                   //Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began
        {
            Debug.Log("Sta succerenno quaccosa?");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {

                    Instantiate(hitVFX, gameObject.transform.position, Quaternion.identity);
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
