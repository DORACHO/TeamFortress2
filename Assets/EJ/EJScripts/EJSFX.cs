using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJSFX : MonoBehaviour
{
    public static EJSFX instance;

    public AudioClip FireSFX;
    public AudioClip LoadSFX;
    public AudioClip TeleportSFX;
    public AudioClip callMedicSFX;
    public AudioClip killedSFX;

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

    public void PlayFireSFX()
    {
        audiosource.PlayOneShot(FireSFX);
    }
    public void PlayLoadSFX()
    {
        audiosource.PlayOneShot(LoadSFX);
    }
    public void PlayTeleportSFX()
    {
        audiosource.PlayOneShot(TeleportSFX);
    }
    public void CallMedicSFX()
    {
        audiosource.PlayOneShot(callMedicSFX);
    }
    public void PlayKilledSFX()
    {
        audiosource.PlayOneShot(killedSFX);
    }
}
