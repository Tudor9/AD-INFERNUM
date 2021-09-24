using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// componenta care se ocupa de logica meniului de pauza
public class PauseMenu : MonoBehaviour
{
    // folosita pentru a semnala in alte parti ale jocului daca acesta este in pauza sau nu
    public static bool isGamePaused = false;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private GameObject loadingScreen;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private GameObject pauseMenuUI;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // verificam in fiecare frame daca jucatorul a apasat butonul de pauza
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                // contine logica pentru revenirea din pauza a jocului
                Resume();
            } 
            else 
            {
                // contine logica pentru intrarea in pauza a jocului
                Pause();
            }
        }
    }

    private void Resume()
    {
        audioManager.Play("PauseEnter");
        pauseMenuUI.SetActive(false);

        // Time.timeScale reprezinta viteza la care trece timpul, 1 - ca in viata de reala, 0 - oprit
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    private void Pause()
    {
        audioManager.Play("PauseExit");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    // metoda pentru a incarca scena precedenta a jocului
    private void LoadMenu()
    {
        StartCoroutine(LoadLevelBar());
    }

    // metoda care implementeaza un loading bar prin incarcarea asincrona a scenei urmatoare si prin a arata o bara de 
    // progres a acestei operatii
    private IEnumerator LoadLevelBar()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex 
            - 1);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            var progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}