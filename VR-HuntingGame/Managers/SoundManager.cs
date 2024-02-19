using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClips[] clips;
 
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySoundEffects(AudioSource audioSource, AudioTypes type)
    {
        AudioClips audio = Array.Find(clips, cl => cl.audioType == type);
        print(audio.audioClip.name);
        audioSource.PlayOneShot(audio.audioClip);
    }
}

[Serializable]
public enum AudioTypes
{
    BearAngry,
    BirdFly,
    BirdWings,
    Deer1,
    Deer2,
    BoarIdle,
    BoarDead,
    SingleBear
}
[Serializable]
public struct AudioClips
{
    public AudioTypes audioType;
    public AudioClip audioClip;
}
