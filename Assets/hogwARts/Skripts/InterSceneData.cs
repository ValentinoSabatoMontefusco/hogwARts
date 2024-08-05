using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InterSceneData
{
    public static GPSCoordinate currentNearestLocation = null;
    public static string chosenLibrary = "Current Location";
    public static bool debugMode = false;

    public static Color GetColorByLocation(GPSCoordinate location)
    {
        Debug.Log("InterSceneData reads location name: " + location.Name);
        Color newColor;
        if (colourz.TryGetValue(location.Name, out newColor))
            return newColor;
        return Color.black;

    }

    public static void changeLibrary(string lib)
    {
        chosenLibrary = lib;
    }

    private static Dictionary<string, Color> colourz = new Dictionary<string, Color> { { "stanza di Valentino", Color.red }, {"PAM nderro casa", Color.green }, { "villa comunale", Color.blue } };

}

