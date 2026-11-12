using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    //is open bools
    [Header("Open Approach Checks")]
    public bool isWindowOpen = false;
    public bool isClosetOpen = false;
    public bool isDoorOpen = false;
    public bool isVentOpen = false;
  

    //detection bools
    [Header("Detection Checks")]
    public bool creatureIsBeingSeen = false;
    public bool isWindowDetected = false;
    public bool isClosetDetected = false;
    public bool isDoorDetected = false;
    public bool isVentDetected = false;

   
    

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