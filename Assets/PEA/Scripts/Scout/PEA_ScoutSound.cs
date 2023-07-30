using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_ScoutSound : MonoBehaviour
{
    private  AudioSource audioSource;

    public AudioClip footSound;
    public AudioClip scatter_Shoot;
    public AudioClip scatter_Reload;
    public AudioClip pistol_Shoot;
    public AudioClip pistol_Reload;
    public AudioClip scout_Die;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootSound()
    {
        audioSource.Play();
        audioSource.loop = true;
    }

    public void StopFootSound()
    {
        // Stop()으로 멈추면 부자연스럽게 음원이 끊김
        audioSource.loop = false;
    }

    public void ScatterShoot()
    {
        audioSource.PlayOneShot(scatter_Shoot);
    }

    public void ScatterReload()
    {
        audioSource.PlayOneShot(scatter_Reload);
    }
    public void PistolShoot()
    {
        audioSource.PlayOneShot(pistol_Shoot);
    }

    public void PistolReload()
    {
        audioSource.PlayOneShot(pistol_Reload);
    }

    public void Die()
    {
        audioSource.PlayOneShot(scout_Die);
    }
}
