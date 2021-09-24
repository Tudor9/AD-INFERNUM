using System.Collections;
using UnityEngine;

public class Enemy : Characters
{
    private SpriteRenderer sprite;  // referinta la componenta de render
    private int hitPoints;          // date despre punctele de viata ale caracterului

    [SerializeField]
    private int highScoreValue;     // date despre cat scor valoreaza acest caracter

    [SerializeField]
    private int damageStrength;     // puterea de atac a caracterului
    
    private AudioManager audioManager;  // referinta la componenta audio
    private Coroutine damageCoroutine;  // referinta la corutina de "daune"

    // referinta la un GameObject sablon, predefinit de noi, ce contine UI-ul acestui caracter
    [SerializeField]
    private HealthBar healthBarPrefab;  

    // variabila in care se va stoca user interface-ul caracterului
    private HealthBar healthBar;

    private void OnEnable()
    {
        audioManager = FindObjectOfType<AudioManager>();
        sprite = GetComponent<SpriteRenderer>();
        ResetCharacter();
	    healthBar.health = hitPoints;
    }

    // cu ajutorul acestei functii vom implementa logica care tine de "ranile" care le primeste caracterul
    // daca intervalul este 0, va fi ranit o singura data, altfel, va fi ranit la fiecare interval definit
    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            if (hitPoints - damage < 0)
            {
                hitPoints = 0;
            } else 
            {
                audioManager.Play("EnemyHurt");
                hitPoints -= damage;
                healthBar.health = hitPoints;
                healthBar.UpdateHealthBar();
            }

            if (hitPoints <= 0)
            {
                audioManager.Play("EnemyDeath");
                var player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.highScore.value += highScoreValue;
                KillCharacter();
                break;
            }

            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }

    // atunci cand caracterul face contact cu un obiect
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();

            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(player.DamageCharacter(damageStrength, 1.0f));
            }
        }
    }

    // atunci cand caracterul iese din contact cu un obiect
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    // logica de initializare a acestui caracter
    public override void ResetCharacter()
    {
        hitPoints = startingHP;
        healthBar = Instantiate(healthBarPrefab, transform, true);
        
        var transform1 = healthBar.transform;
        transform1.position = transform.position + new Vector3(0, sprite.bounds.extents.y + 1, 0);
        
        var rt = (RectTransform)transform1;
        if (sprite.bounds.extents.x * 2 < rt.rect.width)
        {
            rt.localScale = new Vector3(0.5f, 0.5f, 1);
            healthBar.transform.position = this.transform.position + new Vector3(0, sprite.bounds.extents.y + 0.5f, 0);
        }
        healthBar.maxHealth = maxHP;
        healthBar.health = startingHP;
    }
}