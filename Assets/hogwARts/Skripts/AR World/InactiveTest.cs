using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveTest : MonoBehaviour
{
    public GameObject debugTest;
    public GameObject medallionPrefab;
    public GameObject diaryPrefab;
    public GameObject goldenPrefab;
    public GameObject naginiPrefab;
    public GameObject cupPrefab;
    public GameObject thisCamera;

    public TestFeatures supportingScript;


    public void MedallionTest()
    {

        Vector3 spawnPosition = thisCamera.transform.position + Vector3.forward;
        Instantiate(medallionPrefab, spawnPosition, thisCamera.transform.rotation);
        gameObject.SetActive(false);
    }

    public void DiaryTest()
    {
        //TestFeatures.swipeEnabled = true;
        //supportingScript.SpawnEquip("spada");
        Vector3 spawnPosition = thisCamera.transform.position + Vector3.forward;
        Instantiate(diaryPrefab, spawnPosition, thisCamera.transform.rotation);
        gameObject.SetActive(false);
        
    }

    public void SnitchTest()
    {
        supportingScript.SpawnEquip("spada");
        Vector3 spawnPosition = thisCamera.transform.position + thisCamera.transform.forward;
        Instantiate(goldenPrefab, spawnPosition, thisCamera.transform.rotation);
        gameObject.SetActive(false);
    }

    public void NaginiTest()
    {

        Vector3 spawnPosition = thisCamera.transform.position + thisCamera.transform.forward;
        Instantiate(naginiPrefab, spawnPosition, thisCamera.transform.rotation);
        gameObject.SetActive(false);
    }

    public void CupTest()
    {
        supportingScript.SpawnEquip("spada");
        Vector3 spawnPosition = this.thisCamera.transform.position + thisCamera.transform.forward;
        Instantiate(cupPrefab, spawnPosition, thisCamera.transform.rotation);
        gameObject.SetActive(false);
    }

    public void ToothTest()
    {
        Vector3 spawnPosition = this.thisCamera.transform.position + thisCamera.transform.forward;
        Instantiate(diaryPrefab, spawnPosition, thisCamera.transform.rotation);
        supportingScript.SpawnEquip("dente");
        gameObject.SetActive(false);
    }
}
