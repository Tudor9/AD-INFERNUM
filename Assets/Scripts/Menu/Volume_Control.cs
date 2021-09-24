using UnityEngine;
using UnityEngine.Audio;

// componenta pentru a modifica volumul jocului
public class Volume_Control : MonoBehaviour
{
    // mixer-ul al carui audio ii modificam volumul
    [SerializeField]
    private AudioMixer mixer;

    // metoda pentru a modifica volumul acestui mixer
    private void SetVolume(float vol)
    {
        mixer.SetFloat("Menu_background", Mathf.Log10(vol)*20);
    }
}
