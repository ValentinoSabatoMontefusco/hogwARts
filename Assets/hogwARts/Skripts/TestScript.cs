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
    private ARTrackedImageManager _trackedImagesManager;

    // List of prefabs to instantiate - these should be named the same
    // as their corresponding 2D images in the reference image library 
    public GameObject[] ArPrefabs;

    public GameObject latestTrackedGO;
    public GameObject latestInstGO;

    private TextMeshProUGUI latestTracked;
    private TextMeshProUGUI latestInst;

    private readonly Dictionary<string, GameObject> _instantiatedPrefabs = new Dictionary<string, GameObject>();
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
        _trackedImagesManager = GetComponent<ARTrackedImageManager>();
        latestTracked = latestTrackedGO.GetComponent<TextMeshProUGUI>();
        latestInst = latestInstGO.GetComponent<TextMeshProUGUI>();
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
        _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
        
    }
    private void OnDisable()
    {
        _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        GameObject latestPrefab;
        foreach (var trackedImage in eventArgs.added)
        {
            // Get the name of the reference image
            var imageName = trackedImage.referenceImage.name;
            foreach (var curPrefab in ArPrefabs)
            {
                // Check whether this prefab matches the tracked image name, and that
                // the prefab hasn't already been created
                if (string.Compare(curPrefab.name, imageName, StringComparison.OrdinalIgnoreCase) == 0
                 && !_instantiatedPrefabs.ContainsKey(imageName))
                {

                    //if (string.Compare(imageName, "taragozza", StringComparison.OrdinalIgnoreCase) == 0)
                    //{
                    //    Vector3 slightlyAhead = trackedImage.transform.position + Vector3.zero;
                    //    var newPrefab = Instantiate(curPrefab, slightlyAhead, Quaternion.identity);

                    //} else { 
                    // Instantiate the prefab, parenting it to the ARTrackedImage
                    latestPrefab = Instantiate(curPrefab, trackedImage.transform);

                    latestTracked.text = "Latest Tracked: " + imageName +  " " + formattedPosition(trackedImage.transform);
                    latestInst.text = "Latest Inst: " + curPrefab.name + " " + formattedPosition(latestPrefab.transform);
                    //}
                }
            }
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
            _instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(trackedImage.trackingState == TrackingState.Tracking);
            latestTracked.text = "Latest Tracked: " + trackedImage.name + " " + formattedPosition(trackedImage.transform);
            latestInst.text = "Latest Inst: " + _instantiatedPrefabs[trackedImage.name].name + " " + formattedPosition(_instantiatedPrefabs[trackedImage.name].transform);
        }


        foreach (var trackedImage in eventArgs.removed)
        {
            // Destroy its prefab
            Destroy(_instantiatedPrefabs[trackedImage.referenceImage.name]);
            // Also remove the instance from our array
            _instantiatedPrefabs.Remove(trackedImage.referenceImage.name);
            // Or, simply set the prefab instance to inactive
            //_instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(false);
        }
    }

    private String formattedPosition(Transform transform)
    {

        return "(" + transform.position.x + ", " + transform.position.y + ", " + transform.position.z + ")";
    }
}