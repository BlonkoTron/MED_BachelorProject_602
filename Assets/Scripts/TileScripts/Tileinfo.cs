using System.Collections.Generic;
using UnityEngine;

public class Tileinfo : MonoBehaviour
{
    public int x; // axial coordinate
    public int y; // axial coordinate

    public List<Tileinfo> neighbors = new List<Tileinfo>();

    public void SetCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}