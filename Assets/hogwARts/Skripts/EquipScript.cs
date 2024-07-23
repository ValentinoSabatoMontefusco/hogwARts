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
        zPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position.z);
        standardPosition = transform.position;
        standardRotation = transform.rotation;
    }
    public void FollowFinger()
    {
        if (Input.GetMouseButton(0))
        {
            animator.enabled = false;
            float rayDistance;
            zPlane.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayDistance);
            Debug.Log("rayDistance = " + rayDistance);
            Vector3 targetPosition = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(rayDistance);
            Vector3 projection = Vector3.zero;
            if (lastMousePosition != Vector2.zero)
            {
                float diffX = Input.mousePosition.x - lastMousePosition.x;
                float diffY = Input.mousePosition.y - lastMousePosition.y;

                projection = new Vector3(Mathf.Sqrt(Mathf.Abs(diffX)) * Mathf.Sign(diffX), Mathf.Sqrt(Mathf.Abs(diffY)) * Mathf.Sign(diffY), 0);
                targetPosition += projection;
            }

            Debug.Log("Transform Local Position = " + transform.localPosition + ", targetPosition = " + targetPosition);
            Debug.Log("Acceleration = " + Input.accelerationEventCount);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, 0.5f * Time.deltaTime);
           
            
            Vector3 direction = targetPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.right);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation * Quaternion.Euler(90, 0, 0), Time.deltaTime);

            lastMousePosition = Input.mousePosition;
        }
        else if(transform.rotation != standardRotation && transform.position != standardPosition && animator.enabled == false)
        {
            lastMousePosition = Vector2.zero;
            transform.rotation = Quaternion.Lerp(transform.rotation, standardRotation, Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, standardPosition, Time.deltaTime);
        } else
        {
            animator.enabled = true;
        }


    }
}
