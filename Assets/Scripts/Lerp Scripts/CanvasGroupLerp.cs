using UnityEngine;
[RequireComponent (typeof(CanvasGroup))]
[RequireComponent (typeof(Lerping))]

public class CanvasGroupLerp : MonoBehaviour
{
    [SerializeField] Lerping lerpScript;
    [SerializeField] CanvasGroup canvas;

    void Update()
    {
        canvas.alpha = lerpScript.floatLerp;
    }
}
