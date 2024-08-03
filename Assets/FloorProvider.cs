using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FloorProvider : MonoBehaviour
{
    int subscribersCount;
    [SerializeField]
    ARPlaneManager planeManager;
    ARPlane currentBestFloor;
    float bestEval;
    

    private void Awake()
    {
        subscribersCount = 0;
    }

    private void Start()
    {
        if (planeManager == null)
        {
            planeManager = GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>();
        }
        bestEval = -100;
    }
    public void Subscribe()
    {
        subscribersCount++;
        if (subscribersCount == 1 )
        {
            planeManager.planesChanged += PlaneDetection;
        }
        
    }

    public void Unsubscribe()
    {
        subscribersCount--;
        if (subscribersCount <= 0)
        {
            planeManager.planesChanged -= PlaneDetection;
        }
    }

    public ARPlane ProvidePlane()
    {
        Debug.Log("Provide plane called, returning " + (currentBestFloor == null ? "null" : "something"));
        if (currentBestFloor != null)
     
            return currentBestFloor;
        return null;
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

        if (currentBestFloor == null)
        {
            currentBestFloor = p;
            return;
        }

        float planeEvaluation = PlaneEvaluation(p, player);

        if (planeEvaluation > bestEval)
            currentBestFloor = p;
    }
    public float PlaneEvaluation(ARPlane p, Transform player)
    {

        float distance = Utilities.DistanceFromPointToPlane(player.position, p.normal, p.center);
        float distanceEvaluation = -2 * Mathf.Pow(distance, 2) + 5 * distance;
        float size = p.size.x * p.size.y;
        float sizeEvaluation = Mathf.Log(size, 8);

        return distanceEvaluation + sizeEvaluation;

    }

}
