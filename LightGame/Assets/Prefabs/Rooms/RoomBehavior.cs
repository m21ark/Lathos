using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{

    private bool alreadyDefined = false;

    public GameObject[] gates; // up, down, right, left
    public bool[] openStatus; // Indicate doors that should be opened

    void Start()
    {
        // if(openStatus.Length > 0)
            // UpdateRoom(openStatus);
    }

    public void UpdateRoom(bool[] status)
    {
        if(alreadyDefined){
            Debug.Log("ATTEMPT TO REDIFINE ROOM");
            return;
        }

        alreadyDefined = true;
        string s = "";
        for(int i = 0; i < status.Length; i++)
        {
            s +=  i + "-" + status[i] + " ; ";
            bool aux = !status[i];
            gates[i].SetActive(aux);
        }
        Debug.Log("Gate: " + s);
    }
}
