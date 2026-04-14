using System.Collections.Generic;
using UnityEngine;

public class Tileinfo : MonoBehaviour
{
    public int x; // axial coordinate
    public int y; // axial coordinate

    public List<Tileinfo> neighbors = new List<Tileinfo>();

    public Material[] variants; // Materialpick

    void Start()
    {
        int randomIndex = Random.Range(0, variants.Length);
        GetComponent<Renderer>().material = variants[randomIndex];
    }

    public void SetCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}