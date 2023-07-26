using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJSFX_SentryGun : MonoBehaviour
{
    public static EJSFX_SentryGun instance;
    public AudioClip sentryFireSFX;
    AudioSource audiosource;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySentryFireSFX()
    {
        audiosource.PlayOneShot(sentryFireSFX);
    }
}
