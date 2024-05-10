using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Data to save
    public int currentPlayerArea = 1; // Current scene the player is in
    public bool[] unlockedEndings = { false, false, false }; // Endings unlocked by player
    public string playerClassName = "Base";

    public SaveData()
    {

    }
}
