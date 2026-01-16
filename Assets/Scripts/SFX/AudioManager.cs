using UnityEngine;
using System; //for Array.Find

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager; //singleton
    public Sound[] sounds;
    private Sound currentMusic; //to remember which music is playing

    void Awake()
    {
        if(audioManager == null)
        {
            audioManager = this;
        } else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.pitch = s.pitch;
        }
    }

    void Start()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetMusicVol(musicVol);
        SetSFXVol(sfxVol);
        if (AudioManager.audioManager != null)
        {
        AudioManager.audioManager.PlayMusic("MenuTheme"); 
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name); //find sound

        if (s == null)
        {
            Debug.Log("Sound not found: "+ name);
        }
        s.source.pitch = UnityEngine.Random.Range(0.9f, 1.1f); //to avoid freq conflicts
        s.source.PlayOneShot(s.clip);
    }


    public void PlayMusic(string name)
    {
        if (currentMusic != null && currentMusic.name == name) //if requested music is already playing do nothing
        {
            return;
        }
        
        if (currentMusic != null) //if another music is playing stop it
        {
            currentMusic.source.Stop();
        }

        //find music
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        if (s == null)
        {
            Debug.Log("Music not found: "+ name);
        }

        s.source.Play();
        currentMusic = s;
    }

    public void SetMusicVol(float vol)
    {
        PlayerPrefs.SetFloat("MusicVolume", vol);
        foreach (Sound s in sounds)
        {
            if (s.type == Sound.AudioType.Music) //only change music volume
            {
                s.source.volume = vol;
            }
        }
    }

    public void SetSFXVol(float vol)
    {
        PlayerPrefs.SetFloat("SFXVolume", vol);
        foreach (Sound s in sounds)
        {
            if (s.type == Sound.AudioType.SFX) //only change sfx volume
            {
                s.source.volume = vol;
            }
        }
    }
}
