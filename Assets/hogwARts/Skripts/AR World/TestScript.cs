using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TestScript : MonoBehaviour
{
    // Reference to AR tracked image manager component
    [SerializeField]
    private ARTrackedImageManager _trackedImagesManager;

    // List of prefabs to instantiate - these should be named the same
    // as their corresponding 2D images in the reference image library 
    public GameObject[] ArPrefabs;

    public GameObject latestTrackedGO;
    public GameObject latestInstGO;

    private TextMeshProUGUI latestTracked;
    private TextMeshProUGUI latestInst;

    private readonly Dictionary<string, GameObject> _instantiatedPrefabs = new Dictionary<string, GameObject>();

    public GameObject diaryPrefab;
    public GameObject naginiPrefab;
    public GameObject costaPrefab;
    public GameObject mercadantePrefab;
    public GameObject boccinoPrefab;
    public GameObject medallionPrefab;
    public GameObject euroDevinettePrefab;
    public GameObject diademaPrefab;
    public GameObject diarioPrefab;
    public GameObject hufflepuffCup;

    public XRReferenceImageLibrary mercatoneLibrary;
    public XRReferenceImageLibrary fontanaLibrary;
    public XRReferenceImageLibrary fantasmaLibrary;
    public XRReferenceImageLibrary museoLibrary;
    public XRReferenceImageLibrary croceverdeneraLibrary;
    public XRReferenceImageLibrary ultimacenaLibrary;
    public XRReferenceImageLibrary defaultLibrary;



    public Dictionary<string, GameObject> imageToPrefab;



    //  foreach(var trackedImage in eventArgs.added){
    // Get the name of the reference image
    //   var imageName = trackedImage.referenceImage.name;
    //   foreach (var curPrefab in ArPrefabs) {
    // Check whether this prefab matches the tracked image name, and that
    // the prefab hasn't already been created
    //  if (string.Compare(curPrefab.name, imageName, StringComparison.OrdinalIgnoreCase) == 0
    //   && !_instantiatedPrefabs.ContainsKey(imageName)){
    // Instantiate the prefab, parenting it to the ARTrackedImage
    //  var newPrefab = Instantiate(curPrefab, trackedImage.transform);
    void Awake()
    {
        if (_trackedImagesManager == null)
        {
            _trackedImagesManager = GetComponent<ARTrackedImageManager>();
        }
        
        
        latestTracked = latestTrackedGO.GetComponent<TextMeshProUGUI>();
        latestInst = latestInstGO.GetComponent<TextMeshProUGUI>();
        imageToPrefab = new Dictionary<string, GameObject>()
                { { "costapertina",  costaPrefab}, {"taragozza", mercadantePrefab }, {"caccaviello_rosso1", naginiPrefab },
                {"caccaviello_rosso2", naginiPrefab }, {"anceli_mercatone", naginiPrefab }, {"effigie_foresta", naginiPrefab },
                {"ciorellino", boccinoPrefab }, {"10euro_fronte", medallionPrefab }, {"10euro_retro", medallionPrefab }, {"CroceVerdeNera", euroDevinettePrefab},
                {"UltimaCena", boccinoPrefab }, {"MuseoDellaMemoria", diademaPrefab }, {"FantasmaScuola", diarioPrefab }, {"StemmaFontana", hufflepuffCup }, {"ChitarraVerde", euroDevinettePrefab } };

        latestTracked.text += "TestScript awakened...";

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
    private void OnEnable()
    {
        Debug.Log("Started TestScript OnEnable()");
        _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
        Debug.Log("Added callback to imagesChanged event");
        if (InterSceneData.currentNearestLocation == null)
            return;
        Debug.Log("InterScenData: current location name -> " + InterSceneData.currentNearestLocation.Name);
        
        try
        {

            _trackedImagesManager.enabled = false;
            Debug.Log("ImagesManager disabled, even though it should've already been so");
            if (InterSceneData.chosenLibrary != null && InterSceneData.chosenLibrary != "Current Location")
            {
                Debug.Log("Menù a tendina non currentlokesionesco");
                switch(InterSceneData.chosenLibrary)
                {
                    case "UltimaCena": _trackedImagesManager.referenceLibrary = ultimacenaLibrary; break;
                    case "StemmaFontana": _trackedImagesManager.referenceLibrary = fontanaLibrary; break;
                    case "FantasmaScuola": _trackedImagesManager.referenceLibrary = fantasmaLibrary; break;
                    case "AnceliMercatone": _trackedImagesManager.referenceLibrary = mercatoneLibrary; break;
                    case "MuseoDellaMemoria": _trackedImagesManager.referenceLibrary = museoLibrary; break;
                    case "CroceVerdeNera": _trackedImagesManager.referenceLibrary = croceverdeneraLibrary; break;
                    default: _trackedImagesManager.referenceLibrary = defaultLibrary; break;
                }
                latestInst.text = "Current Library = " + _trackedImagesManager.referenceLibrary.ToString();
                Debug.Log("Libreria scelta tramite menù a tendina: " + _trackedImagesManager.referenceLibrary.ToString());
            } else
            {
                Debug.Log("Scegliendo libreria per prossimità");
                switch (InterSceneData.currentNearestLocation.Name)
                { 
                    case "UltimaCena":
                        _trackedImagesManager.referenceLibrary = ultimacenaLibrary;
                        latestInst.text = "NearestLocation = " + InterSceneData.currentNearestLocation.Name; break;
                    case "AnceliMercatone":
                        _trackedImagesManager.referenceLibrary = mercatoneLibrary;
                        latestInst.text = "NearestLocation = " + InterSceneData.currentNearestLocation.Name; break;
                    case "StemmaFontana":
                        _trackedImagesManager.referenceLibrary = fontanaLibrary;
                        latestInst.text = "NearestLocation = " + InterSceneData.currentNearestLocation.Name; break;
                    case "Quarto beach arena":
                    case "FantasmaScuola":
                        _trackedImagesManager.referenceLibrary = fantasmaLibrary;
                        latestInst.text = "NearestLocation = " + InterSceneData.currentNearestLocation.Name; break;
                    case "MuseoDellaMemoria":
                        _trackedImagesManager.referenceLibrary = museoLibrary;
                        latestInst.text = "NearestLocation = " + InterSceneData.currentNearestLocation.Name; break;
                    case "PAM nderro casa":
                    case "CroceVerdeNera":
                        _trackedImagesManager.referenceLibrary = croceverdeneraLibrary;
                        latestInst.text = "NearestLocation = " + InterSceneData.currentNearestLocation.Name; break;
                    default:
                        _trackedImagesManager.referenceLibrary = defaultLibrary;
                        latestInst.text = "NearestLocation = " + InterSceneData.currentNearestLocation.Name; break;
                }
                Debug.Log("Libreria scelta tramite prossimità: " + _trackedImagesManager.referenceLibrary.ToString());
            }

            Debug.Log("Tentativo di abilitazione ImageManager");
            _trackedImagesManager.enabled = true;
            Debug.Log("Manager abilitato");

        }
        catch (Exception e)
        {
            latestInst.text = e.Message;
            Debug.Log(e.Message);
        }


    }
    private void OnDisable()
    {
        _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        GameObject latestPrefab;
        //latestTracked.text = "OnTrackedImagesChanged called";
        foreach (var trackedImage in eventArgs.added)
        {
            // Get the name of the reference image
            var imageName = trackedImage.referenceImage.name;
            GameObject chosenPrefab;
            bool debugBool = imageToPrefab.TryGetValue(imageName, out chosenPrefab);
            latestTracked.text = "Checking image with name " + imageName;
            Debug.Log("Checking image with name " + imageName);
            if (!debugBool)
            {
                latestTracked.text = "Error with tracking!";
                return;
            }
            else
            {

               if (trackedImage.referenceImage.name == "CroceVerdeNera" || chosenPrefab.name == "Devinette" || trackedImage.referenceImage.name == "ChitarraVerde")
                {
                    euroDevinettePrefab.SetActive(true);
                    latestTracked.text = "Devinette?";
                }
                else if (!_instantiatedPrefabs.ContainsKey(chosenPrefab.name))
                {
                    latestPrefab = Instantiate(chosenPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    _instantiatedPrefabs.Add(chosenPrefab.name, latestPrefab);
                    latestTracked.text = "Latest Tracked: " + imageName + " " + formattedPosition(trackedImage.transform);
                    latestInst.text = "Latest Inst: " + chosenPrefab.name + " " + formattedPosition(latestPrefab.transform);


                }
            }
                
            return;
            //foreach (var curPrefab in ArPrefabs)
            //{
            //    // Check whether this prefab matches the tracked image name, and that
            //    // the prefab hasn't already been created
            //    if (string.Compare(curPrefab.name, imageName, StringComparison.OrdinalIgnoreCase) == 0
            //     && !_instantiatedPrefabs.ContainsKey(imageName))
            //    {

            //        //if (string.Compare(imageName, "taragozza", StringComparison.OrdinalIgnoreCase) == 0)
            //        //{
            //        //    Vector3 slightlyAhead = trackedImage.transform.position + Vector3.zero;
            //        //    var newPrefab = Instantiate(curPrefab, slightlyAhead, Quaternion.identity);

            //        //} else { 
            //        // Instantiate the prefab, parenting it to the ARTrackedImage
            //        latestPrefab = Instantiate(curPrefab, trackedImage.transform);

            //        latestTracked.text = "Latest Tracked: " + imageName + " " + formattedPosition(trackedImage.transform);
            //        latestInst.text = "Latest Inst: " + curPrefab.name + " " + formattedPosition(latestPrefab.transform);
            //        //}
            //    }
            //}
        }
        foreach (var trackedImage in eventArgs.updated)
        {
            // var imageName = trackedImage.referenceImage.name;
            //foreach (var curPrefab in AreaEffector2DPrefabs)
            // {
            // if (string.Compare(curPrefab.name, imageName, StringComparison.OrdinalIgnoreCase) == 0
            //  && !_instantiatedPrefabs.ContainsKey(imageName))
            //  {
            //  var newPrefab = Instantiate(curPrefab, trackedImage.transform);
            //   _instantiatedPrefabs[imageName] = newPrefab;
            // }
            // }
            // }
            //_instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(trackedImage.trackingState == TrackingState.Tracking);
            latestTracked.text = "Latest Tracked: " + trackedImage.name + " " + formattedPosition(trackedImage.transform);
            latestInst.text = "Latest Inst: " + _instantiatedPrefabs[trackedImage.name].name + " " + formattedPosition(_instantiatedPrefabs[trackedImage.name].transform);
        }


        foreach (var trackedImage in eventArgs.removed)
        {
            // Destroy its prefab
            //Destroy(_instantiatedPrefabs[trackedImage.referenceImage.name]);
            // Also remove the instance from our array
            //_instantiatedPrefabs.Remove(trackedImage.referenceImage.name);
            // Or, simply set the prefab instance to inactive
            //_instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(false);
        }
    }

    private String formattedPosition(Transform transform)
    {

        return "(" + transform.localPosition.x + ", " + transform.localPosition.y + ", " + transform.localPosition.z + ")";
    }
}