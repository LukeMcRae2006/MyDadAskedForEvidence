using UnityEngine;

public class RoomManager : MonoBehaviour
{

    public bool isClosetOpen = false;
    public bool isWindowOpen = false;   
    public bool isDoorOpen = false;
    public bool isVentOpen = false;


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