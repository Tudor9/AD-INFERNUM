using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// setam ca si conditii apartenenta componentelor de acest tip
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
// se va ocupa de logica AI-ului
public class ClericBeast : MonoBehaviour
{
    private CircleCollider2D circleCollider;    // va fi folosit ca sa setam distanta de vazut

    [SerializeField]
    private float pursuitSpeed;                 // viteza caracterului

    // setam o variabila de tip corutina pentru a nu initializa una noua de fiecare data cand acest caracter urmareste 
    // jucatorul
    private Coroutine moveCoroutine;            
    private Rigidbody2D rb2d;                   // referinta la "corpul" obiectului
    private Animator animator;                  // referinta la componenta de animatie 
    private Vector3 initialPosition;            // vom salva pozitia initiala a obiectului

    // va deveni "Transform-ul" jucatorului, pentru a putea vedea constant unde acesta se afla
    private Transform targetTransform; 

    // va reprezenta pozitia finala in care trebuie sa ajunga caracterul - pozitia initiala sau pozitia jucatorului
    [HideInInspector]
    public Vector3 endPosition;

    private void Start()    // initializam datele
    {
        initialPosition = transform.position;
        circleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>(); 
    }

    // folosim aceasta functie pentru a desena un cerc in jurul oricarui caracter ce contine aceasta componenta pentru a 
    // putea observa zona pe care caracterul o "vede"
    private void OnDrawGizmos()
    {
        if (circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius * transform.localScale.x);
        }
    }

    // logica de urmarire incepere a urmaririi jucatorului
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetTransform = collision.gameObject.transform;
            endPosition = targetTransform.position;

            circleCollider.radius *= 3;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move());
        }
    }

    // logica cand jucatorul iese din "vederea" inamicului
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            circleCollider.radius /= 3;
            animator.SetBool("isWalking", false);

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            endPosition = initialPosition;
            targetTransform = null;

            moveCoroutine = StartCoroutine(Move());
        }
    }

    // logica care misca caracterul in functie de unde acesta trebuie sa ajunga si logica de atac
    public IEnumerator Move()
    {
        // salvam distanta care o avem de parcurs pentru ca sa stie caracterul cum sa se comporte
        var remainingDistance = (transform.position - endPosition).sqrMagnitude;

        // cat timp nu am ajuns la destinatie
        while (remainingDistance > float.Epsilon)
        {   
            // daca urmarim jucatorul
            if (targetTransform != null)
            {
                endPosition = targetTransform.position;

                // conditia de incepere a atacului acestui caracter
                if (remainingDistance < 12)
                {
                    // asteptam ca corutina de atac sa se termine
                    yield return Attack();
                } 
                else if (remainingDistance >= 12)
                {
                    // resetam acesti parametrii pentru ca acest caracter sa nu atace aleatoriu cand jucatorul iese din
                    // raza de atac al acestuia
                    animator.ResetTrigger("NormalAttack");
                    animator.ResetTrigger("HeavyAttack");
                }
            }
            
            // logica de miscare a caracterului si de setare corecta a animatiei
            if (rb2d != null)
            {
                var newPosition = Vector3.MoveTowards(rb2d.position, endPosition, pursuitSpeed * Time.deltaTime);

                animator.SetFloat("xDir", newPosition.x - rb2d.position.x);

                animator.SetBool("isWalking", true);

                rb2d.MovePosition(newPosition);
                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }
        animator.SetBool("isWalking", false);
    }

    // descriem logica unui atac
    private IEnumerator Attack()
    {
        // caracterul se opreste
        rb2d.MovePosition(this.transform.position);
        animator.SetBool("isWalking", false);

        // asteptam 0.25 secunde
        yield return new WaitForSeconds(0.25f);

        // caracterul va alege un atac aleatoriu si va executa animatia acestuia
        animator.SetTrigger(Random.Range(0,5) == 0 ? "NormalAttack" : "HeavyAttack");
    }
    
    // folosit pentru a desena o linie de la pozitia caracterului la destinatia acestuia
    private void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red);
    }
}
