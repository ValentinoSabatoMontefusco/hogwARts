using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupScript : MonoBehaviour
{
    Camera AR_Camera;
    bool beInPlace = false;
    public int rotationSpeed;
    void Start()
    {
        AR_Camera = Camera.main;
    }

    
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        if (!beInPlace)
        {
            if (Vector3.Distance(transform.position, (AR_Camera.transform.position + AR_Camera.transform.forward)) > 0.2f)

                transform.position = Vector3.MoveTowards(transform.position, AR_Camera.transform.position + AR_Camera.transform.forward, 0.5f * Time.deltaTime);

            else
                
                    beInPlace = true;
                
        }
        
            
            
        
    }
}
