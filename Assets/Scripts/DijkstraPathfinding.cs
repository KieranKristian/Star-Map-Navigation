using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class DijkstraPathfinding : MonoBehaviour {
    //Lists
    [HideInInspector]
    public List<Star> galaxyStarList;
    [HideInInspector]
    public List<RouteMesh> allRoutes = new List<RouteMesh>();
    [HideInInspector]
    public List<RouteMesh> pathRoutes = new List<RouteMesh>();

    public Star startStar;
    public Star endStar;

    //UI Elements
    [Header("UI Elements")]
    public TextScrolling pathText;
    public TextScrolling pathLengthText;
    public TextScrolling errorMessage;
    public GameObject startButton;

    public StarPath path;

    public GamemodeManager gamemodeManager;

    public bool avoidEnemies;

    /// <summary>
    /// Function that is used by a button in the UI, resets current path and gets a new one
    /// </summary>
    public void GeneratePath() {
        //Resetting current route, if there is one
        startButton.SetActive(false);
        path = null;
        for(int i = 0; i < allRoutes.Count; i++) {
            allRoutes[i].GetComponent<Renderer>().material.color = Color.white;
        }
        pathRoutes.Clear();
        //Generating new route
        path = GetShortestPath(startStar, endStar);
        if(path != null && path.stars.Count > 1) {
            //Checking whether the player has enough fuel
            if (path.length <= gamemodeManager.fuelCapacity) {
                startButton.SetActive(true);
                errorMessage.gameObject.SetActive(false);
            } else {
                errorMessage.gameObject.SetActive(true);
                errorMessage.StartLerp("Sorry, you haven't got enough fuel for this journey");
            }
        }
    }

    /// <summary>
    /// Allows the star spawner to populate the pathfinding algorithm with all stars and route meshes
    /// </summary>
    public void SetGalaxyList(List<Star> stars, List<RouteMesh> routes) {
        galaxyStarList = new List<Star>(stars);
        allRoutes = new List<RouteMesh>(routes);
    }

    /// <summary>
    /// Uses Dijkstra pathfinding to find the shortest possible route from a start and end star
    /// </summary>
    /// <param name="start">The star to start from</param>
    /// <param name="end">The star to end at</param>
    /// <returns>A path containing each star in the shortest path, and the calculated length of the path</returns>
    StarPath GetShortestPath(Star start, Star end) {
        //Returns null if path is not safe
        if (avoidEnemies) {
            if (start.faction == gamemodeManager.enemyFaction.ToString() || end.faction == gamemodeManager.enemyFaction.ToString()) {
                errorMessage.gameObject.SetActive(true);
                errorMessage.StartLerp("Unable to locate safe route between these points");
                return null;
            }
        }

        //Returns null if either stars are null
        if (start == null || end == null) {
            errorMessage.gameObject.SetActive(true);
            errorMessage.StartLerp("Please select a start and end star");
            return null;
        }

        //Final path
        StarPath path = new StarPath();

        //If start and end are the same, add start to path and return
        if (start == end) {
            path.stars.Add(start);
            return path;
        }

        List<Star> unvisited = new List<Star>();

        Dictionary<Star, Star> previous = new Dictionary<Star, Star>();

        Dictionary<Star, float> distances = new Dictionary<Star, float>();

        //Going through each star and setting the distance to infinity
        for (int i = 0; i < galaxyStarList.Count; i++) {
            Star star = galaxyStarList[i];
            unvisited.Add(star);

            //Setting all distances to infinity
            distances.Add(star, float.MaxValue);
        }

        //Setting distance of start star to 0
        distances[start] = 0;
        //While there are still unvisited stars
        while (unvisited.Count != 0) {
            //Ordering unvisited list by smallest to largest distance
            unvisited = unvisited.OrderBy(star => distances[star]).ToList();

            //Getting the star with the smallest distance
            Star current = unvisited[0];

            //Remove current star from unvisited list
            unvisited.Remove(current);

            //Returns the path when the current star is the end star
            if (current == end) {
                //Construct shortest path
                while (previous.ContainsKey(current)) {
                    //Insert the star onto the final result
                    path.stars.Insert(0, current);

                    //Traverse from start to end
                    current = previous[current];
                }

                //Insert the source onto the final result
                path.stars.Insert(0, current);
                break;
            }

            //Checks whether it should avoid enemy territories, if so,
            //also checks whether the current stars faction is the enemy faction, if so,
            //set the distance to infinity
            if (avoidEnemies) {
                if(current.faction == gamemodeManager.enemyFaction.ToString()) {
                    distances[current] = float.MaxValue;
                }
            }

            //Looping through the star neighbours and where the neighbour is available at unvisited list
            for (int i = 0; i < current.starNeighbours.Count; i++) {
                Star neighbour = current.starNeighbours.Keys.ToList()[i];

                //Getting the distance between the two
                float length = current.starNeighbours[neighbour];

                //The distance from start star to this neighbour of current star
                float alt = distances[current] + length;

                //A shorter path to the neighbour has been found
                if (alt < distances[neighbour]) {
                    distances[neighbour] = alt;
                    previous[neighbour] = current;
                }
            }
        }

        //Checks if there are no possible routes between the two stars
        if(path.stars.Count <= 1) {
            errorMessage.gameObject.SetActive(true);
            errorMessage.StartLerp("Sorry there are no possible routes between these points");
        } else {
            //Calculates the total distance for the route, and displays all stars needed to travel to from start to end
            errorMessage.gameObject.SetActive(false);
            path.CalculateDistance();
            ColorRoutes(path);
            
            ChangePathText(path);
        }
        return path;
    }

    /// <summary>
    /// Adds all star names from a path to a string with an arrow and a line between them
    /// </summary>
    void ChangePathText(StarPath path) {
        string names = null;
        for (int i = 0; i < path.stars.Count; i++) {
            names += path.stars[i].ID;
            if (i < path.stars.Count - 1) {
                names += "  -->  \n \n";
            }
        }
        pathText.StartLerp(names);
        
        pathLengthText.StartLerp(KMaths.Truncate(path.length).ToString() + " LY's");
    }

    /// <summary>
    /// Loops through all connected routes in the path and colors them in yellow
    /// </summary>
    void ColorRoutes(StarPath path) {
        for (int i = 0; i < path.stars.Count - 1; i++) {
            //Loops through all of the routes
            for (int j = 0; j < allRoutes.Count; j++) {
                if (allRoutes[j].pointA == path.stars[i].transform && allRoutes[j].pointB == path.stars[i + 1].transform) {
                    allRoutes[j].GetComponent<Renderer>().material.color = Color.yellow;
                    pathRoutes.Add(allRoutes[j]);
                    break;
                }
                if (allRoutes[j].pointB == path.stars[i].transform && allRoutes[j].pointA == path.stars[i + 1].transform) {
                    allRoutes[j].GetComponent<Renderer>().material.color = Color.yellow;
                    pathRoutes.Add(allRoutes[j]);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Fades the route that has just been travelled to red using Lerping library
    /// </summary>
    public void ColorRouteRed(RouteMesh route) {
        Lerping routeLerp = route.transform.GetComponent<Lerping>();
        routeLerp.ObjectColor.startValues = Color.yellow;
        routeLerp.ObjectColor.endValues = Color.red;

        routeLerp.doesChangeColor = true;

        routeLerp.OnAndOff(0.1f);
    }

    /// <summary>
    /// Allows a UI toggle to toggle the bool on and off
    /// </summary>
    public void ToggleAvoidEnemies(bool toggle) {
        avoidEnemies = toggle;
    }
}
