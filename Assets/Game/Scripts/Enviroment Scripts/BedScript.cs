using UnityEngine;

public class BedScript : MonoBehaviour, IInteractable
{
  private bool playerInBed = true;
    [SerializeField] private AudioSource bedAudio;
    [SerializeField] private AudioClip getInClip, getOutClip;
    public void Interact()
    {
        if(playerInBed)
        {
            playerInBed = false;
            PlayerController.Instance.CrawlOutOfBed();
            bedAudio.clip = getOutClip;
        }
        else
        {
            playerInBed = true;
            PlayerController.Instance.CrawlIntoBed();
            bedAudio.clip = getInClip;
        }
        
        bedAudio.Play();
    }
}
