using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoopScript : MonoBehaviour
{
    
    public UnityEvent OnBoop = new();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (booped())
            OnBoop.Invoke();
    }

    private bool booped()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Booped!");
                    return true;
                }
        }

        return false;
    }

}
