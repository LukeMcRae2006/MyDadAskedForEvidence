using UnityEngine;

public class ClosetScript : MonoBehaviour, IInteractable, IApproachable
{
    private bool isOpen = false;
    [SerializeField] private Animator closetAnimator;
    [SerializeField] private AudioClip openAudioClip, closeAudioClip;
    [SerializeField] private AudioSource audioSource;

    public void Interact()
    {
        isOpen = !isOpen;
        closetAnimator.SetBool("isOpen", isOpen);
        RoomManager.Instance.isClosetOpen = isOpen;
        //play audio clip
        audioSource.clip = isOpen ? openAudioClip : closeAudioClip;
        audioSource.Play(); 



    }

    public void SetApproachDetection(bool isDetected)
    {
        RoomManager.Instance.isClosetDetected = isDetected;
    }
}
