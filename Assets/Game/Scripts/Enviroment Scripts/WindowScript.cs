using UnityEngine;

public class WindowScript : MonoBehaviour, IInteractable, IApproachable
{
    private bool isWindowOpen = false;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioClip openAudioClip, closeAudioClip;
    [SerializeField] private AudioSource audioSource;
    public void Interact()
    {
        isWindowOpen = !isWindowOpen;
        anim.SetBool("isWindowOpen", isWindowOpen);
        RoomManager.Instance.isWindowOpen = isWindowOpen;
        //play interact audio
        audioSource.clip = isWindowOpen ? openAudioClip : closeAudioClip;
        audioSource.Play();

    }

    public void SetApproachDetection(bool isDetected)
    {
        RoomManager.Instance.isWindowDetected = isDetected;
    }
}
