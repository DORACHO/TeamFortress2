using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJSFX_Walk : MonoBehaviour
{
    public AudioClip WalkSFX;
    AudioSource audiosource;
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayWalkSFX()
    {
        audiosource.PlayOneShot(WalkSFX);
    }
}
