using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocationInfoPackage
{
    Vector2 currentLocation;
    GPSCoordinate nearestNeighbor;
    float directionAngle;

    public LocationInfoPackage(Vector2 currentLocation, GPSCoordinate nearestNeighbor, float directionAngle)
    {
        this.currentLocation = currentLocation;
        this.nearestNeighbor = nearestNeighbor;
        this.directionAngle = directionAngle;
    }

    public Vector2 CurrentLocation { get => currentLocation; }
    public GPSCoordinate NearestNeighbor { get => nearestNeighbor; }
    public float DirectionAngle { get => directionAngle; }

    // non optimal solution
    public float Distance { get => Utilities.EuclideanDistanceM(currentLocation, nearestNeighbor.Gps_position); }
   
}
public class LocalizationLogic : MonoBehaviour
{

    LocationService locServ;
    const float MILLS = 0.001f;

    // Data structures for locations 
    QuadTree locations;
    Hashtable lokescions;
    public static readonly float UPDATE_INTERVAL = 1.0f;

    public TMPro.TextMeshProUGUI debugText; 

    [System.Serializable]
    public class LocationCallback : UnityEvent<LocationInfoPackage> { };
    public LocationCallback onLocationCheck = new();

    public void Awake()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            // Request fine location permission
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
        {
            // Request coarse location permission
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }

        locServ = new LocationService();

        locServ.Start();
        lokescions = new Hashtable();
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(CoroutineUmiliante());
        }

    }

    public void Start()
    {
        //if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        //{
            
        //}
        //else
        //{
        //    //Handle permission denial
        //}
        
        Debug.Log("Localization service start() called");
        //GetComponent<MainMenuScript>().menuText.text = "Verifica location service cominziata...";

        if (locServ.status == LocationServiceStatus.Running)
        {

            //GetComponent<MainMenuScript>().menuText.text += " sembra workare :O";
            Debug.Log("Location service running");
            if (locations != null)
                GetComponent<MainMenuScript>().menuText.text = "\n Il quadtree contiene " + locations.Count + " nodi\n L'hashtable " + lokescions.Count;
            else
                GetComponent<MainMenuScript>().menuText.text = "Per qualche ragione il quadtree è null";
            Input.compass.enabled = true;
            StartCoroutine(checkLocation());
            
        }
        else
        {
            //GetComponent<MainMenuScript>().menuText.text += " sembra porcodio dare problemi";
            Debug.Log("Location service not running");
            // Handle service not working
            //onLocationCheck.Invoke(new LocationInfoPackage(new Vector2(40.5f, 15), new GPSCoordinate(40.77427792171623f, 14.789859567782036f, (string)lokescions[new Vector2(40.77427792171623f, 14.789859567782036f)])));
        }
    }

    IEnumerator checkLocation()
    {
        float x;
        float y;
        do
        {
            yield return new WaitForSeconds(UPDATE_INTERVAL);
            x = Input.location.lastData.longitude;
            y = Input.location.lastData.latitude;

            GPSCoordinate currCord = new GPSCoordinate(y, x);

            Vector2 nearestNeighbour = locations.FindNearestNeighbor(new Vector2(currCord.Gps_position.y, currCord.Gps_position.x)); // The Quadtree works with x as latitude and y as longitude
            GPSCoordinate nearestHotspot = new GPSCoordinate(nearestNeighbour.x, nearestNeighbour.y, (string)lokescions[nearestNeighbour]);

            float heading; // = Utilities.CalculateHeading(Input.acceleration, Input.compass.rawVector);
            heading = Input.compass.trueHeading;
            float bearing = Utilities.CalculateBearing(currCord.Gps_position.y, currCord.Gps_position.x, nearestHotspot.Gps_position.y, nearestHotspot.Gps_position.x);
            // 0 is ahead, 180 is behind, right is 90, left is 270
            float relativeDirection = (bearing - heading + 360) % 360;

            debugText.text = "Bearing: " + bearing + "\nHeading: " + heading + "\nRelative Direction: " + relativeDirection + "\nWe fucked?";

            onLocationCheck.Invoke(new LocationInfoPackage(currCord.Gps_position, nearestHotspot, relativeDirection));

            //menuText.text = "Your current coordinates are: Lat: ~ " + y + ", Long: ~ " + x;
            //float hotspotDistance = Utilities.EuclideanDistanceM(new Vector2(y, x), nearestNeighbour);

            //if (hotspotDistance < 25)
            //{
            //    menuText.text += "\nThey match ";
            //}
            //else if (hotspotDistance < 100)
            //{
            //    menuText.text += "\nThey're pristy close to ";
            //}
            //else
            //{

            //    menuText.text += "\nThey do be pretty far from ";
            //}

            //menuText.text += nearestHotspot.Name + " (" + Utilities.EarthDistanceFormattedString(hotspotDistance) + ")";
            //menuText.text += "\n NearHot: Lat: " + nearestHotspot.Gps_position.y.ToString("F3") + ", Lng: " + nearestHotspot.Gps_position.x.ToString("F3");
            //menuText.color = Color.Lerp(Color.green, new Color(1, 1, 0.4f, 0.5f), hotspotDistance < 500 ? hotspotDistance / 500 : 1);


        } while (locServ.status == LocationServiceStatus.Running);

        yield break;

    }


    private class JsonHotspots
    {
        public GPSCoordinate[] hotspots;
    }
    IEnumerator CoroutineUmiliante()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "hotspots.JSON");
        UnityWebRequest www = UnityWebRequest.Get(path);

        yield return www.SendWebRequest();

        Debug.Log("WebRequest tornata");

        if (www.result == UnityWebRequest.Result.Success)
        {
            JsonHotspots jsonHotspots = JsonUtility.FromJson<JsonHotspots>(www.downloadHandler.text);
            GPSCoordinate[] hotspots = jsonHotspots.hotspots;
            lokescions = new Hashtable();

            foreach (GPSCoordinate hotspot in hotspots)
            {
                lokescions.Add(new Vector2(hotspot.Gps_position.x, hotspot.Gps_position.y), hotspot.Name);
            }

            locations = new QuadTree(new Rect(40.5f, 14.5f, 1f, 1f), 2);

            foreach (Vector2 point in lokescions.Keys)
            {
                locations.Insert(point);
            }
            Debug.Log("Servizio verosimilmente svolto con successo, benché in maniera umiliante");
        }
        else
        {
            Debug.Log("Mannaggia cristo chissà mo che cazzo è successo con 'sta web request del cazzo");
        }

        yield break;

    }

}
