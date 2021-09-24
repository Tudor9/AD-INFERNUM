using UnityEngine.Audio;
using System;
using UnityEngine;

// aceasta componenta se ocupa de initializarea sunetelor ce vor fi folosite pe parcursul jocului si urmareste
// un pattern de tip Singleton
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixerGroup audioMixer;     // referinta la grupul audio in care vor fi setate sunetele, setat in editor
    
    [SerializeField]
    private Sound[] sounds;
    
    // initializam sunetele setate in editor
    private void Awake()
    {
        foreach(var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = audioMixer;
            s.source.loop = s.loop;
        }
    }

    // dam "play" la un sunet in functie de numele primit
    public void Play(string audioName)
    {
        var s = Array.Find(sounds, sound => sound.name == audioName);
        
        if (s == null)
        {
            Debug.LogWarning("Sound " + s.name + " not found! Typo?");
            return;
        }
        s.source.Play();
    }
}
