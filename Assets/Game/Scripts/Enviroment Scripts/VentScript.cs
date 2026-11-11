using UnityEngine;

public class VentScript : MonoBehaviour, IInteractable, IApproachable
{
     private bool isVentOpen = false;
    [SerializeField] private Animator anim;
    public void Interact()
    {
        isVentOpen = true;
        anim.SetBool("isVentOpen", isVentOpen);
        RoomManager.Instance.isVentOpen = isVentOpen;
    }

    public void SetApproachDetection(bool isDetected)
    {
        RoomManager.Instance.isVentDetected = isDetected;
    }
}
