using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveTest : MonoBehaviour
{
    public GameObject debugTest;
    public GameObject gollumPrefab;
    public GameObject silentPrefab;
    public GameObject goldenPrefab;
    public GameObject catPrefab;
    public GameObject thisCamera;


    public void GollumTest()
    {
        Vector3 spawnPosition = thisCamera.transform.position + Vector3.forward;
        Instantiate(gollumPrefab, spawnPosition, thisCamera.transform.rotation);
        gameObject.SetActive(false);
    }

    public void SHTest()
    {
        TestFeatures.swipeEnabled = true;
        Vector3 spawnPosition = thisCamera.transform.position + Vector3.forward;
        Instantiate(silentPrefab, spawnPosition, thisCamera.transform.rotation);
        gameObject.SetActive(false);
        
    }

    public void RayTest()
    {

        Vector3 spawnPosition = thisCamera.transform.position + Vector3.forward;
        Instantiate(goldenPrefab, spawnPosition, thisCamera.transform.rotation);
        gameObject.SetActive(false);
    }

    public void CatTest()
    {
        Vector3 spawnPosition = thisCamera.transform.position + Vector3.forward;
        Instantiate(catPrefab, spawnPosition, thisCamera.transform.rotation);
        gameObject.SetActive(false);
    }
}
