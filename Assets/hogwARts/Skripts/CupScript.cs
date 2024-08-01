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
    void Start()
    {
        AR_Camera = Camera.main;
        if (gameObject.name.Equals("CorinnaDiadem(Clone)"))
        {
            VoiceScript.sprach += BurnDiadem;
        }
    }

    

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        if (!beInPlace)
        {
            if (Vector3.Distance(transform.position, (AR_Camera.transform.position + AR_Camera.transform.forward)) > 0.2f || transform.position.y < 0)

                transform.position = Vector3.MoveTowards(transform.position, AR_Camera.transform.position + 
                    AR_Camera.transform.forward + (Vector3.up * (transform.position.y >= 0 ? 0 : (- transform.position.y))), 0.5f * Time.deltaTime);

            else

                beInPlace = true;

        }
        
    }

    void BurnDiadem(string sprach)
    {
        if (sprach != "ardemonio")
            return;

        fireEffects.SetActive(true);

    }

    public void DeactivateDiadem()
    {
        diadem.SetActive(false);
    }


}
