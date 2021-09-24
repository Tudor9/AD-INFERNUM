using UnityEngine;

// componenta ce instantiaza un anumit numar de obiecte, la un interval predefinit sau doar o singura data
// la pozitia in care obiectul cu aceasta componenta se afla
public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToSpawn;

    [SerializeField]
    private float repeatInterval;
    
    [SerializeField]
    private int ammountToSpawn;

    private int ammountSpawned;

    private void Start()
    {
        ammountSpawned = 0;
        if (repeatInterval > 0 )
        {  
            InvokeRepeating("SpawnObject", 0.0f, repeatInterval);
        }
    }

    public GameObject SpawnObject()
    {
        if (ammountSpawned == ammountToSpawn)
        {
            CancelInvoke();
        }
        
        if (prefabToSpawn != null)
        {
            ammountSpawned++;
            return Instantiate(prefabToSpawn, transform.position, Quaternion.identity);  
        }
        return null;
    }
}
