using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeMovement : MonoBehaviour
{

    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        gameObject.transform.position += startPosition;
        
    }
}
