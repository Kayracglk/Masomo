using System.Collections;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioClip[] soundClips; 
    public float minDelay = 1f;    
    public float maxDelay = 5f;    

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomSound());
    }

    IEnumerator PlayRandomSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            if (soundClips.Length > 0)
            {
                AudioClip randomClip = soundClips[Random.Range(0, soundClips.Length)];
                audioSource.clip = randomClip;
                audioSource.Play();
            }
        }
    }
}
