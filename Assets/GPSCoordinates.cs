using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSCoordinate
{
   
    private Vector2 gps_position;
    private string name;

    public GPSCoordinate(float longitude, float latitude)
    {
        this.gps_position = new Vector2(longitude, latitude);
    }

    public GPSCoordinate(float longitude, float latitude, string name)
    {
        this.gps_position = new Vector2(longitude, latitude);
        this.name = name;
    }

    public Vector2 Gps_position { get => gps_position; set => gps_position = value; }
    public string Name { get => name; set => name = value; }

    public float distanceTo(GPSCoordinate other)
    {
        return Vector2.Distance(this.gps_position, other.gps_position);

    }
}
