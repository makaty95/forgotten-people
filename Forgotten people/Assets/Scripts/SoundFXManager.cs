using System;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    [SerializeField] private AudioSource SoundFXObject;

    void Awake()
    {
        if(Instance == null) Instance = this;
    }

    public void PlaySoundFXClip_1Time(AudioClip audioClip, Transform transform, float volume)
    {
        // spawn audio
        AudioSource audioSource = Instantiate(SoundFXObject, transform.position, Quaternion.identity);

        // set clip
        audioSource.clip = audioClip;
        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);

    }

    public AudioSource PlaySoundFXClipInLoop(AudioClip audioClip, Transform transform, float volume, bool autoPlay = true)
    {
        // spawn audio
        AudioSource audioSource = Instantiate(SoundFXObject, transform.position, Quaternion.identity);

        // set clip
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = true;

        if(autoPlay) audioSource.Play();

        return audioSource;

    }

}