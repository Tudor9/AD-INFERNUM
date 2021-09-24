using System.IO;
using System.Collections;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// componenta care se ocupa de toate datele/metodele de care are nevoie meniul principal al jocului
public class MenuFunctions : MonoBehaviour
{
    public static bool hasContinued = false;
    public static int saveFile = 0;
    public static bool isLoggedIn = false;
    public static string characterName;

    [SerializeField]
    private GameObject logoutButton;

    [SerializeField]
    private GameObject[] buttons;

    [SerializeField]
    private Text[] ammoStats;

    [SerializeField]
    private Text[] healthPotionStats;

    [SerializeField]
    private Text[] highscoreStats;

    [SerializeField]
    private Text[] characterNames;

    [SerializeField]
    private GameObject noSaveSlotsAvailable;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Text playerName;

    [SerializeField]
    private GameObject continueButton;

    [SerializeField]
    private GameObject createMenu;

    [SerializeField]
    private GameObject chooseCharMenu;

    [SerializeField]
    private GameObject loginRegisterMenu;

    [SerializeField]
    private GameObject loadingScreen;

    private void Start()
    {
        // ne asiguram ca apare butonul de logout atunci cand user-ul este logat
        if (isLoggedIn)
        {
            logoutButton.SetActive(true);
        }
    }

    // metoda care incarca urmatoarea scena, si semnaleaza faptul ca jucatorul continua pe un "save" precedent
    public void ContinueButton()
    {
        hasContinued = true;
        LoadLevel();
    }

    // metoda care verifica daca atunci cand creem un caracter nou, exista spatiu pentru salvarea acestui caracter
    // in caz contrar, afiseaza acest lucru in UI
    public void CreateNewCharacter()
    {
        characterName = playerName.text;
        Directory.CreateDirectory(Application.persistentDataPath + "/" + AdInfernumDb.username);

        for (var i = 1; i < 4; i++)
        {
            if(!File.Exists(Application.persistentDataPath + "/" + AdInfernumDb.username + "/playerData" + i 
                + ".sav"))
            {
                SetSaveFile(i);
                break;
            } 
            else if (i == 3)
            {
                noSaveSlotsAvailable.SetActive(true);
                return;
            }
        }

        createMenu.SetActive(false);
        LoadLevel();
    }

    // metoda care afiseaza corect UI-ul in functie de faptul ca user-ul este logat sau nu
    public void ProcessStart()
    {
        if (isLoggedIn)
        {
            chooseCharMenu.SetActive(true);
            ShowButtonContinue();
        }
        else
        {
            loginRegisterMenu.SetActive(true);
        }
    }

    // metoda care afiseaza butonul de continue doar daca exista "save-uri"
    public void ShowButtonContinue()
    {
        for (var i = 1; i < 4; i++)
        {
            if(File.Exists(Application.persistentDataPath + "/" + AdInfernumDb.username + "/playerData" + i 
                + ".sav"))
            {
                continueButton.SetActive(true);
                return;
            }
        }

        continueButton.SetActive(false);
    }

    // metoda care afiseaza "save-urile" acestui cont, detinute local pe calculator, si care afiseaza informatii 
    // despre acestea
    public void ShowContinueOptions()
    {
        for (var i = 1; i < 4; i++)
        {
            if(File.Exists(Application.persistentDataPath + "/" + AdInfernumDb.username + "/playerData" + i 
                + ".sav"))
            {
                buttons[i - 1].SetActive(true);
                SetSaveFile(i);
                PlayerData playerData = SaveSystem.LoadPlayer();
                ammoStats[i - 1].text = "x" + playerData.ammo;
                healthPotionStats[i - 1].text = "x" + playerData.healthPotions;
                highscoreStats[i - 1].text = "Score: " + playerData.highScore;
                characterNames[i - 1].text = playerData.characterName + "";
            }
        }
    }

    // metoda pentru a incarca scena urmatoare a jocului
    private void LoadLevel()
    {
        StartCoroutine(LoadLevelBar());
    }

    // metoda care implementeaza un loading bar prin incarcarea asincrona a scenei urmatoare si prin a arata o bara de 
    // progres a acestei operatii
    private IEnumerator LoadLevelBar()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }

    //1-first save file, 2-second save file, etc (3 save files in total)
    public void SetSaveFile(int value) 
    {
        saveFile = value;
    }

    // metoda pentru a sterge un anumit save file
    public void DeleteSaveFile()
    {
        try
        {
            File.Delete(Application.persistentDataPath + "/" + AdInfernumDb.username + "/playerData" + saveFile 
                + ".sav");
        }
        catch
        {
            Debug.LogError("Something teribly wrong happened");
        }
    }
}
