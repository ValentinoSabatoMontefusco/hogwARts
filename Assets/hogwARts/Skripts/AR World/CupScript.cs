using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupScript : MonoBehaviour
{
    Camera AR_Camera;
    bool beInPlace = false;
    public int rotationSpeed;
    public GameObject fireEffects;
    public GameObject diadem;
    bool rotate;
    Rigidbody rigidbody;
    void Start()
    {
        rotate = true;
        AR_Camera = Camera.main;
        if (gameObject.name.Equals("CorinnaDiadem(Clone)"))
        {
            VoiceScript.sprach += BurnDiadem;
        }
        rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null)
            rigidbody = GetComponentInChildren<Rigidbody>();
    }

    

    void Update()
    {
        if (rigidbody != null && rigidbody.useGravity == true)
            this.enabled = false;
        if (rotate)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        if (!beInPlace)
        {
            if (Vector3.Distance(transform.position, (AR_Camera.transform.position +
                    AR_Camera.transform.forward + (Vector3.up * (transform.position.y >= 0 ? 0 : (-transform.position.y))))) > 0.2f)

                transform.position = Vector3.MoveTowards(transform.position, AR_Camera.transform.position + 
                    AR_Camera.transform.forward + (Vector3.up * (transform.position.y >= 0 ? 0 : (- transform.position.y))), 0.5f * Time.deltaTime);

            else

                beInPlace = true;

        }

        if (Input.GetKey(KeyCode.KeypadEnter))
            BurnDiadem("ardemonio");
        
    }

    void BurnDiadem(string sprach)
    {
        rotate = false;
        transform.LookAt(AR_Camera.transform.position);
        if (sprach != "ardemonio")
            return;
        if (fireEffects != null) 
            fireEffects.SetActive(true);
        GetComponent<Animator>().enabled = true;

    }

    public void DeactivateDiadem()
    {
        diadem.SetActive(false);
    }

    public void DestroyPhone()
    {
        TestFeatures.DestroyApp();
    }

}
