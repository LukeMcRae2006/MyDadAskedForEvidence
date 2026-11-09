using UnityEngine;

public class WindowScript : MonoBehaviour, IInteractable
{
    private bool isWindowOpen = false;
    [SerializeField] private Animator anim;
    public void Interact()
    {
        isWindowOpen = !isWindowOpen;
        anim.SetBool("isWindowOpen", isWindowOpen);
        RoomManager.Instance.isWindowOpen = isWindowOpen;

    }
}
