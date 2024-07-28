using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCharles : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject hitVFX;
    [SerializeField]
    GameObject gauntRing;

    bool isAccioed = false;

    void Start()
    {
        TestFeatures.OnTripleTap += ToggleAnimator;
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
                    Instantiate(gauntRing, gameObject.transform.position, Quaternion.identity);
                    Destroy(gameObject);

                }
                Debug.Log("Hit!");
            } else
            {
                Debug.Log("Missed"!);
            }
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            isAccioed = !isAccioed;
            ToggleAnimator();
        }

        if (isAccioed)
        {
            //if (transform.rotation != Camera.main.transform.rotation)
            //{
            //    transform.rotation = Quaternion.Lerp(transform.rotation, Camera.main.transform.rotation, Time.deltaTime);
            //}
            if (transform.position != Camera.main.transform.position + Camera.main.transform.forward)
            {

                transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position + Camera.main.transform.forward * 0.3f, 0.7f * Time.deltaTime);
            }
        }
    }

    public void ToggleAnimator()
    {
        GetComponentInParent<Animator>().enabled = !GetComponentInParent<Animator>().enabled;
    }

    private void Accioed(string accioed) 
    {
        if (string.Compare(accioed, "boccino", System.StringComparison.OrdinalIgnoreCase) == 0)
        {
            isAccioed = true;
            ToggleAnimator();
        }
    }
    
}
