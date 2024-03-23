using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float[] position;

    public SaveData(Transform trans)
    {
        position = new float[3];
        position[0] = trans.position.x;
        position[1] = trans.position.y;
        position[2] = trans.position.z;
    }
}