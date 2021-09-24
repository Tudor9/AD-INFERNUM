using System.Collections.Generic;
using System;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using System.Linq;


public static class DataValidator
{
    static string connectionString => $"URI=file:{Application.dataPath}/AD_INFERNUM_DB.db";
    static IDbConnection connection;
    public static List<Validation> Use(
        string value,
        string column,
        ValidationOption[] options)
        => ValidateProperty(value, column, options);

    public static List<Validation> Use(
        this List<Validation> validations,
        string value,
        string column,
        ValidationOption[] options)
        => ValidateProperty(value, column, options, validations);

    private static List<Validation> ValidateProperty(
        string value,
        string column,
        ValidationOption[] options,
        List<Validation> validations = null)
    {
        if (validations is null) validations = new List<Validation>();
        string message = "";
        bool isValid = false;

        foreach (var option in options)
        {
            switch (option)
            {
                case ValidationOption.IsUnique:
                    (message, isValid) = IsUnique(value, column);
                    break;
                case ValidationOption.NotEmpty:
                    (message, isValid) = NotEmpty(value);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            validations.Add(new Validation()
            {
                Result = isValid,
                Message = message,
                Column = column
            });

        }

        return validations;
    }

    static (string, bool) NotEmpty(string value)
        => string.IsNullOrEmpty(value)
            ? ("field cannot be empty", false)
            : ("validation successful", true);

    static (string, bool) IsUnique(
        string value,
        string column)
    {
        (string, bool) returnValue;

        if (connection == null) connection = new SqliteConnection(connectionString);
        connection.Open();

        IDbCommand command = connection.CreateCommand();
        command.CommandText = $@"SELECT {column} FROM Users "; //todo where id is provided id
        IDataReader reader = null; //Yes it's null :)

        try
        {
            returnValue = (@"If you see this message something went terribly wrong", false);

            reader = command.ExecuteReader();
            var index = 0;
            List<string> entriesInDb = new List<string>();
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    entriesInDb.Add(reader.GetString(0));
                    index++;
                }
            }

            if (!entriesInDb.Any(entry => entry == value)) returnValue = ($"{column} is valid", true);
            else returnValue = ($"already exists!", false);

        }
        catch (SqliteException exception)
        {
            //log the exception
            Debug.Log(@$"Column with name {column} threw an exception: InnerException: {exception.Message}"); 

            return (exception.Message, false);
        }
        finally
        {
            reader?.Close();
            command.Dispose();
            connection.Close();
        }

        return returnValue;
    }

}

public class Validation
{

    public bool Result { get; set; }
    public string Message { get; set; }
    public string Column { get; set; }
}

public enum ValidationOption
{
    IsUnique = 0,
    NotEmpty = 1,
    //Todo add other rules and implement
}


