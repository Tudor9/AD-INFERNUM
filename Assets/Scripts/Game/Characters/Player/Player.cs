using System.Collections;
using System.Linq;
using UnityEngine;

public class Player : Characters
{
    private Animator animator;  // referinta la componenta de animatie
    private int cd;             // variabila ce va avea rol de a limita folosirea repetata a potiunilor
    private AudioManager audioManager;  // referinta la componenta audio

    public HitPoints currentHP;     // referinta la viata caracterului
    public ItemCount ammo;          // referinta la numarul de gloante al caracterului
    public ItemCount healthPotions; // referinta la numarul de potiuni al caracterului
    public HighScore highScore;     // referinta la scorul caracterului

    [SerializeField]
    private UI uiPrefab;    // referinta la un GameObject sablon, predefinit de noi, in acest caz UI-ul caracterului
    private UI ui;          // variabila in care se va stoca user interface-ul caracterului

    private void Start()    // se apeleaza atunci cand caracterul este initializat
    {
        ResetCharacter();
        audioManager = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>();
    }

    private void Update()   // se apeleaza in functie de numarul de frameuri in care ruleaza jocul
    {
        if (PauseMenu.isGamePaused) return; // verifica daca jocul este in meniul de pauza
        
        // implementarea functionalitatii de "heal" a caracterului, verificam daca caracterul nu executa
        // alta actiune atunci cand apasam pe buton
        if(Input.GetKeyDown("q") && currentHP.value != maxHP && healthPotions.value > 0 && cd < 0 
           && !animator.GetBool("isShooting") && !animator.GetBool("isRolling") && !animator.GetBool("isAttacking"))
        {
            // ajustam viata caracterului cu +3
            AdjustHitPoints(3);       

            // se comunica componentei de animatie modificarea unui parametru
            animator.SetBool("isHealing", true);  

            // se da play la sunetul de "heal"  
            audioManager.Play("PlayerHeal");       
            
            // se consuma o potiune
            healthPotions.value--;    

            // setam un "cooldown" de ~5 secunde              
            cd = 100;                               
        }
    }

    private void FixedUpdate()  // se apeleaza o data la 1/50s
    {
         cd--;
    }
    
    // atunci cand intram in raza unui collider de tip trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // daca obiectul are tagul corespunzator(poate fi cules)
        if (collision.gameObject.CompareTag("CanBePickedUp"))   
        {
            // salvam datele care le contine obiectul pe care il vom culege
            var hitObject = collision.gameObject.GetComponent<Consumable>().item;   

            // verificam ca obiectul sa nu fie null
            if (hitObject == null) return;                                          

            // in functie de ce obiect culegem, vom seta daca acesta ar trebui sa dispara sau nu
            var shouldDisappear = false;

            // dam play sunetului de culegere a unui obiect
            audioManager.Play("PickItem");
            
            // itemType este un ENUM ce contine ce tip de obiect poate fi cel cules, AMMO/HEALTH
            switch (hitObject.itemType)
            {
                case Item.ItemType.AMMO:
                    shouldDisappear = true;

                    // se adauga cantitatea culeasa la numarul curent al caracterului
                    ammo.value += hitObject.quantity;           
                    break;
                case Item.ItemType.HEALTH:
                    shouldDisappear = true;

                    // se adauga cantitatea culeasa la numarul curent al caracterului
                    healthPotions.value += hitObject.quantity;
                    break;
                default:
                    break;
            }

            // daca am decis ca obiectul trebuie sa dispara il vom dezactiva
            if (shouldDisappear)
            {
                collision.gameObject.SetActive(false);
            }
        }
    }


    // cu ajutorul acestei functii vom implementa logica care tine de "ranile" care le primeste caracterul
    // daca intervalul este 0, va fi ranit o singura data, altfel, va fi ranit la fiecare interval definit
    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            // daca caracterul este ranit
            if (currentHP.value - damage < 0)
            {
                currentHP.value = 0;
            } else 
            {
                audioManager.Play("PlayerHurt");
                currentHP.value = currentHP.value - damage;
            }

            // daca caracterul moare
            if (currentHP.value == 0)
            {
                audioManager.Play("PlayerDeath");
                animator.SetTrigger("Death");
                KillCharacter();
                break;
            }

            if (interval > float.Epsilon)
            {
                // asteptam sa treaca intervalul inainte de a mai aplica inca o data daunele asupra caracterului
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }
    
    // cand caracterul moare, vrem sa eliberam memoria ocupata de UI si de caracter
    public override void KillCharacter()
    {
        base.KillCharacter();
        Destroy(ui.gameObject);
    }

    public override void ResetCharacter()
    {
        // instantiem un UI nou si il atasam acestui caracter
        ui = Instantiate(uiPrefab);
        ui.character = this;
        
        // daca user-ul a ales sa continue pe un caracter creat anterior
        if (MenuFunctions.hasContinued)
        {
            LoadPlayer();
            MenuFunctions.hasContinued = false;
        }
        else
        {
            ammo.value = 50;
            healthPotions.value = 50;
            highScore.value = 0;
            currentHP.value = startingHP;
        }
    }

    private void AdjustHitPoints(int amount)
    {
        if (currentHP.value + amount <= maxHP) 
        {
            currentHP.value = currentHP.value + amount;
        } else if (amount > 0)
        {
            currentHP.value = maxHP;
        }
    }

    // logica de salvare a starii jocului
    private void SavePlayer()
    {
        // avand in vedere ca avem toate obiectele incarcate de la inceputul jocului, atunci cand salvam numele celor care
        // inca sunt in scena intr-un array
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var bosses = GameObject.FindGameObjectsWithTag("Boss");
        var destructibles = GameObject.FindGameObjectsWithTag("Destructible");
        var entityNames = new string[enemies.Length + bosses.Length + destructibles.Length];

        var i = 0;
        foreach(var enemy in enemies)
        {
            entityNames[i] = enemy.name;
            i++;
        }

        foreach(var boss in bosses)
        {
            entityNames[i] = boss.name;
            i++;
        }

        foreach(var destructible in destructibles)
        {
            entityNames[i] = destructible.name;
            i++;
        }

        SaveSystem.SavePlayer(this, entityNames);
    }

    // logica de incarcare a jocului in aceeasi stare ca atunci cand acesta sa salvat anterior
    private void LoadPlayer()
    {
        var playerData = SaveSystem.LoadPlayer();
        
        currentHP.value = playerData.currentHP;

        highScore.value = playerData.highScore;
        ammo.value = playerData.ammo;
        healthPotions.value = playerData.healthPotions;

        // cautam toti inamicii existenti in scena si comparam numele lor cu numele care le avem in fisier,
        // stergand pe cei care nu se regasesc in fisier
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var bosses = GameObject.FindGameObjectsWithTag("Boss");
        var destructibles = GameObject.FindGameObjectsWithTag("Destructible");

        foreach(var enemy in enemies)
        {
            if (!playerData.entities.Contains(enemy.name))
            {
                Destroy(enemy);
            }
        }

        foreach(var enemy in bosses)
        {
            if (!playerData.entities.Contains(enemy.name))
            {
                Destroy(enemy);
            }
        }

        foreach(var destructible in destructibles)
        {
            if (!playerData.entities.Contains(destructible.name))
            {
                Destroy(destructible);
            }
        }
    }
}