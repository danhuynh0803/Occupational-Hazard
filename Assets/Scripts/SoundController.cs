using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum BG
{
    // TODO
};

public enum SFX
{
    Explode,
    Yes1,
    Yes2,
    Yes3,
    Yes4,
    Yes5,
    Yes6,
    Yes7,
    GunShot,
    AgentDeath1,
    AgentDeath2,
    AgentDeath3
};

public enum GUISFX
{
    Load,
    Click
};

public class SoundController : MonoBehaviour
{
    public AudioClip[] clips;    // Make sure clips are ordered in the same as the enum
    public static AudioSource[] sources;

    // TODO: Have a menu option to control sound levels
    public static float masterVolume = 1.0f;
    public static float bgmVolume = 1.0f;
    public static float sfxVolume = 1.0f;

    void Start()
    {
        //Debug.Log("SoundController Start()");
        // Set up sfx audio
        sources = new AudioSource[clips.Length];
        //Debug.Log("Clips Length=" + clips.Length);
        for (int i = 0; i < clips.Length; ++i)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;
            sources[i] = child.AddComponent<AudioSource>() as AudioSource;
            sources[i].clip = clips[i];
        }
    }
    
    public static void Play(int soundIndex)
    {
        //Debug.Log("SoundIndex=" + soundIndex + "\tSizeof Sources=" + sources.Length);
        sources[soundIndex].volume = Mathf.Min(sfxVolume, masterVolume);  // Play at specified volume
        sources[soundIndex].Play();
    }

    // Play desired clip at specified volume 
    // 1.0f for max volume    
    public static void Play(int soundIndex, float volumeLevel)
    {
        volumeLevel = Mathf.Clamp(volumeLevel, 0.0f, 1.0f);
        sources[soundIndex].volume = volumeLevel * Mathf.Min(sfxVolume, masterVolume);
        sources[soundIndex].Play();
    }

    public static void Play(int soundIndex, Transform transform, float volumeLevel)
    {
        volumeLevel = Mathf.Clamp(volumeLevel, 0.0f, 1.0f);
        sources[soundIndex].volume = volumeLevel * Mathf.Min(sfxVolume, masterVolume);
        AudioSource.PlayClipAtPoint(sources[soundIndex].clip, transform.position);
    }

    public static void Play(int soundIndex, Vector3 position, float volumeLevel)
    {
        volumeLevel = Mathf.Clamp(volumeLevel, 0.0f, 1.0f);
        sources[soundIndex].volume = volumeLevel * Mathf.Min(sfxVolume, masterVolume);
        AudioSource.PlayClipAtPoint(sources[soundIndex].clip, position);
    }

    public static void Play(SFX sfx, float volumeLevel)
    {
        volumeLevel = Mathf.Clamp(volumeLevel, 0.0f, 1.0f);
        sources[(int)sfx].volume = volumeLevel * Mathf.Min(sfxVolume, masterVolume);
        sources[(int)sfx].Play();
    }

    public static void Play(SFX sfx, Transform transform, float volumeLevel)
    {
        volumeLevel = Mathf.Clamp(volumeLevel, 0.0f, 1.0f);
        sources[(int)sfx].volume = volumeLevel * Mathf.Min(sfxVolume, masterVolume);
        AudioSource.PlayClipAtPoint(sources[(int)sfx].clip, transform.position);
    }

    public static void Play(SFX sfx, Vector3 position, float volumeLevel)
    {
        volumeLevel = Mathf.Clamp(volumeLevel, 0.0f, 1.0f);
        sources[(int)sfx].volume = volumeLevel * Mathf.Min(sfxVolume, masterVolume);
        AudioSource.PlayClipAtPoint(sources[(int)sfx].clip, position);
    }

}