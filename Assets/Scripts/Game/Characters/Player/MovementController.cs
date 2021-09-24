using UnityEngine;
using System.Collections;

// se ocupa de logica de miscare a caracterului
public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 3.0f; // reprezinta cat de repede se misca caracterul
    
    private int ok = 1;                 // folosit pentru a verifica ca viteza de rostogolire se seteaza corect
    private Animator animator;          // referinta la componenta de animatie
    private Rigidbody2D rb2D;           // referinta la componenta ce descrie "corpul" caracterului
    private Vector2 movement;           // vector de 2 coordonate ce va fi folosit pentru a misca caracterul
    private AudioManager audioManager;  // referinta la componenta audio

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()       // se apeleaza in functie de numarul de frameuri in care ruleaza jocul
    {
        // se ocupa de logica setarii corecte a animatiilor in functie de ce face jucatorul
        UpdateState();
    }

    private void FixedUpdate()  // se apeleaza o data la 1/50 s
    {
        // se ocupa de logica de a misca caracterul corect in functie de ce face jucatorul
        MoveCharacter();
    }

    private void MoveCharacter()
    {   
        if (!animator.GetBool("isRolling") && !animator.GetBool("isShooting") && !animator.GetBool("isHealing") &&
            !animator.GetBool("isAttacking"))
        {
            // apasand tasta A vom avea valoarea de -1 iar apasand tasta D vom avea valoarea de +1
            movement.x = Input.GetAxisRaw("Horizontal"); 

            // apasand tasta S vom avea valoarea de -1 iar apasand tasta W vom avea valoarea de +1
            movement.y = Input.GetAxisRaw("Vertical");

            // normalizam vectorul, astfel incat daca jucatorul apasa orice combinatie care l-ar face sa mearga in
            // diagnoala, acesta ar avea viteza sqrt(2), iar noi vrem sa aiba viteza 1
            movement.Normalize();

            // setam velocitatea(viteza) caracterului in functie de tastele apasate
            rb2D.velocity = movement * movementSpeed;
            ok = 1;      
        } 
        else if (animator.GetBool("isRolling"))
        {
            // ne asiguram ca viteza caracterului cand se rostogoleste se dubleaza doar o data
            if (ok == 1)
            {
                rb2D.velocity *= 2; // dublam velocitatea caracterului
                ok = 0;
            }
        } 
        else if (animator.GetBool("isShooting") || animator.GetBool("isHealing") || animator.GetBool("isAttacking"))
        {
            // daca caracterul se afla in oricare din starile din conditie, acesta va sta pe loc
            rb2D.velocity *= 0;
        }
    }
    
    // aici vom seta animatiile in functie de cum controleaza jucatorul acest caracter
    private void UpdateState()
    {
        if (!PauseMenu.isGamePaused)
        {
            if (Mathf.Approximately(movement.x, 0) && Mathf.Approximately(movement.y, 0))
            {
                animator.SetBool("isWalking", false);
            }
            else
            {
                // prin acest mod de implementare cunoastem intotdeauna directia in care se uita caracterul,
                // astfel animatorul stie corect ce animatii trebuie sa ruleze
                animator.SetFloat("xDir", movement.x);
                animator.SetFloat("yDir", movement.y);
                animator.SetBool("isWalking", true);
            }

            if (Input.GetKeyDown("space") && animator.GetBool("isWalking") && !animator.GetBool("isRolling") && 
                !animator.GetBool("isShooting") && !animator.GetBool("isHealing") && !animator.GetBool("isAttacking"))
            {
                animator.SetBool("isRolling", true);
                audioManager.Play("PlayerRoll");
            }
        }
    }

    // functiile de mai jos vor fi folosite ca si eventuri in cadrul editorului care se ocupa de logica animatiilor
    private void RollingEnded()
    {
        animator.SetBool("isRolling", false);
    }

    private void HealingEnded()
    {
        animator.SetBool("isHealing", false);
    }

    private void ShootingEnded()
    {
        animator.SetBool("isShooting", false);
    }

    private void StartWalkingSound()
    {
        audioManager.Play("PlayerWalk");
    }

    private void AttackingEnded()
    {
        animator.SetBool("isAttacking", false);
    }
}