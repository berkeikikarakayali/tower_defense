using UnityEngine;

[System.Serializable] //to see the list in inspector
public class Sound
{
    public string name;
    public AudioClip clip; //mp3 or wav
    [Range(0f,1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
public float pitch = 1f;
    public bool loop; //music true, or false
    public enum AudioType { SFX, Music } //to determine is it a sfx or music
    public AudioType type; //to change it in the settings

    [HideInInspector]
    public AudioSource source; // for unity req
}
