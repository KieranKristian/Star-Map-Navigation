using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;

public class SpaceshipScript : MonoBehaviour {
    //Class References
    [Header("Object References")]
    [SerializeField] 
    Lerping lerpScript;
    [SerializeField]
    Lerping soundLerp;

    [SerializeField] 
    TextScrolling currentStarText;

    [SerializeField] 
    TMP_Text distanceTraveledText;

    [SerializeField] 
    DijkstraPathfinding pathFinder;

    [HideInInspector]
    public float distanceTraveled;

    List<Star> journey = new List<Star>();

    int starIndex;
    bool lerping;

    public void StartJourney(StarPath starPath) {
        transform.SetParent(null);
        starIndex = 0;
        journey = starPath.stars;
        //Making the spaceship move to the starting point
        lerpScript.Position.startValues = transform.position;
        lerpScript.Position.endValues = journey[starIndex].transform.position;
        transform.LookAt(journey[starIndex].transform.position);
        lerpScript.doesMove = true;
        lerpScript.OnAndOff(0.1f);
        soundLerp.OnAndOff(0.1f);

        currentStarText.StartLerp(journey[starIndex].ID);
    }

    /// <summary>
    /// Moves the spaceship to the next star and changes the current star text
    /// </summary>
    public void JumpStar() {
        lerpScript.doesLookAt = true;
        if (starIndex < journey.Count - 1) {
            if (!lerping) {
                MoveShip();
                starIndex++;
                lerping = true;
                currentStarText.StartLerp(journey[starIndex].ID);
                StartCoroutine(EndOfLerp());
            }
        }
    }

    /// <summary>
    /// Performs the lerp to make the ship move and look towards the next star, also lerps the distance travelled
    /// </summary>
    void MoveShip() {
        Vector3 startPos = journey[starIndex].transform.position;
        Vector3 endPos = journey[starIndex + 1].transform.position;
        Vector3 lookDir = endPos - startPos;

        //Setting movement lerp values
        lerpScript.Position.startValues = startPos;
        lerpScript.Position.endValues = endPos;
        
        //Setting rotation lerp values
        lerpScript.Rotation.startValues = transform.forward;
        lerpScript.Rotation.endValues = lookDir;

        //Setting distance lerp values
        lerpScript.Floats.startValue = distanceTraveled;
        lerpScript.Floats.endValue = distanceTraveled + journey[starIndex].starNeighbours[journey[starIndex + 1]];

        lerpScript.OnAndOff(0.1f);
        soundLerp.duration = 1.25f;
        soundLerp.OnAndOff(1.5f);

        pathFinder.ColorRouteRed(pathFinder.pathRoutes[starIndex]);
    }

    /// <summary>
    /// Resets lerping to false after the spaceship finishes moving, this is so you cant make it skip through stars
    /// </summary>
    /// <returns></returns>
    IEnumerator EndOfLerp() {
        yield return new WaitForSeconds(2.5f);
        lerping = false;
    }

    private void Update() {
        distanceTraveled = lerpScript.floatLerp;
        distanceTraveledText.text = KMaths.Truncate(distanceTraveled).ToString() + "LY's";
    }
}
