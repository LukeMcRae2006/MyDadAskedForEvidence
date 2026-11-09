using UnityEngine;

public class ClosetScript : MonoBehaviour, IInteractable
{
    private bool isOpen = false;
    [SerializeField] private Animator closetAnimator;

    public void Interact()
    {
        isOpen = !isOpen;
        closetAnimator.SetBool("isOpen", isOpen);
        RoomManager.Instance.isClosetOpen = isOpen;
    }
}
