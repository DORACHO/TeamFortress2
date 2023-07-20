using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_ScatterSound : MonoBehaviour
{
    public  AudioSource gunAudioSource;
    public AudioSource footAudioSource;

    public AudioClip footSound;
    public AudioClip scatter_Shoot;
    public AudioClip scatter_Reload;
    public AudioClip pistol_Shoot;
    public AudioClip pistol_Reload;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootSound()
    {
        footAudioSource.Play();
    }

    public void StopFootSound()
    {
        footAudioSource.Stop();
    }

    public void ScatterShoot()
    {
        gunAudioSource.clip = scatter_Shoot;
        gunAudioSource.loop = false;
        gunAudioSource.Play();
    }

    public void ScatterReload()
    {
        gunAudioSource.clip = scatter_Reload;
        gunAudioSource.loop = true;
        gunAudioSource.Play();
    }
    public void PistolShoot()
    {
        gunAudioSource.clip = pistol_Shoot;
        gunAudioSource.loop = false;
        gunAudioSource.Play();
    }

    public void PistolReload()
    {
        gunAudioSource.clip = pistol_Reload;
        gunAudioSource.loop = true;
        gunAudioSource.Play();
    }
}
