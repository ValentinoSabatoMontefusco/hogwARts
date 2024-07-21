using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InterSceneData
{
    public static GPSCoordinate currentNearestLocation = null;

    public static Color GetColorByLocation(GPSCoordinate location)
    {
        Color newColor;
        if (colourz.TryGetValue(location.Name, out newColor))
            return newColor;
        return Color.black;

    }

    private static Dictionary<string, Color> colourz = new Dictionary<string, Color> { { "stanza di Valentino", Color.red }, {"PAM nderro casa", Color.green }, { "villa comunale", Color.blue } };

}

