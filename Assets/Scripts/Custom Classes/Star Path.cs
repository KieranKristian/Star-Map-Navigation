using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class StarPath
{
    //Path Details
    public List<Star> stars = new List<Star>();
    public float length;

    /// <summary>
    /// Calculates the length of the route by check each star in the list of stars neighbours,
    /// If the neighbour is included in the stars list as well add the distance
    /// </summary>
    public void CalculateDistance() {
        List<Star> calculated = new List<Star>();
        length = 0f;
        for (int i = 0; i < stars.Count; i++) {
            Star star = stars[i];
            for (int j = 0; j < star.starNeighbours.Count; j++) {
                Star neighbour = star.starNeighbours.Keys.ToList()[j];

                //Dont calculate calculated stars
                if (stars.Contains(neighbour) && !calculated.Contains(neighbour)) {
                    length += star.starNeighbours[neighbour];
                } 
            }
            calculated.Add(star);
        }
    }
}