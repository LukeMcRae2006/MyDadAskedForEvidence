using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Inputs")]
 
    public InputActionReference moveInput, lookInput, aimInput;

    [Header("Script References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private PlayerInput playerInput;
    [Header("Right Hand References")]
    [SerializeField] private Animator rightHandAnim;

    public static PlayerController Instance { get; private set; }

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

  



    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleLook();
        AimCamera();
    }



    // RIGHT HAND
    private void AimCamera()
    {
        

        rightHandAnim.SetBool("isAiming", aimInput.action.IsPressed());


    }



    private void HandleMovement()
    {
       
        playerMovement.Move(moveInput.action.ReadValue<Vector2>());
    }

    private void HandleLook()
    {
       
        playerLook.Look(lookInput.action.ReadValue<Vector2>());
    }
}
