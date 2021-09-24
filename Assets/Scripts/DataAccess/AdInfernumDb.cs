using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.DataAccess.Models;

public class AdInfernumDb : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu, loginMenu, registerMenu, logoutButton;

    [SerializeField]
    private InputField registerUserInput, registerPasswordInput, registerRePasswordInput, loginUserInput, loginPasswordInput;

    [SerializeField]
    private Text[] usernameHighscoreMenu;

    [SerializeField]
    private Text[] highscore;

    [SerializeField]
    private Text registerMessageText, loginMessageText;

    [SerializeField]
    private Text usernameLoginInput, passwordLoginInput;

    [SerializeField]
    private Text usernameRegisterInput, passwordRegisterInput, confirmPasswordRegisterInput;

    public static int IDKey { get; private set; }
    public static string username;

    [SerializeField]
    private HighScore newScore;
    private string oldScore;
    
    

    public void Login()
    {
        var isUserValid = true;
        var validations = DataValidator
            .Use(loginUserInput.text, "Username", new[] { ValidationOption.NotEmpty })
            .Use(loginPasswordInput.text, "Password", new[] { ValidationOption.NotEmpty });

        var userVerification = DataValidator
            .Use(loginUserInput.text, "Username", new[] { ValidationOption.IsUnique });
        foreach (var validation in validations)
        {
            //treat it accordingly
            //Exmaple

            if (!validation.Result)
            {
                isUserValid = false;

                //if user is not valid get validation message and treat it accordingly
                loginMessageText.text = validation.Column + " " + validation.Message; 
                //or something, maybe show a ui error
                //you can get property that failed from validaion.column
            }

        }

        if (userVerification[userVerification.Count - 1].Result)
        {
            loginMessageText.text = userVerification[0].Column + " doesn't exist!";
            isUserValid = false;
        }


        if (!isUserValid) return; //don't login the user if it's not valid

        try
        {
            IDbCommand dbcmd = DbConfiguration().Item1;
            IDbConnection dbconn = DbConfiguration().Item2;

            string sqlQuery = "SELECT * FROM Users WHERE Username= '" + loginUserInput.text + 
                "' AND Password= '" + loginPasswordInput.text + "'";

            dbcmd.CommandText = sqlQuery;
            IDKey = Convert.ToInt32(dbcmd.ExecuteScalar());

            Debug.Log(IDKey);

            // user logged in succesfully
            if (IDKey != 0)
            {
                loginMessageText.text = "";
                username = usernameLoginInput.text;
                MenuFunctions.isLoggedIn = true;
                loginUserInput.text = "";
                loginPasswordInput.text = "";
                mainMenu.SetActive(true);
                logoutButton.SetActive(true);
                loginMenu.SetActive(false);
            }
            else
            {
                loginMessageText.text = "Password is incorrect!";
            }

            dbcmd.Dispose();

            dbconn.Close();

        }
        catch (Exception ex)
        {
            Debug.Log("Error" + ex);
        }
    } // Login
    


    public void Register()
    {
        var isUserValid = true;
        var user = new User(registerUserInput.text, registerPasswordInput.text, registerRePasswordInput.text);

        var validations = DataValidator
            .Use(user.Username, "Username", new[] {
                    ValidationOption.IsUnique,
                    ValidationOption.NotEmpty
                //,ValidationOption.ExampleOption you can add as many as you want
            })
            .Use(user.Password, "Password", new[] { ValidationOption.NotEmpty })
            .Use(user.RePassword, "ConfirmPassword", new[] { ValidationOption.NotEmpty });

        foreach (var validation in validations)
        {
            //treat it accordingly
            //Exmaple

            if (!validation.Result)
            {
                isUserValid = false;

                //if user is not valid get validation message and treat it accordingly
                //or something, maybe show a ui error                                                            
                //you can get property that failed from validaion.column
                registerMessageText.text = validation.Column + " " + validation.Message; 
            }
        }

        if (registerPasswordInput.text != registerRePasswordInput.text)
        {
            registerMessageText.text = "Passwords don't match!"; //textField
            isUserValid = false;
        }

        if (!isUserValid) return; //don't register the user if it's not valid

        try
        {
            IDbCommand dbcmd = DbConfiguration().Item1;
            IDbConnection dbconn = DbConfiguration().Item2;
            string sqlQuery = "INSERT INTO Users ('Username', 'Password') VALUES ('" + registerUserInput.text + "','" + registerPasswordInput.text + "')";
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();

            registerMessageText.text = "";
            registerPasswordInput.text = "";
            registerRePasswordInput.text = "";
            registerUserInput.text = "";

            registerMenu.SetActive(false);
            loginMenu.SetActive(true);

            reader.Close();

            dbcmd.Dispose();

            dbconn.Close();

        }
        catch (SqliteException ex)
        {
            Debug.Log(ex.Message);
        }


    } // Registration
    
    

    public void SaveHighScore() 
    {
        
            IDbCommand dbcmd = DbConfiguration().Item1;
            IDbConnection dbconn = DbConfiguration().Item2;
            
            string sqlQuery = "SELECT score from Users WHERE Id = '" + IDKey + "'";
            dbcmd.CommandText = sqlQuery;

            oldScore = Convert.ToString(dbcmd.ExecuteScalar());

            if (newScore.value > Convert.ToInt32(oldScore))
            {
                sqlQuery = "UPDATE Users SET Score = '" + newScore.value + "' WHERE Id ='" + IDKey + "'";
                dbcmd.CommandText = sqlQuery;
                IDataReader reader = dbcmd.ExecuteReader();
                reader.Close();

                dbcmd.Dispose();

                dbconn.Close();
            }
    } //SaveHighScore

    
    
    public void Logout() 
    {
        IDKey = 0;
        username = "";
        MenuFunctions.isLoggedIn = false;
    }
    
    

    public void ShowHighScore()
    {
        IDbCommand dbcmd = DbConfiguration().Item1;
        IDbConnection dbconn = DbConfiguration().Item2;
        
        string sqlQuery = "SELECT Username, Score from Users ORDER BY Score DESC limit 5";
        dbcmd.CommandText = sqlQuery;
        SqliteDataAdapter daUniv = new SqliteDataAdapter((SqliteCommand)dbcmd);
        DataSet dsUser = new DataSet();
        daUniv.Fill(dsUser);
        
        int i = 0;

        foreach(DataRow dr in dsUser.Tables[0].Rows)
        {
            usernameHighscoreMenu[i].text = dr.ItemArray[0] + "";
            highscore[i].text = dr.ItemArray[1] + "";
            i++;
        } 
        
        dbcmd.Dispose();

        dbconn.Close();
        
    } //ShowHighScore
    
    

    private (IDbCommand,IDbConnection) DbConfiguration()
    {
        string conn = $"URI=file:{Application.dataPath}/AD_INFERNUM_DB.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        return (dbcmd,dbconn);
    } //DbConfiguration
    
    public void ClearLoginFields()
    {
        loginUserInput.text = "";
        loginPasswordInput.text = "";
        loginMessageText.text = "";
    }

    public void ClearRegisterFields()
    {
        registerUserInput.text = "";
        registerPasswordInput.text = "";
        registerRePasswordInput.text = "";
        registerMessageText.text = "";
    }
}
