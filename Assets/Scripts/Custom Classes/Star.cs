using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Star : MonoBehaviour
{
    //Star neighbours
    public Dictionary<Star, float> starNeighbours = new Dictionary<Star, float>();
    [HideInInspector]
    public List<Star> routesToGenerate = new List<Star>();
    //Star Values
    [Header("Star Values")]
    public string ID;
    public string faction;

    /// <summary>
    /// Populates the routesToGenerateList so that route meshes can be spawned between each star
    /// </summary>
    public void FillRouteList() {
        routesToGenerate = starNeighbours.Keys.ToList<Star>();
    }
}
