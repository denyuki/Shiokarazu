using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip clearSound;
    public AudioClip gameOverSound;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClearSound()
    {
        audioSource.PlayOneShot(this.clearSound);
    }

    public void PlayGameOverSound()
    {
        audioSource.PlayOneShot(this.gameOverSound);
    }
}
