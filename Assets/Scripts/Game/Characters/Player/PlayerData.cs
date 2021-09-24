using UnityEngine.Serialization;

// va contine datele care dorim sa le salvam
[System.Serializable]
public class PlayerData
{
    public int currentHP;
    public int ammo;
    public int healthPotions;
    public int highScore;
    public string characterName;
    public string[] entities;

    // constructorul pentru clasa, apelat din scriptul playerului
    public PlayerData(Player player, string[] entityNames)
    {
        currentHP = player.currentHP.value;
        ammo = player.ammo.value;
        healthPotions = player.healthPotions.value;
        highScore = player.highScore.value;
        characterName = MenuFunctions.characterName;
        entities = entityNames;
    }
}

