using UnityEngine;

public class VentScript : MonoBehaviour, IInteractable, IApproachable
{
     private bool isVentOpen = false;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource audioSource;
    public void Interact()
    {
        if(isVentOpen)
        {
            return;
        }
        isVentOpen = true;
        anim.SetBool("isVentOpen", isVentOpen);
        RoomManager.Instance.isVentOpen = isVentOpen;
        if (audioSource != null)
        {
            audioSource.Play();
        }

        
    }

    public void SetApproachDetection(bool isDetected)
    {
        RoomManager.Instance.isVentDetected = isDetected;
    }
}
