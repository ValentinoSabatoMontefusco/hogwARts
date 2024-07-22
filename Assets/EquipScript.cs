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
    }
    public void FollowFinger()
    {
        if (Input.GetMouseButton(0))
        {
            animator.enabled = false;
            transform.localPosition = Vector3.Lerp(transform.localPosition, Input.mousePosition + Vector3.forward * 0.2f, 0.3f * Time.deltaTime);

            Vector3 direction = Input.mousePosition + Vector3.forward*0.2f - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.right);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation * Quaternion.Euler(90, 0, 0), 0.8f * Time.deltaTime);
        }
        else if(transform.rotation != standardRotation && transform.position != standardPosition && animator.enabled == false)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, standardRotation, Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, standardPosition, Time.deltaTime);
        } else
        {
            animator.enabled = true;
        }


    }
}
