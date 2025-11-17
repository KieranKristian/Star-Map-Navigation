using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera[] cameras;
    [SerializeField] GameObject panel;

    public void ChangeCamera(int index) {
        StartCoroutine(FadeToBlack(index));
    }

    IEnumerator FadeToBlack(int index) {
        panel.GetComponent<Lerping>().OnAndOff(2.1f);

        yield return new WaitForSeconds(2.1f);

        foreach (Camera cam in cameras) {
            cam.gameObject.SetActive(false);
        }

        cameras[index].gameObject.SetActive(true);
    }
}
