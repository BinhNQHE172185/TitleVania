using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip coin;
    public AudioClip heart;
    public AudioClip jump;
    public AudioClip dash;
    public AudioClip run;
    public AudioClip hurt;
    public AudioClip die;
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
