using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set;}

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one AudioManager in the scene");
        else instance = this;
        
    }

    public void PlayOneShot(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }
}
