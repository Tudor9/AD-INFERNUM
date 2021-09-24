using UnityEngine;

// contine logica unui glont
public class Ammo : MonoBehaviour
{
    [SerializeField]
    private int damageInflicted;    // ranile cauzate de glont

    // implementam logica unui glont atunci cand se loveste de un obiect/caracter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is BoxCollider2D && !collision.tag.Equals("Player") && 
            (collision.tag.Equals("Enemy") || collision.tag.Equals("Boss")))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();

            // aici este implementata logica ca atunci cand un inamic este lovit, acesta va merge in directia in care
            // se afla jucatorul
            if (collision.gameObject.GetComponent<Stationary_NoWeapon>() != null)
            {
                var ai = collision.gameObject.GetComponent<Stationary_NoWeapon>();
                ai.endPosition = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
                ai.StartCoroutine(ai.Move());
            } 
            else if (collision.gameObject.GetComponent<ClericBeast>() != null)
            {
                var ai = collision.gameObject.GetComponent<ClericBeast>();
                ai.endPosition = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
                ai.StartCoroutine(ai.Move());
            }

            StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));

            gameObject.SetActive(false);
        } 
        else if (collision.tag.Equals("Enviorment"))
        {
            // daca glontul loveste un obiect cu tagul "Enviorment", il dezactivam
            gameObject.SetActive(false);
        }
    }
}