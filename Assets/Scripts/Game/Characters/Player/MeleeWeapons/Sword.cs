using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] 
    private int damageInflicted;        // reprezinta cate "daune" va provoca arma atunci cand loveste un inamic
    
    private Animator animator;          // referinta la componenta de animatie
    private Animator animatorParent;    // referinta la componenta de animatie al "parintelui" obiectului
    private AudioManager audioManager;  // referinta la componenta audio

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>();
        animatorParent = transform.parent.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (PauseMenu.isGamePaused) return;
        
        if (Input.GetMouseButtonDown(1) && !animatorParent.GetBool("isHealing") && !animatorParent.GetBool("isShooting")
            && !animatorParent.GetBool("isRolling") && !animatorParent.GetBool("isAttacking"))
        {
            Attack();
        }
    }

    // aici definim logica de "Attack" al sabiei
    private void Attack()
    {
        // pentru a stii ce animatie trebuie sa ruleze animatorul sabiei
        animator.SetFloat("xDir", animatorParent.GetFloat("xDir"));
        animator.SetFloat("yDir", animatorParent.GetFloat("yDir"));


        animator.SetTrigger("Attack");
        animatorParent.SetBool("isAttacking", true);
        audioManager.Play("PlayerSwordAttack");
    }

    // atunci cand sabia face contact cu un obiect
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is BoxCollider2D && !collision.tag.Equals("Player") && 
        (collision.tag.Equals("Enemy") || collision.tag.Equals("Boss")))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
        }

        if (collision.tag.Equals("Destructible"))
        {
            // comunicam componentei de animatie al obiectului lovit "trigger-ul" pentru animatie
            collision.gameObject.GetComponent<Animator>().SetTrigger("Destroy");

            // dezactivam componenta colider a obiectului atunci cand il distrugem, intrucat sa putem merge peste el
            collision.gameObject.GetComponent<Collider2D>().enabled = false;

            // distrugem obiectul dupa 3 secunde
            Destroy(collision.gameObject, 3);
        }
    }
}
