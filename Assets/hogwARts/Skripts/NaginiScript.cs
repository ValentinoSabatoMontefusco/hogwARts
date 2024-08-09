    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class NaginiScript : MonoBehaviour
{
    [SerializeField]
    GameObject serpenteseGO;
    [SerializeField]
    GameObject gridPrefab;

    FloorProvider floorProvider;

    
    ARPlane actualFloor;
    

    bool instancedGrid;
    
    

    void Start()
    {
        //planeManager = GameObject.FindGameObjectWithTag("ARControllers").GetComponent<ARPlaneManager>();
        instancedGrid = false;
        VoiceScript.sprach += Echidna;
        floorProvider = GameObject.Find("AR Session Origin").GetComponent<FloorProvider>();
        floorProvider.Subscribe();
    }

    private void OnDisable()
    {
        floorProvider.Unsubscribe();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            Echidna("echidna");
    }



    private void Echidna(string sprach)
    {
        if (string.Compare(sprach, "echidna", System.StringComparison.OrdinalIgnoreCase) == 0)
        {
            if (!instancedGrid)
            {
                actualFloor = floorProvider.ProvidePlane();
                GameObject gridsies;
                if (actualFloor != null)
                {
                    float naginiHeight = Utilities.DistanceFromPointToPlane(this.transform.position, actualFloor.normal, actualFloor.center);
                    gridsies = GameObject.Instantiate(gridPrefab, this.transform);
                    gridsies.transform.position -= new Vector3(0, naginiHeight, 4.5f);
                } else
                {
                    gridsies = GameObject.Instantiate(gridPrefab, this.transform);
                    gridsies.transform.position -= new Vector3(0, 1.5f, 4.5f);
                }
                instancedGrid = true;
            }
            serpenteseGO.SetActive(false);
            serpenteseGO.SetActive(true);
        }
    }
}


/*
 private void OnEnable()
    {
        if(planeManager != null)
        {
            planeManager.planesChanged += PlaneDetection;
        }
        
    }

    private void OnDisable()
    {
        if (planeManager != null)
        {
            planeManager.planesChanged -= PlaneDetection;
        }
        
    }

    private void PlaneDetection(ARPlanesChangedEventArgs eventArgs)
    {
        Transform player = Camera.main.transform;
        foreach (ARPlane p in eventArgs.added)
        {

            PlaneProcess(p, player);

        }
        foreach (ARPlane p in eventArgs.updated)
        {
            PlaneProcess(p, player);
        }

    }


    private void PlaneProcess(ARPlane p, Transform player)
    {
        if (p.alignment != UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            return;

        if (actualFloor == null)
        {
            actualFloor = p;
            return;
        }

        float planeEvaluation = PlaneEvaluation(p, player);

        if (planeEvaluation > bestEval)
            actualFloor = p;
    }
    public float PlaneEvaluation(ARPlane p, Transform player)
    {

        float distance = Utilities.DistanceFromPointToPlane(player.position, p.normal, p.center);
        float distanceEvaluation = -2 * Mathf.Pow(distance, 2) + 5 * distance;
        float size = p.size.x * p.size.y;
        float sizeEvaluation = Mathf.Log(size, 8);

        return distanceEvaluation + sizeEvaluation;

    }
*/