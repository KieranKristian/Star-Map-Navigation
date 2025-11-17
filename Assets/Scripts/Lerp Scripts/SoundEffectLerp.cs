using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLerp : MonoBehaviour
{
    [SerializeField] Lerping lerpScript;
    [SerializeField] AudioSource source;

    void Update()
    {
        source.volume = lerpScript.floatLerp;
    }
}
