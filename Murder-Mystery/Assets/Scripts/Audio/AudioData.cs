using System;
using UnityEngine;


[Serializable]
public struct NamedClip
{
    public string name;
    public AudioClip clip;
}


[CreateAssetMenu]
public class AudioData : ScriptableObject
{    
    //Data scriptable object to hold the data for the AudioManager

    [SerializeField]
    public NamedClip[] clipMap;

    [SerializeField]
    public NamedClip[] songs;
}
