using UnityEngine;

public class VentScript : MonoBehaviour, IInteractable
{
     private bool isVentOpen = false;
    [SerializeField] private Animator anim;
    public void Interact()
    {
        isVentOpen = true;
        anim.SetBool("isVentOpen", isVentOpen);
        RoomManager.Instance.isVentOpen = isVentOpen;
    }
}
