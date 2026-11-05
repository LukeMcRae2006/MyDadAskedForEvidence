using UnityEngine;

public class BedScript : MonoBehaviour, IInteractable
{
  private bool playerInBed = false;
    public void Interact()
    {
        if(playerInBed)
        {
            playerInBed = false;
            PlayerController.Instance.CrawlOutOfBed();
        }
        else
        {
            playerInBed = true;
            PlayerController.Instance.CrawlIntoBed();
        }
        
    }
}
