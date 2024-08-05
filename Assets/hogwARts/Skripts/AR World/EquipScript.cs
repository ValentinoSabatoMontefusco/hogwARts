using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using System;

public class EquipScript : MonoBehaviour
{
    [SerializeField]
    TrailRenderer trail;
    [SerializeField]
    GameObject vfxConglomerate;
    [SerializeField]
    Animator animator;
    [SerializeField]
    GameObject textField;

    [SerializeField]
    GameObject hitVFX;

    [SerializeField]
    GameObject floorPlane;

    Vector3 standardPosition;
    Quaternion standardRotation;
    Plane zPlane;
    Vector2 lastMousePosition = Vector2.zero;

    bool isGryffindor;
    HashSet<string> gryffindableHorcrux = new() { { "GauntRing" }, { "SalazarMedallion" }, { "Nagini" } };
    HashSet<string> slytherinableHorcrux = new() { { "TomRiddleDiaryPage" }, { "HuffPuffCup" } };

    [SerializeField]
    GameObject hintGO;

    public bool followingTouch;

    [SerializeField]
    FloorProvider floorProvider;
    bool floorInstantiated;

    public static Action<Collider> horcruxHit;

    public void Start()
    {
        followingTouch = false;
        Debug.Log("Calling name on this equip returns: " + name);
        if (string.Compare(name, "GryffindorSword(Clone)", System.StringComparison.OrdinalIgnoreCase) == 0)
            isGryffindor = true;


        if (floorProvider == null)
            floorProvider = GameObject.Find("AR Session Origin").GetComponent<FloorProvider>();
        floorProvider.Subscribe();
        floorInstantiated = false;
    }
    private void OnDisable()
    {
        floorProvider.Unsubscribe();
    }
    public void Update()
    {
        if (followingTouch)
            FollowFinger();
    }

    public void ToggleText()
    {
        textField.SetActive(!textField.activeSelf);
    }

    public void ToggleVfx()
    {
        vfxConglomerate.SetActive(!vfxConglomerate.activeSelf);
    }

    public void ToggleAnimatorBool(string boolName)
    {
        bool value = !animator.GetBool(boolName);
        animator.SetBool(boolName, value);
    }

    public void ToggleTrail()
    {
        trail.gameObject.SetActive(!trail.gameObject.activeSelf);
    }

    public void ToggleFollowingTouch()
    {
        followingTouch = !followingTouch;

        standardPosition = new Vector3(0.25f, -0.5f, 1.2f);
        standardRotation = Quaternion.Euler(30, 0, 30);
    }
    public void FollowFinger()
    {
        if (Input.GetMouseButton(0))
        {
            animator.enabled = false;
            GetComponent<Collider>().enabled = true;
            float rayDistance;
            zPlane = new Plane(Camera.main.transform.forward * -1, Camera.main.transform.position + Camera.main.transform.forward);
            
            zPlane.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayDistance);
            Debug.Log("rayDistance = " + rayDistance);
            Vector3 targetPosition = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(rayDistance);
            Vector3 projection = Vector3.zero;
            if (lastMousePosition != Vector2.zero)
            {
                float diffX = Input.mousePosition.x - lastMousePosition.x;
                float diffY = Input.mousePosition.y - lastMousePosition.y;

                projection = new Vector3(Mathf.Sqrt(Mathf.Abs(diffX)) * Mathf.Sign(diffX), Mathf.Sqrt(Mathf.Abs(diffY)) * Mathf.Sign(diffY), 0);
                
            }

            Debug.Log("Transform Local Position = " + transform.localPosition + ", targetPosition = " + targetPosition);
            Debug.Log("Acceleration = " + Input.accelerationEventCount);
            transform.position = Vector3.Lerp(transform.position, targetPosition, projection.magnitude * Time.deltaTime);


            Vector3 direction = targetPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.right);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation * Quaternion.Euler(90, 0, 0), Time.deltaTime);

            lastMousePosition = Input.mousePosition;
        }
        else if(transform.localRotation != standardRotation && transform.localPosition != standardPosition && animator.enabled == false)
        {
            GetComponent<Collider>().enabled = false;
            lastMousePosition = Vector2.zero;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, standardRotation, Time.deltaTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, standardPosition, Time.deltaTime);
        } else
        {
            animator.enabled = true;
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Horcrux".ToLower()))
        {
            Destroy(collision.gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Horcrux"))
        {
            if (!isGryffindor)
            {
                if (slytherinableHorcrux.Contains(other.name.Substring(0, other.name.Length - "(Clone)".Length)) || slytherinableHorcrux.Contains(other.name)) 
                {

                    HorcruxHit(other);
                }
                else
                {
                    StartCoroutine(MainMenuScript.Bumpie(other.gameObject));
                }
            } else
            {
                if (gryffindableHorcrux.Contains(other.name.Substring(0, other.name.Length - "(Clone)".Length)) || gryffindableHorcrux.Contains(other.name)) 
                {
                    HorcruxHit(other);
                }
                else
                {
                    StartCoroutine(MainMenuScript.Bumpie(other.gameObject));
                }
            }
           
        }
    }

    private void HorcruxHit(Collider other)
    {
        horcruxHit?.Invoke(other);
        other.tag = "Untagged";
        GameObject.Instantiate(hitVFX, other.transform.position, Quaternion.identity);
        GameObject hint = Instantiate(hintGO, other.transform.position, Quaternion.identity);
        hint.transform.LookAt(Camera.main.transform.position);
        string hintText = "";
        switch (other.name)
        {
            case "SalazarMedallion(Clone)": hintText = "la bianca effigie di colui che ne incarnò realmente il potere /0"; break;
            case "HuffPuffCup(Clone)": hintText = "non senza amici, in prossimità della chiusura 6/"; break;
            case "Nagini(Clone)": hintText = "è l'ultima caccia\" 0"; break;
            case "GauntRing(Clone)": hintText = ""; break;
            default: break;
        }
        hint.GetComponentInChildren<TextMeshPro>().text = hintText;

        //Destroy(other.gameObject);
        Animator otherAnimator = other.GetComponent<Animator>();
        if (otherAnimator == null)
            otherAnimator = other.GetComponentInParent<Animator>();
        if (otherAnimator != null)
            otherAnimator.enabled = false;

        other.transform.SetParent(null);
        Debug.Log("HorcruxHit says: Floor instantiated = " + floorInstantiated + ", floorProvider = " + floorProvider.ToString());
        if (!floorInstantiated && floorProvider != null)
        {
            ARPlane floor = floorProvider.ProvidePlane();
            
            if (floor != null)
            {
                Debug.Log("HorcruxHit says: Floor = " + floor.ToString());
                float floorY = Utilities.DistanceFromPointToPlane(Camera.main.transform.position, floor.normal, floor.center);
                Vector3 floorPosition = Camera.main.transform.position - Vector3.up * floorY;
                GameObject allegedFloor = Instantiate(floorPlane, floorPosition, Quaternion.identity);
                floorInstantiated = true;
                Debug.Log("The alleged floor results to be: " + (allegedFloor == null ? "null" : allegedFloor.ToString() + " with position: " + allegedFloor.transform.position));
            }
            Debug.Log("Floor is null");

        }
            
        other.attachedRigidbody.isKinematic = false;
        other.attachedRigidbody.useGravity = true;
        other.attachedRigidbody.AddForceAtPosition((other.transform.position - this.transform.position).normalized * 2, other.ClosestPointOnBounds(this.transform.position), ForceMode.Impulse);
        StartCoroutine(Minimize(other.transform, 10f));
      

    }

    private IEnumerator Minimize(Transform target, float duration)
    {
        float initialDuration = duration;
        Vector3 initialScale = target.localScale;

        while (duration > 0)
        {
            target.localScale = Vector3.Lerp(Vector3.zero, initialScale, duration / initialDuration);
            duration -= Time.deltaTime;
            yield return null;
        }

        Destroy(target.gameObject);

    }

   
    
         
    


}
