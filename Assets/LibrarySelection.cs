using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LibrarySelection : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown thisDropdown;

    
    private void Start()
    {
        thisDropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(int value)
    {
        InterSceneData.changeLibrary(thisDropdown.options[value].text);
    }

}
