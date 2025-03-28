using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _sounds;
    [SerializeField] private AudioSource _sfxAudioSource;
    public static SoundManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }   
        else
        {
            Destroy(gameObject);
        } 
    }

    public void PlaySound(string soundName)
    {
        foreach (AudioClip audioClip in _sounds)
        {
            if (audioClip.name == soundName)
            {
                _sfxAudioSource.PlayOneShot(audioClip);
            }
        }
    }
}
