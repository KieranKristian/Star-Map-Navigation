using System.Collections;
using TMPro;
using UnityEngine;

public class GamemodeManager : MonoBehaviour
{
    //Adjustable Game Constraints
    [Header("Adjustable Constraints")]
    [Range(300, 700)]
    public float fuelCapacity;

    public enum Faction {
        Bombers, SkullKids, Tingles, Beedles, Guardians
    }
    public Faction playerFaction;
    public Faction enemyFaction;

    //UI Elements
    [Header("UI")]
    public TextScrolling enemyFactionText;
    public TMP_Text fuelCapacityText;

    public Lerping startPanel;
    public Lerping gamePanels;
    [SerializeField] GameObject[] screens;
    [SerializeField] GameObject[] chosenStarIndicators;

    [Header("Object References")]
    [SerializeField] SpaceshipScript spaceShip;
    [SerializeField] StarSpawner spawner;
    [SerializeField] DijkstraPathfinding pathFinder;

    private void Start() {
        SetFactions(0);
    }

    private void Update() {
        fuelCapacityText.text = KMaths.Truncate(fuelCapacity - KMaths.Truncate(spaceShip.distanceTraveled)).ToString() + " LY's";
    }

    //Public Functions to be called by in game UI elements

    public void SetFuelCapacity(float fuel) {
        fuelCapacity = fuel;
    }

    /// <summary>
    /// Sets the player faction based on the UI dropdown selection
    /// Changes the enemy faction to a random faction that isnt the same as the player faction
    /// </summary>
    public void SetFactions(int index) {
        //Sets player faction
        playerFaction = (Faction)index;
        //Finds a random faction for the enemy, as long as it isn't the same as the players'
        int enemyIndex = Random.Range(0, 4);
        while (enemyIndex == index) {
            enemyIndex = Random.Range(0, 4);
        }
        enemyFaction = (Faction)enemyIndex;
        enemyFactionText.StartLerp(enemyFaction.ToString());
    }

    /// <summary>
    /// Lerps the start screen to move it off screen and brings the game panels down
    /// </summary>
    public void StartSimulation() {
        startPanel.OnAndOff(0.1f);
        gamePanels.OnAndOff(0.1f);
        //Spawns the faction areas in the game scene
        for (int i = 0; i < Random.Range(1, 3); i++) {
            spawner.SetFactions(playerFaction.ToString(), spawner.safetyIndicator, true);
        }
        for (int i = 0; i < Random.Range(2, 3); i++) {
            spawner.SetFactions(enemyFaction.ToString(), spawner.dangerIndicator, false);
        }
    }

    public void StartJourney() {
        StartCoroutine(StartStarJourney());
        foreach(GameObject go in chosenStarIndicators) {
            go.SetActive(false);
        }
    }

    /// <summary>
    /// Hides the pregame screen and brings up the Game UI
    /// </summary>
    IEnumerator StartStarJourney() {
        screens[0].GetComponent<Lerping>().OnAndOff(0.1f);
        yield return new WaitForSeconds(2);
        screens[0].SetActive(false);
        screens[1].SetActive(true);
        screens[1].GetComponent<Lerping>().OnAndOff(0.1f);

        spaceShip.StartJourney(pathFinder.path);
    }
}
