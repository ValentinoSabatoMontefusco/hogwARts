using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static string EarthDistanceFormattedString (float distance)
    {
        if (distance > 1000)
            return (distance / 1000).ToString("F2") + " km";
        else
            return distance.ToString("F2") + " m";
    }

    public static float HeavertineDistanceM(Vector2 sourcePos, Vector2 targetPos)
    {
        
        float earthRadius = 6371000f; // Earth radius in meters
        float dLat = Mathf.Deg2Rad * (targetPos.x - sourcePos.x);
        float dLon = Mathf.Deg2Rad * (targetPos.y - sourcePos.y);
        float lat1 = Mathf.Deg2Rad * sourcePos.x;
        float lat2 = Mathf.Deg2Rad * targetPos.x;

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                  Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2) * Mathf.Cos(lat1) * Mathf.Cos(lat2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        float distance = earthRadius * c;

        return distance;
    }

    public static float EuclideanDistanceM(Vector2 userLocation, Vector2 destinationLocation)
    {
        // Approximate latitude and longitude as Cartesian coordinates
        float deltaX = destinationLocation.x - userLocation.x;
        float deltaY = destinationLocation.y - userLocation.y;

        // Assuming 1 degree of latitude = 111 kilometers and 1 degree of longitude = 111 kilometers at the equator
        float latitudeConversionFactor = 111000; // Approximation for latitude at the equator (in meters)
        float longitudeConversionFactor = 111000; // Approximation for longitude at the equator (in meters)

        // Calculate Euclidean distance using Pythagorean theorem
        float distance = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);

        // Convert distance to meters using latitude conversion factor
        distance *= Mathf.Max(latitudeConversionFactor, longitudeConversionFactor);

        return distance;
    }

    public static float SquaredDistanceToRect(Vector2 point, Rect rect)
    {
        // Clamp the point to the rectangle boundary
        float x = Mathf.Max(rect.xMin, Mathf.Min(point.x, rect.xMax));
        float y = Mathf.Max(rect.yMin, Mathf.Min(point.y, rect.yMax));

        // Calculate squared distance between clamped point and original point
        float dx = x - point.x;
        float dy = y - point.y;
        float squaredDistance = dx * dx + dy * dy;

        return squaredDistance;
    }

    public static float CalculateBearing(float lat1, float lon1, float lat2, float lon2)
    {
        double latitude1 = Mathf.Deg2Rad * lat1;
        double latitude2 = Mathf.Deg2Rad * lat2;
        double longDiff = Mathf.Deg2Rad * (lon2 - lon1);

        double y = Mathf.Sin((float)longDiff) * Mathf.Cos((float)latitude2);
        double x = Mathf.Cos((float)latitude1) * Mathf.Sin((float)latitude2) - Mathf.Sin((float)latitude1) * Mathf.Cos((float)latitude2) * Mathf.Cos((float)longDiff);

        return (Mathf.Atan2((float)y, (float)x) * Mathf.Rad2Deg + 360) % 360;
    }

    public static float CalculateHeading(Vector3 acceleration, Vector3 magnetometer)
    {
        float heading = Mathf.Atan2(magnetometer.y, magnetometer.x) * Mathf.Rad2Deg;
        return heading < 0 ? heading += 360 : heading;
    }

    public static string ElaborateDirection(float directionValue)
    {
        string directionString = "indefinite";

        if (directionValue < 22.5 || directionValue >= 337.5)
        {
            directionString = "Ahead";
        }
        else if (directionValue >= 22.5 && directionValue < 67.5)
        {
            directionString = "Northeast";
        }
        else if (directionValue >= 67.5 && directionValue < 112.5)
        {
            directionString = "Right";
        }
        else if (directionValue >= 112.5 && directionValue < 157.5)
        {
            directionString = "Southeast";
        }
        else if (directionValue >= 157.5 && directionValue < 202.5)
        {
            directionString = "Behind";
        }
        else if (directionValue >= 202.5 && directionValue < 247.5)
        {
            directionString = "Southwest";
        }
        else if (directionValue >= 247.5 && directionValue < 292.5)
        {
            directionString = "Left";
        }
        else if (directionValue >= 292.5 && directionValue < 337.5)
        {
            directionString = "Northwest";
        }
        return directionString;
    }

    public static float LogLerp(float a, float b, float t)
    {
        // Ensure a and b are positive to avoid issues with logarithms
        if (a <= 0 || b <= 0)
        {
            Debug.LogError("Logarithmic interpolation requires positive values.");
            return 0;
        }

        return a * Mathf.Pow(b / a, t);
    }

}

