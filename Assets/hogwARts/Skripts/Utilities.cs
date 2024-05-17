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
}

