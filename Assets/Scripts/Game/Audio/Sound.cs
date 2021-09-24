using UnityEngine.Audio;
using UnityEngine;

// implementam structura si setarile unui sunet care il vom avea in joc, acestea fiind configurabile din editor
[System.Serializable]
public class Sound
{
    // numele
    public string name;

    // sunetul
    public AudioClip clip;

    // volumul sunetului
    [Range(0f, 1f)]
    public float volume = 1;

    // "tonul" sunetului
    [Range(0.1f, 3f)]
    public float pitch = 1;

    public bool loop = false;

    // componenta de tip AudioSource care va fi adaugata ulterior
    [HideInInspector]
    public AudioSource source;
}
