using UnityEngine;

[System.Serializable]
public class Audio 
{
    public string name;
    public string description;
    public AudioClip clip;
    public AudioClip[] randomSampleClips;
    
    public AudioSource audioSource;

    public AudioClip GetAudioClip()
    {
        return randomSampleClips.Length == 0 ? clip :
            randomSampleClips[Random.Range(0, randomSampleClips.Length)];
    }
}

