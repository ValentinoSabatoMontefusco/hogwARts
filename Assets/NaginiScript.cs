using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaginiScript : MonoBehaviour
{
    [SerializeField]
    GameObject serpenteseGO;
    void Start()
    {
        VoiceScript.sprach += Echidna;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Echidna(string sprach)
    {
        if (string.Compare(sprach, "echidna", System.StringComparison.OrdinalIgnoreCase) == 0)
        {
            serpenteseGO.SetActive(false);
            serpenteseGO.SetActive(true);
        }
    }
}
