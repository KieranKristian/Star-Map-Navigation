using System.Collections;
using UnityEngine;
using TMPro;

public class SidePanelManager : MonoBehaviour
{
    public enum SidePanelType {
        Left, right
    }
    public SidePanelType type;

    public Lerping sidePanel;
    public TMP_Text arrow;

    bool panelActive;

    string a, b;

    //Changes the arrow based on the type of panel it is
    private void Start() {
        a = type == SidePanelType.Left ? ">" : "<";
        b = a == ">" ? "<" : ">";
    }

    /// <summary>
    /// Public function to be called by UI buttons
    /// </summary>
    public void MovePanel() {
        StartCoroutine(PanelMove());
    }

    /// <summary>
    /// Starts the lerp and changes the arrow on the button to face the other way
    /// </summary>
    IEnumerator PanelMove() {
        panelActive = !panelActive;
        sidePanel.OnAndOff(0.1f);
        yield return new WaitForSeconds(0.9f);

        arrow.text = panelActive ? b : a;
    }
}
