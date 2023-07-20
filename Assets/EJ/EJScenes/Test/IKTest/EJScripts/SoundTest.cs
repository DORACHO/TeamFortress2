using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip soundFire;
    public AudioClip soundLoad;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {            
            audioSource.PlayOneShot(soundFire);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            
            audioSource.PlayOneShot(soundLoad);
        }
    }

}
