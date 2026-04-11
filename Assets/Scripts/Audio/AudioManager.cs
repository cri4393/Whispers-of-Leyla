using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    [SerializeField] AudioMixer mixer;
    public bool isFadingAudio = false;
    public const string musicKey = "musicVolume";
    public const string sfxKey = "sfxVolume";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.loop = s.loop;
        }

        LoadVolume();
    }
    private void Start()
    {
        
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Play();
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.outputAudioMixerGroup = s.AudioMixerGroup;
    }
    public IEnumerator StartFade(string sound, float duration, float targetVolume)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        float currentTime = 0;
        if(s.source.isPlaying)
        {
            yield break;
        }
        s.source.outputAudioMixerGroup = s.AudioMixerGroup;
        s.source.volume = 0;
        float start = s.source.volume;
        s.source.Play();
        isFadingAudio = true;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            s.source.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            if (s.source.volume == targetVolume)
            {
                Debug.Log("volume = target!");
                isFadingAudio = false;
            }
            yield return null;
        }
        //yield break;
    }
    public IEnumerator StartFadeOut(string sound, float duration, float targetVolume)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        float currentTime = 0;
        s.source.outputAudioMixerGroup = s.AudioMixerGroup;
        s.source.volume = 1;
        float start = s.source.volume;
        isFadingAudio = true;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            s.source.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            Debug.Log(s.source.volume);
            if (s.source.volume == targetVolume)
            {
                Debug.Log("volume = target!");
                s.source.Stop();
                isFadingAudio = false;
            }
            yield return null;
        }
        //yield break;
    }
    public void StopPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }
    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.PlayOneShot(s.clip);
    }
    public void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(musicKey, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(sfxKey, 1f);

        // mixer.SetFloat(VolumeSettings.mixerMusic, Mathf.Log10(musicVolume) * 20);
        // mixer.SetFloat(VolumeSettings.mixerSfx, Mathf.Log10(sfxVolume) * 20);
    }
}
