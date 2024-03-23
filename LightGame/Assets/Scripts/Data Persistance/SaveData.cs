using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Data to save
    public int currentPlayerArea = 1; // Current scene the player is in
    public bool[] unlockedEndings = { false, false, false }; // Endings unlocked by player
    public string playerClassName = "Base";

    public float[] position; // Player position (TEMPORARY JUST FOR TESTING)

    public SaveData(Transform trans)
    {
        position = new float[3];
        position[0] = trans.position.x;
        position[1] = trans.position.y;
        position[2] = trans.position.z;
    }
}
