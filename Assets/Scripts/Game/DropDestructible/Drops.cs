using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// aceasta componenta se ocupa de logica de "dropare" a anumitor iteme
public class Drops : MonoBehaviour
{
    private AudioManager audioManager;  // referinta la componenta audio

    [SerializeField]
    private Consumable[] consumablePrefab;  // referinta la tipurile de obiecte ce pot "dropa"
    
    private Consumable consumable;  // obiectul care va fi "dropat" va fi instantiat in aceasta variabila

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Drop()
    {
        // sansa de 33% sa dropeze un obiect aleatoriu din lista
        if (Random.Range(1, 3) == 1)
        {
            consumable = Instantiate(consumablePrefab[Random.Range(0, consumablePrefab.Length)]);
            consumable.item.quantity = Random.Range(1, 5);
            consumable.transform.position = this.transform.position;
        }
    }

    // event folosit in editor
    private void BarrelDesotryed()
    {
        audioManager.Play("BarrelDestroyed");
    }
}
