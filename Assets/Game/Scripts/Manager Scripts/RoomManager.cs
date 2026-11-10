using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public bool isWindowOpen = false;
    public bool isClosetOpen = false;
    public bool isDoorOpen = false;
    public bool isVentOpen = false;
    public bool creatureIsBeingSeen = false;
    

    public static RoomManager Instance { get; private set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
 
         private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }






    }
}