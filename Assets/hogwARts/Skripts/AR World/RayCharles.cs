using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCharles : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject hitVFX;
    [SerializeField]
    GameObject hufflePuffCup;

    bool isAccioed = false;

    void Start()
    {
        TestFeatures.OnTripleTap += DisableAnimator;
        VoiceScript.accioing += Accioed;
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
                    Instantiate(hufflePuffCup, gameObject.transform.position, Quaternion.identity);
                    Destroy(gameObject);

                }
                Debug.Log("Hit!");
            } else
            {
                Debug.Log("Missed"!);
            }
        }

        if (isAccioed)
        {
            if (transform.position != Camera.main.transform.position + Vector3.forward)
            {

                transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position + Camera.main.transform.forward, 0.4f * Time.deltaTime);
            }
        }
    }

    public void DisableAnimator()
    {
        GetComponentInParent<Animator>().enabled = !GetComponentInParent<Animator>().enabled;
    }

    private void Accioed(string accioed) 
    {
        if (string.Compare(accioed, "boccino", System.StringComparison.OrdinalIgnoreCase) == 0)
        {
            isAccioed = true;
        }
    }
    
}
