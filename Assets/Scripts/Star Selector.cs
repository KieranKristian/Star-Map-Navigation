using UnityEngine;

public class StarSelector : MonoBehaviour
{
    //Object References
    [Header("Object References")]
    [SerializeField] Camera freeCam;
    [SerializeField] DijkstraPathfinding pathFinder;

    [SerializeField] AudioSource selectNoise;
    public enum starToPick {
        None,
        Start,
        End
    }
    [Header("Start and End Stars")]
    public starToPick starToChoose;
    //UI Elements
    [SerializeField] TextScrolling startStarText, endStarText;
    [SerializeField] GameObject startStarIndicator, endStarIndicator;

    /// <summary>
    /// Switches the starToChoose, and sets either the start or end star on the path finder to the star that is returned on the SelectStar function
    /// </summary>
    void OnClick() {
        switch (starToChoose) {
            case starToPick.Start:
                Star startStar = SelectStar();
                if (startStar != null) {
                    pathFinder.startStar = startStar;
                    startStarText.StartLerp(startStar.ID);
                    //Moving chosen star indicator
                    startStarIndicator.GetComponent<MeshRenderer>().enabled = true;
                    startStarIndicator.transform.position = startStar.transform.position;
                }
                break;

            case starToPick.End:
                Star endStar = SelectStar();
                if (endStar != null) {
                    pathFinder.endStar = endStar;
                    endStarText.StartLerp(endStar.ID);
                    //Moving chosen star indicator
                    endStarIndicator.GetComponent<MeshRenderer>().enabled = true;
                    endStarIndicator.transform.position = endStar.transform.position;
                }
                break;
        }
    }

    /// <summary>
    /// Casts a raycast from the mouse position and sees if it hits a star
    /// </summary>
    /// <returns> A star that is hit by the raycast</returns>
    Star SelectStar() {
        Ray mousePoint = freeCam.ScreenPointToRay(Input.mousePosition);
        starToChoose = starToPick.None;
        if (Physics.Raycast(mousePoint, out RaycastHit starHit)) {
            selectNoise.Play();
            return starHit.transform.GetComponent<Star>();
        } else {
            return null;
        }
    }

    public void StartOrEnd(int star) {
        starToChoose = (starToPick)star;
    }

    void OnEscape() {
        Application.Quit();
    }
}
