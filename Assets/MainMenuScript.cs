using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Text menuText;
    LocationService locServ;
    const float MILLS = 0.001f;

    GPSCoordinate myRoom = new GPSCoordinate(40.91540078137776f, 14.82838650405708f, "stanza di Valentino");
    public void Awake()
    {
        locServ = new LocationService();
        locServ.Start();
        menuText.text = "Location Service tried to activate and did so..";

    }

    public void Start()
    {
        if (locServ.status == LocationServiceStatus.Running)
        {
            StartCoroutine(checkLocation());
            menuText.text += ". succesfully.";
        } else
            menuText.text += ". failurely :c";


    }

    IEnumerator checkLocation()
    {
        float x;
        float y;
        do
        {
            yield return new WaitForSeconds(3.0f);
            x = Input.location.lastData.latitude;
            y = Input.location.lastData.longitude;

            GPSCoordinate currCord = new GPSCoordinate(x, y);

            menuText.text = "Your current coordinates are: X ~ " + x + ", Y ~ " + y; 

            if (myRoom.distanceTo(currCord) / MILLS < 0.1f)
            {
                menuText.text += "\nThey match ";
            } else if (myRoom.distanceTo(currCord) / MILLS < 1)
            {
                menuText.text += "\nThey're pristy close to ";
            } else
            {

                menuText.text += "\nThey do be pretty far from ";
            }

            menuText.text += myRoom.Name;


        } while (locServ.status == LocationServiceStatus.Running);

        yield break;

    }

    private bool inRange(float value, float target, float delta)
    {
        if (Mathf.Abs(value - target) < delta)
            return true;
        else
            return false;

    }
    // Let's keep it basic
    public void LoadScene (string sceneName)
    {

        SceneManager.LoadScene (sceneName);
    }
}
