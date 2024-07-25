using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Vector3 standardPosition;
    Quaternion standardRotation;
    Plane zPlane;
    Vector2 lastMousePosition = Vector2.zero;

    public bool followingTouch;

    public void Start()
    {
        followingTouch = false;
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
            zPlane = new Plane(Camera.main.transform.forward * -1, Camera.main.transform.position + Camera.main.transform.forward * 1.5f);
            
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
            GameObject.Instantiate(hitVFX, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }
}
