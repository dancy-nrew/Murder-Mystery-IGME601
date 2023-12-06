using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<AudioClip> sfxClips;
    public AudioClip backgroundMusic;
    private bool _isMusicPlaying;

    private GameObject musicSource;
    private Dictionary<string, GameObject> sfxLibrary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
        _isMusicPlaying = false;
        sfxLibrary = new Dictionary<string, GameObject>();
        EnableMusic();
        for (int i = 0; i < sfxClips.Count; i++)
        {
            EnableSFXForScene(i);
        }
    }

    public void PlaySFX(string key)
    {
        AudioSource audioSource = sfxLibrary[key].GetComponent<AudioSource>();
        audioSource.Play();
    }

    private void EnableSFXForScene(int index) {
        string key = "SFXAudioSource" + index.ToString();
        GameObject go = new GameObject(key);
        sfxLibrary.Add(key, go);
        go.transform.parent = transform;
        AudioSource sfxSource = go.AddComponent<AudioSource>();
        sfxSource.clip = sfxClips[index];
    }

    private void EnableMusic()
    {
        musicSource = new GameObject("MusicSource");
        musicSource.transform.parent = transform;
        AudioSource audio = musicSource.AddComponent<AudioSource>();
        audio.clip = backgroundMusic;
        audio.loop = true;
    }

    public void StartBackgroundMusic()
    {
        AudioSource musicAudio = musicSource.GetComponent<AudioSource>();
        musicAudio.Play();
        _isMusicPlaying = true;
    }

    public void StopBackgroundMusic()
    {
        AudioSource musicAudio = musicSource.GetComponent<AudioSource>();
        musicAudio.Stop();
        _isMusicPlaying = false;
    }

    public void ToggleMusic()
    {
        if (_isMusicPlaying)
        {
            StopBackgroundMusic();
        } else
        {
            StartBackgroundMusic();
        }
    }
}
