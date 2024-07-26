using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class MainMenuScript : MonoBehaviour
{
    public Text menuText;
    [SerializeField]
    GameObject Deathly;
    [SerializeField]
    GameObject debugView;
    [SerializeField]
    GameObject mainView;
    [SerializeField]
    GameObject rippleEffect;
    //LocationService locServ;
    const float MILLS = 0.001f;
    QuadTree locations;
    float latestDistance = float.MaxValue;
    private readonly static float SPIN_THRESHOLD = 15; // RIPORTARE A 25 O 20

    

    GPSCoordinate myRoom = new GPSCoordinate(40.91540078137776f, 14.82838650405708f, "stanza di Valentino");
    Hashtable lokescions;
    LocalizationLogic locLogic;
    bool isMenuDebug = true;
    
    
    public void Awake()
    {
        latestDistance = float.MaxValue;
        isMenuDebug = true;
        locLogic = GetComponent<LocalizationLogic>();
        locLogic.onLocationCheck.AddListener(updateUI);
        //locServ = new LocationService();
        //locServ.Start();
        //menuText.text = "Location Service tried to activate and did so";

        //lokescions = new Hashtable();
        //menuText.text += ".";
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    StartCoroutine(CoroutineUmiliante());
        //} else
        //{
        //    GPSCoordinate[] hotspots = InitializeHotspots();

        //    menuText.text += hotspots.Length;
        //    //Debug.Log(hotspots[0]);
        //    foreach (GPSCoordinate hotspot in hotspots)
        //    {
        //        lokescions.Add(new Vector2(hotspot.Gps_position.x, hotspot.Gps_position.y), hotspot.Name);
        //    }

        //    menuText.text += ".";

        //    locations = new QuadTree(new Rect(40.5f, 14.5f, 1f, 1f), 2);

        //    foreach (Vector2 point in lokescions.Keys)
        //    {
        //        locations.Insert(point);
        //    }
        //    menuText.text += ".";
        //}






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
        //menuText.text += ".";
        //Permission.RequestUserPermission(Permission.FineLocation);

        //menuText.text += ".";

        //if (locServ.status == LocationServiceStatus.Running)
        //{
        //    StartCoroutine(checkLocation());
        //    menuText.text += ". succesfully.";
        //} else
        //    menuText.text += ". failurely :c";
        
        

    }

    private void updateUI(LocationInfoPackage locInfo)
    {
        latestDistance = locInfo.Distance;
        Deathly.GetComponent<Animator>().SetFloat("latestDistance", latestDistance);
        if (isMenuDebug)
        {
            menuText.text = "Thy current coordinates are: Lat ~ " + locInfo.CurrentLocation.y + ", Long: ~ " + locInfo.CurrentLocation.x + "\n";
            
            if (locInfo.Distance < SPIN_THRESHOLD)
            {
                menuText.text += "\nThey kinda match ";
            }
            else if (locInfo.Distance < 100)
            {
                menuText.text += "\nThey're pristy close to ";
            }
            else
            {

                menuText.text += "\nThey do be pretty darn far from ";
            }

            menuText.text += locInfo.NearestNeighbor.Name + " (" + Utilities.EarthDistanceFormattedString(locInfo.Distance) + ")";
            menuText.text += "\n NearHot: Lat: " + locInfo.NearestNeighbor.Gps_position.y.ToString("F3") + ", Lng: " + locInfo.NearestNeighbor.Gps_position.x.ToString("F3");
            menuText.text += "\n Direction: " + Utilities.ElaborateDirection(locInfo.DirectionAngle) + " (" + locInfo.DirectionAngle + "°)";
            menuText.color = Color.Lerp(Color.green, new Color(1, 1, 0.4f, 0.5f), locInfo.Distance < 500 ? locInfo.Distance / 500 : 1);

        } else
        {
            Deathly.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0.3f), locInfo.Distance < 500 ? locInfo.Distance / 500 : 1);
            float lerpScale = Utilities.LogLerp(0.5f, 1.8f, locInfo.Distance > 500 ? 0 : (500 - locInfo.Distance) / 500);
            Deathly.transform.localScale = new Vector3(lerpScale, lerpScale, lerpScale);
            //Deathly.GetComponent<Image>().color = InterSceneData.GetColorByLocation(InterSceneData.currentNearestLocation);
            if (latestDistance > SPIN_THRESHOLD)
            {
                //Deathly.GetComponent<Animator>().SetBool("SpinSpin", false);
                StopCoroutine("rotationAnimation");
                StartCoroutine(rotationAnimation(Deathly.transform, Quaternion.Euler(0, 0, -locInfo.DirectionAngle), LocalizationLogic.UPDATE_INTERVAL));
                Debug.Log("Started rotation animation coroutine with angle: " + (-locInfo.DirectionAngle));
            }
            else
            {
                StartCoroutine(rotationAnimation(Deathly.transform, Mathf.Lerp(2.5f, 0.75f, latestDistance / SPIN_THRESHOLD), LocalizationLogic.UPDATE_INTERVAL));
                //Deathly.GetComponent<Animator>().SetBool("SpinSpin", true);
            }
            

            //Deathly.transform.rotation = Quaternion.Euler(0, 0, - locInfo.DirectionAngle);
        }
    }

    IEnumerator rotationAnimation(Transform objectTransform, float multiplier, float duration)
    {
        Debug.Log("SpinSpin activated");
        float remainingDuration = duration;
        while ((remainingDuration -= Time.deltaTime) > 0)
        {
            objectTransform.RotateAround(objectTransform.position, Vector3.forward, Time.deltaTime * (-360) * multiplier);
            yield return null;
        }
    }

    IEnumerator rotationAnimation(Transform objectTransform, Quaternion targetRotation, float duration)
    {
        Debug.Log("Trying to turn Deathly towards objective");
        Transform startingTransform = objectTransform;
        float startingDuration = duration;

        while (((duration -= Time.deltaTime) > 0))
        {

            objectTransform.rotation = Quaternion.Lerp(targetRotation, objectTransform.rotation, duration / startingDuration);

            yield return null;
        }

    }


    public void DeathClick(GameObject source)
    {
        if (latestDistance <= SPIN_THRESHOLD)
        {
            LoadScene("AR_Camera_Scene");
        }
        else
        {
            StartCoroutine(Bumpie(source));


            
            
            
        }

    }

    public static IEnumerator Bumpie(GameObject bumped)
    {
        {

            Vector3 scale = bumped.transform.localScale;
            float duration = 0.3f;
            float timeRemaining = duration;

            while ((timeRemaining -= Time.deltaTime) > 0)
            {
                if (timeRemaining > 0.2f)
                {
                    bumped.transform.localScale = Vector3.Lerp(scale * 0.88f, scale, (timeRemaining - 0.2f) / 0.1f);
                }
                else
                if (timeRemaining > 0.1f)
                {
                    bumped.transform.localScale = Vector3.Lerp(scale * 1.15f, scale * 0.88f, (timeRemaining - 0.1f) / 0.1f);
                }
                else
                {
                    bumped.transform.localScale = Vector3.Lerp(scale, scale * 1.15f, timeRemaining / 0.1f);
                }

                yield return null;
            }

        }
    }

    //IEnumerator checkLocation()
    //{
    //    float x;
    //    float y;
    //    do
    //    {
    //        yield return new WaitForSeconds(3.0f);
    //        x = Input.location.lastData.longitude;
    //        y = Input.location.lastData.latitude;

    //        GPSCoordinate currCord = new GPSCoordinate(y, x);

    //        Vector2 nearestNeighbour = locations.FindNearestNeighbor(new Vector2(y, x)); // The Quadtree works with x as latitude and y as longitude
    //        GPSCoordinate nearestHotspot = new GPSCoordinate(nearestNeighbour.x, nearestNeighbour.y, (string)lokescions[nearestNeighbour]);

    //        menuText.text = "Your current coordinates are: Lat: ~ " + y + ", Long: ~ " + x;
    //        float hotspotDistance = Utilities.EuclideanDistanceM(new Vector2(y, x), nearestNeighbour);

    //        if (hotspotDistance < 25)
    //        {
    //            menuText.text += "\nThey match ";
    //        }
    //        else if (hotspotDistance < 100)
    //        {
    //            menuText.text += "\nThey're pristy close to ";
    //        }
    //        else
    //        {

    //            menuText.text += "\nThey do be pretty far from ";
    //        }

    //        menuText.text += nearestHotspot.Name + " (" + Utilities.EarthDistanceFormattedString(hotspotDistance) + ")";
    //        menuText.text += "\n NearHot: Lat: " + nearestHotspot.Gps_position.y.ToString("F3") + ", Lng: " + nearestHotspot.Gps_position.x.ToString("F3");
    //        menuText.color = Color.Lerp(Color.green, new Color(1, 1, 0.4f, 0.5f), hotspotDistance < 500 ? hotspotDistance / 500 : 1);


    //    } while (locServ.status == LocationServiceStatus.Running);

    //    yield break;

    //}

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

    public void swapView()
    {

        if (debugView == null || mainView == null)
        {
            Debug.LogError("Una delle viste è assenti!");
            return;
        }

        if(isMenuDebug)
        {
            debugView.SetActive(false);
            mainView.SetActive(true);
            isMenuDebug = false;
        } else
        {
            debugView.SetActive(true);
            mainView.SetActive(false);
            isMenuDebug = true;
        }
    }
    
}
