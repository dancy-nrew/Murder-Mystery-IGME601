using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip backgroundMusic;
    public AudioData audioData;
    private Dictionary<string, AudioClip> sfxLibrary;
    private Dictionary<string, AudioClip> musicLibrary;

    private bool _isMusicPlaying;

    private GameObject musicSource;

    private Dictionary<string, GameObject> clipChildren;

    private void Awake()
    {
        //Singleton stuff
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(Instance);


        // Data Setup
        _isMusicPlaying = false;

        // Turn the lists in the AudioData into dictionaries for fast lookup
        sfxLibrary = new Dictionary<string, AudioClip>();
        foreach(NamedClip clip in audioData.clipMap)
        {
            sfxLibrary[clip.name] = clip.clip;
        }

        musicLibrary = new Dictionary<string, AudioClip>();
        foreach(NamedClip clip in audioData.songs)
        {
            musicLibrary[clip.name] = clip.clip;
        }

        // Create a map of your children for easy access
        clipChildren = new Dictionary<string, GameObject>();
        EnableMusic();
    }

    public void PlaySFX(string key)
    {
        // Play a sound effect. The key should be equal to the key
        // in the named clip in the AudioData object
        GameObject child;

        //Lazy Instantiation
        if (!clipChildren.TryGetValue(key, out child))
        {
            child = MakeSFXSource(key);
        }

        // Have the corresponding audio source play the sound effect
        AudioSource audioSource = child.GetComponent<AudioSource>();
        audioSource.Play();
    }

    private GameObject MakeSFXSource(string key)
    {
        //Create the necessary audio source to play the sound
        GameObject go = new GameObject(key);
        clipChildren.Add(key, go);
        go.transform.parent = transform;
        AudioSource sfxSource = go.AddComponent<AudioSource>();
        sfxSource.clip = sfxLibrary[key];
        return go;
    }


    private void EnableMusic()
    {
        // Add the music as a separate source, and make it looping
        // This doesn't specify which song is playing, that is 
        // a separate call
        musicSource = new GameObject("MusicSource");
        musicSource.transform.parent = transform;
        AudioSource audio = musicSource.AddComponent<AudioSource>();
        audio.loop = true;
    }

    public void StartBackgroundMusic()
    {
        // Press play
        AudioSource musicAudio = musicSource.GetComponent<AudioSource>();
        musicAudio.Play();
        _isMusicPlaying = true;
    }

    public void StopBackgroundMusic()
    {
        // Press stop
        AudioSource musicAudio = musicSource.GetComponent<AudioSource>();
        musicAudio.Stop();
        _isMusicPlaying = false;
    }

    private void SetSong(string key)
    {
        AudioSource audio = musicSource.GetComponent<AudioSource>();
        audio.clip = musicLibrary[key];
    }

    public void ChangeSong(string key)
    {
        // Changes the song playing in the background. The key should be equal to the named song in the AudioData
        // This will likely be useful when loading into and out of the card battle, but other
        // use cases may arise
        StopBackgroundMusic();
        SetSong(key);
        StartBackgroundMusic();
    }

    public void ToggleMusic()
    {
        // Controls for flipping between play and stop, should we need to. Likely useful for pause menus
        if (_isMusicPlaying)
        {
            StopBackgroundMusic();
        } else
        {
            StartBackgroundMusic();
        }
    }
}
