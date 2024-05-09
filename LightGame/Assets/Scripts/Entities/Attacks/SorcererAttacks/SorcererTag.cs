using UnityEngine;

public class SorcererTag : MonoBehaviour
{
    public int stackCounter = 1;

    public void addStack(int stackCounter = 1){
        this.stackCounter += stackCounter;
    }
}