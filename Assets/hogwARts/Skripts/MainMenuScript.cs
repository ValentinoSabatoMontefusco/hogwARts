using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Text menuText;
    LocationService locServ;
    const float MILLS = 0.001f;
    QuadTree locations;

    GPSCoordinate myRoom = new GPSCoordinate(40.91540078137776f, 14.82838650405708f, "stanza di Valentino");



    Hashtable lokescions;
    public void Awake()
    {
        locServ = new LocationService();
        locServ.Start();
        menuText.text = "Location Service tried to activate and did so";

        lokescions = new Hashtable();
        menuText.text += ".";
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(CoroutineUmiliante());
        } else
        {
            GPSCoordinate[] hotspots = InitializeHotspots();

            menuText.text += hotspots.Length;
            //Debug.Log(hotspots[0]);
            foreach (GPSCoordinate hotspot in hotspots)
            {
                lokescions.Add(new Vector2(hotspot.Gps_position.x, hotspot.Gps_position.y), hotspot.Name);
            }

            menuText.text += ".";

            locations = new QuadTree(new Rect(40.5f, 14.5f, 1f, 1f), 2);

            foreach (Vector2 point in lokescions.Keys)
            {
                locations.Insert(point);
            }
            menuText.text += ".";
        }
        



        

        //lokescions.Add(new Vector2(40.91540078137776f, 14.82838650405708f), "stanza di Valentino");
        //lokescions.Add(new Vector2(40.914094468604254f, 14.80575556180331f), "parco Manganelly");
        //lokescions.Add(new Vector2(40.89756231358262f, 14.828281311422465f), "Quarto beach arena");
        //lokescions.Add(new Vector2(40.91723858701945f, 14.830413909754018f), "PAM nderro casa");
        //lokescions.Add(new Vector2(40.92197959327504f, 14.83398339992653f), "villa comunale");

        

        //Vector2 nearest = locations.FindNearestNeighbor(new Vector2(40.915000f, 14.82840000f));
        //menuText.text = "Debug: nearest lat: " + nearest.x + ", long: " + nearest.y;

        
    }

    public void Start()

    {
        menuText.text += ".";
        Permission.RequestUserPermission(Permission.FineLocation);

        menuText.text += ".";

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
            x = Input.location.lastData.longitude;
            y = Input.location.lastData.latitude;
            
            GPSCoordinate currCord = new GPSCoordinate(y, x);

            Vector2 nearestNeighbour = locations.FindNearestNeighbor(new Vector2(y, x)); // The Quadtree works with x as latitude and y as longitude
            GPSCoordinate nearestHotspot = new GPSCoordinate(nearestNeighbour.x, nearestNeighbour.y, (string)lokescions[nearestNeighbour]);

            menuText.text = "Your current coordinates are: Lat: ~ " + y + ", Long: ~ " + x;
            float hotspotDistance = Utilities.EuclideanDistanceM(new Vector2(y, x), nearestNeighbour);

            if (hotspotDistance < 25)
            {
                menuText.text += "\nThey match ";
            } else if (hotspotDistance < 100)
            {
                menuText.text += "\nThey're pristy close to ";
            } else
            {

                menuText.text += "\nThey do be pretty far from ";
            }

            menuText.text += nearestHotspot.Name + " (" + Utilities.EarthDistanceFormattedString(hotspotDistance) + ")";
            menuText.text += "\n NearHot: Lat: " + nearestHotspot.Gps_position.y.ToString("F3") + ", Lng: " + nearestHotspot.Gps_position.x.ToString("F3");
            menuText.color = Color.Lerp(Color.green, new Color(1, 1, 0.4f, 0.5f), hotspotDistance < 500 ? hotspotDistance / 500 : 1);
            

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
        SceneManager.LoadSceneAsync(sceneName);
        
    }

    public GPSCoordinate getNN(GPSCoordinate gPSCoordinate)

    {
        float minDistance = float.MaxValue;
        GPSCoordinate nearestNeigh = null;
        foreach (GPSCoordinate gpsCord in lokescions)
        {
            float currentDistance = gpsCord.distanceTo(gPSCoordinate);

            if (minDistance > currentDistance)
            {
                minDistance = currentDistance;
                nearestNeigh = gpsCord;
            }
        }

        return nearestNeigh;
    }

    private GPSCoordinate[] InitializeHotspots()
    {
        Debug.Log("Initialize Hotspots invocato");
        string path = Path.Combine(Application.streamingAssetsPath, "hotspots.JSON");
        Debug.Log("path = " + path);
        bool dirTest = Directory.Exists(Application.streamingAssetsPath);
        Debug.Log("La directory 'StreamingAssets' pare " + (dirTest == false ? "non " : "") + "esista");

        
        if (File.Exists(path))
        {
            Debug.Log("File trovato");
            JsonHotspots jsonHotspots = JsonUtility.FromJson<JsonHotspots>(File.ReadAllText(path));
            Debug.Log(jsonHotspots.hotspots[0].ToString());
            return jsonHotspots.hotspots;

        }

        Debug.Log("No hotspots file found");
        return null;
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

        if (www.result == UnityWebRequest.Result.Success)
        {
            JsonHotspots jsonHotspots = JsonUtility.FromJson<JsonHotspots>(www.downloadHandler.text) ;
            GPSCoordinate[] hotspots = jsonHotspots.hotspots;
            lokescions = new Hashtable();
            
            foreach(GPSCoordinate hotspot in hotspots)
            {
                lokescions.Add(new Vector2(hotspot.Gps_position.x, hotspot.Gps_position.y), hotspot.Name);
            }

            locations = new QuadTree(new Rect(40.5f, 14.5f, 1f, 1f), 2);

            foreach (Vector2 point in lokescions.Keys)
            {
                locations.Insert(point);
            }
            Debug.Log("Servizio verosimilmente svolto con successo, benché in maniera umiliante");
        } else
        {
            Debug.Log("Mannaggia cristo chissà mo che cazzo è successo con 'sta web request del cazzo");
        }

        yield break;

    }

    
}
