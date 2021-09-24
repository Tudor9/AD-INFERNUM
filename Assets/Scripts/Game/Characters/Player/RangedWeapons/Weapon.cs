using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private ItemCount ammo;         // referinta la numarul de gloante al obiectului care detine aceasta arma

    private Animator animator;      // referinta la componenta de animatie

    [SerializeField]
    private float weaponVelocity;   // date despre velocitatea armei

    [SerializeField]
    public GameObject ammoPrefab;   // referinta la un obiect prestabilit, care va fi folosit pentru a reprezenta glontul

    private static List<GameObject> ammoPool;   // pentru a folosi notiunea de "object pooling"
    private AudioManager audioManager;  // referinta la componenta audio

    [SerializeField]
    private int poolSize;           // reprezinta cate gloante v-om avea in "pool"

    // initializam "pool-ul" de gloante
    private void Awake()
    {
        if (ammoPool == null)
        {
            
            ammoPool = new List<GameObject>();
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!PauseMenu.isGamePaused)
        {
            if (Input.GetMouseButtonDown(0) && ammo.value > 0 && !animator.GetBool("isShooting") &&
                !animator.GetBool("isHealing") && !animator.GetBool("isRolling") && !animator.GetBool("isAttacking"))
            {
                FireAmmo();
            }
        }
    }

    private GameObject SpawnAmmo(Vector3 location)
    {
        foreach (GameObject ammo in ammoPool)
        {
            if (ammo.activeSelf == false)
            {
                ammo.SetActive(true);
                ammo.transform.position = location;
                return ammo;
             }
        }
        return null;
    }
    
    // contine logica de tragere a armei
    private void FireAmmo()
    {
        GameObject ammoObject = SpawnAmmo(transform.position);

        if (ammoObject != null)
        {
            ammo.value--;

            // setam modul de "miscare" al glontului si directia acestuia
            Linear linearScript = ammoObject.GetComponent<Linear>();
            
            Vector2 direction = new Vector2(animator.GetFloat("xDir"), animator.GetFloat("yDir")) * weaponVelocity;

            linearScript.Travel(direction);

            animator.SetBool("isShooting", true);
            audioManager.Play("PlayerShoot");
        }
    }

    private void OnDestroy()    // se apeleaza atunci cand obiectul care contine aceasta componenta este distrus
    {
        ammoPool = null;
    }
}
