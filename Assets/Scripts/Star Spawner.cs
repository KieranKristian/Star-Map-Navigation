using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class StarSpawner : MonoBehaviour
{
    public enum GalaxyShape {
        Concave,
        DoubleHelix,
        Disc,
        Bowl,
        HollowSphere
    }
    public GalaxyShape galaxyShape;

    //Object prefabs
    [Header("Prefabs")]
    public Star starPrefab;
    public RouteMesh routeMesh;
    public GameObject safetyIndicator, dangerIndicator;

    //Adjustable values
    [Header("Changeable Values")]
    [Range(100, 150)]
    public int radius;
    [Range(0, 1888)]
    public int spawnNum;

    //List of stars and routes
    [Header("Lists")]
    private List<Star> stars = new List<Star>();
    private List<RouteMesh> routes = new List<RouteMesh>();

    private string[] allNames;

    //References to scene objects
    [Header("Scene Objects")]
    public DijkstraPathfinding pathFinder;
    public GamemodeManager gamemodeManager;
    public Transform routeMeshHandler;

    //UI Elements
    [Header("UI Elements")]
    public GameObject galaxyShapeUI;
    public GameObject startSimButton;
    public GameObject generateButton;

    bool allStarsSpawned;
    bool needToDoFactions;

    private void Start() {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "ZeldaCharacters.txt");
        allNames = File.ReadAllLines(filePath);
    }

    //Called by UI button, which will start the SpawnStars coroutine
    public void PopulateGalaxy() {
        StartCoroutine(SpawnStars());
    }

    /// <summary>
    /// Will populate a galaxy of stars
    /// </summary>
    IEnumerator SpawnStars() {
        //Turning off/on UI elements
        galaxyShapeUI.GetComponent<Lerping>().OnAndOff(0.1f);
        yield return new WaitForSeconds(1);
        galaxyShapeUI.SetActive(false);
        startSimButton.SetActive(true);
        startSimButton.GetComponent<Lerping>().OnAndOff(0.1f);

        //Spawning the stars
        for (int i = 0; i < spawnNum; i++) {
            yield return new WaitForSeconds(0.0000005f);
            Vector3 randomDistance = SwitchGalaxyShape();
            //Tries 100 times to spawn a star that isnt within 5 meters of another star
            for(int j = 0; j < 100; j++) {
                if (Physics.CheckBox(randomDistance, new Vector3(5, 5, 5))) {
                    randomDistance = SwitchGalaxyShape();
                } else {
                    //Spawning the star and setting its values
                    Star star = SpawnStar(randomDistance);
                    star.ID = allNames[i];
                    star.name = allNames[i];
                    stars.Add(star);
                    break;
                }
            }
        }
        allStarsSpawned = true;
        //Checks whether it still needs to set the factions 
        if (needToDoFactions) {
            //Setting ally factions
            for (int i = 0; i < Random.Range(1, 3); i++) {
                SetFactions(gamemodeManager.playerFaction.ToString(), safetyIndicator, true);
            }
            //Setting enemy factions
            for (int i = 0; i < Random.Range(2, 3); i++) {
                SetFactions(gamemodeManager.enemyFaction.ToString(), dangerIndicator, false);
            }
        }

        //Generate neighbours for each star
        for (int i = 0; i < stars.Count; i++) {
            GenerateNeighbours(stars[i]);
        }
        //Remove random neighbours from eachs star
        for (int i = 0; i < stars.Count; i++) {
            RemoveRandomNeighbours(stars[i]);
        }
        //Fill every stars routesToGenerate list
        for (int i = 0; i < stars.Count; i++) {
            stars[i].FillRouteList();
        }
        //Generate route meshes between each connected star
        for (int i = 0; i < stars.Count ; i++) {
            GenerateMeshes(stars[i]);
            yield return new WaitForSeconds(0.0000000005f);
        }
        //Set the pathfinders galaxy list
        pathFinder.SetGalaxyList(stars, routes);
        generateButton.SetActive(true);
    }

    /// <summary>
    /// Checks whether all the stars have spawned
    /// If so, it will find a random position within the galaxy and change all stars' faction within a random radius to the enemy faction
    /// If not, it will set needToDoFactions to true so that the script can call the function itself after all of the stars have been spawned
    /// </summary>
    /// <param name="faction">The faction name that the stars need to be set to</param>
    /// <param name="visualiser">The in game prefab that shows the area of a certain territory</param>
    /// <param name="ally">Bool determining whether the factions are allies</param>
    public void SetFactions(string faction, GameObject visualiser, bool ally) {
        if (allStarsSpawned) {
            //Initialising variables such as position and scale
            int randomSize = Random.Range(25, 40);
            Vector3 randomPos = SwitchGalaxyShape();
            //Instantiates the faction indicator
            GameObject factionSphere = Instantiate(visualiser, randomPos, Quaternion.identity);
            factionSphere.transform.localScale = new Vector3(randomSize, randomSize, randomSize) * 2;
            
            //Checks for all stars within the random radius
            Collider[] stars = Physics.OverlapSphere(randomPos, randomSize, 1 << 3);
            foreach (Collider c in stars) {
                c.GetComponent<Star>().faction = faction;
                //Makes the star start changing color depending on whether its an ally or enemy
                Lerping colorLerp = c.GetComponent<Lerping>();
                colorLerp.ObjectColor.startValues = ally ? Color.green : Color.red;
                colorLerp.doesChangeColor = true;
                colorLerp.shouldLerp = true;
            }
        } else {
            needToDoFactions = true;
        }
    }

    /// <summary>
    /// Instantiates the star object and sets it to a random color
    /// </summary>
    /// <returns>Returns as a Star class</returns>
    Star SpawnStar(Vector3 spawnPos) {
        Star star = Instantiate(starPrefab, (transform.position + spawnPos), transform.rotation, transform) as Star;
        star.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        return star;
    }

    /// <summary>
    /// Adds all Stars and their distances within a radius depending on the galaxy shape to the dictionary of a Star that is passed through
    /// </summary>
    void GenerateNeighbours(Star star) {
        Collider[] neighbours = Physics.OverlapSphere(star.transform.position, CheckRadius(), 1 << 3);

        foreach(Collider col in neighbours) {
            Star neighbour = col.GetComponent<Star>();
            //Calculates and sets the distance to the neighbour star
            float distance = Vector3.Distance(star.transform.position, neighbour.transform.position);
            //If the current star or neighbour star is an ally, the distance is halved
            distance = star.faction == gamemodeManager.playerFaction.ToString() || neighbour.faction == gamemodeManager.playerFaction.ToString() ? distance / 2 : distance;
            //Add the neighbour and its distance to the star neighbours dictionary
            star.starNeighbours.Add(neighbour, distance);
        }
        star.starNeighbours.Remove(star); //Removes itself from its dictionary of neighbours
    }

    /// <summary>
    /// Removes a random amount of neighbours from a star that is passed through
    /// </summary>
    void RemoveRandomNeighbours(Star star) {
        List<Star> neighbours = star.starNeighbours.Keys.ToList();
        for (int i = 0; i < Random.Range(0, star.starNeighbours.Count - 1); i++) {
            Star neighbour = neighbours[Random.Range(0, star.starNeighbours.Count - 1)];
            neighbour.starNeighbours.Remove(star);
            star.starNeighbours.Remove(neighbour);
        }
    }

    /// <summary>
    /// Generates a route mesh between the passed through star and all of its neighbours, also removes that Star from the neighbours list
    /// </summary>
    void GenerateMeshes(Star star) {
        List<Star> neighbours = star.routesToGenerate;
        for (int i = 0; i < neighbours.Count; i++) { //Looping through each neighbour
            if (neighbours[i].routesToGenerate.Count > 0) { //Checking whether current neighbour has any routes to generate
                for (int j = 0; j < neighbours.Count; j++) { 
                    //Generating the mesh
                    RouteMesh route = Instantiate<RouteMesh>(routeMesh, routeMeshHandler) as RouteMesh;
                    route.pointA = star.transform;
                    route.pointB = neighbours[j].transform;
                    route.Generate();
                    routes.Add(route);
                    //Removing the star from the neighbours list so it doesn't make two routes over each other
                    neighbours[j].routesToGenerate.Remove(star);
                }
                star.routesToGenerate.Clear();
            } 
        }
    }

    /// <summary>
    /// Switches the galaxy shape enum
    /// </summary>
    /// <returns>A vector 3 position in the desired shape</returns>
    public Vector3 SwitchGalaxyShape() {
        return galaxyShape switch {
            GalaxyShape.Concave => GalaxyShapes.RandomDistanceConcave(radius),
            GalaxyShape.DoubleHelix => GalaxyShapes.RandomDistanceHelix(radius, transform),
            GalaxyShape.Disc => GalaxyShapes.RandomDistanceDisc(radius, transform),
            GalaxyShape.Bowl => GalaxyShapes.RandomDistanceBowl(radius),
            GalaxyShape.HollowSphere => GalaxyShapes.RandomDistanceHollowSphere(radius),
            _ => Vector3.zero
        };
    }

    /// <summary>
    /// Returns a float for a radius for each star to check for neighbours depending on the galaxy shape
    /// </summary>
    public float CheckRadius() {
        return galaxyShape switch {
            GalaxyShape.Concave => 12,
            GalaxyShape.DoubleHelix => 15,
            GalaxyShape.Disc => 8,
            GalaxyShape.Bowl => 9,
            GalaxyShape.HollowSphere => 12,
            _ => 0
        };
    }

    public void DropDownMenuChoice(int choice) {
        galaxyShape = (GalaxyShape)choice;
    }
}
