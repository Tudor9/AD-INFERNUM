using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// componenta de tip Singleton, se ocupa de initializarea jocului si de finalizarea acestuia in cazul mortii caracterului
// controlat de jucator
public class RPGGameManager : MonoBehaviour
{
    // pentru a avea referinta la camera pe care o v-om seta sa il urmareasca pe jucator
    [SerializeField]
    private RPGCameraManager cameraManager;     

    // referinta la viata curenta a caracterului controlat de jucator
    public HitPoints currentHP;

    // referinta la un UI pentru atunci cand caracterul moare
    [SerializeField]
    private GameObject deathUI;

    // referinta la componenta audio
    private AudioManager audioManager;

    // referinta la componenta care contine pozitia unde se va "initializa" caracterul jucatorului
    [SerializeField]
    private SpawnPoint playerSpawnPoint;

    // neimplementat, referinta la componenta de tip SpawnPoint
    [SerializeField]
    private SpawnPoint[] enemySpawnPoint;

    private AdInfernumDb db;

    private static RPGGameManager sharedInstance = null;

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        } else
        {
            sharedInstance = this;
        }
    }

    // LateUpdate se apeleaza dupa toate Update-urile din frame-ul curent
    // Utilizat pentru a verifica viata jucatorului la finalul fiecarui frame
    private void LateUpdate()
    {
        if (currentHP.value == 0)
        {
            deathUI.SetActive(true);
            db.SaveHighScore();
            StartCoroutine(RestartScene(3));
        }
    }

    // metoda pentru reinitializarea unei scene dupa un anumit interval de timp
    private IEnumerator RestartScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // metoda pentru initializarea caracterului controlat de jucator si atasarea camerei acestuia
    private void SpawnPlayer()
    {
        if (playerSpawnPoint != null)
        {
            var player = playerSpawnPoint.SpawnObject();
            cameraManager.virtualCamera.Follow = player.transform;
        }
    }

    private void SpawnEnemy()
    {
        if (enemySpawnPoint != null)
        {
            foreach(var enemySpawn in enemySpawnPoint)
            {
                GameObject enemy = enemySpawn.SpawnObject();
            }
        }
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        db = GetComponent<AdInfernumDb>();
        SetupScene();
    }

    // metoda pentru initializarea scenei si asigurarea functionarii corecte a acesteia
    private void SetupScene()
    {
        if (Time.timeScale == 0f) 
        {
            Time.timeScale = 1f;
            PauseMenu.isGamePaused = false;
        }
        
        SpawnPlayer();
        SpawnEnemy();
        audioManager.Play("Background");
    }
}
