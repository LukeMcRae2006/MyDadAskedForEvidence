using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Inputs")]
 
    public InputActionReference moveInput;
    public InputActionReference lookInput, aimInput, captureInput, editInput;

    [Header("Script References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private PlayerInput playerInput;
    [Header("Player FPS Camera References")]
    [SerializeField] private CinemachineCamera fpsCam;
    [Header("Right Hand References")]
    [SerializeField] private Animator rightHandAnim;
    [Header("Digital Camera References")]
    [SerializeField] private CameraController cameraController;

    [Header("Camera Attributes & Properties")]
    public bool isAimingCamera;

    public static PlayerController Instance { get; private set; }
    public PlayerState playerState;

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

    public enum PlayerState
    {
        Resting,
        Active,
        Editing
    }


    // Update is called once per frame
    void Update()
    {
       
       

        switch(playerState)
        {
            case PlayerState.Resting:
                Resting();
                break;
            case PlayerState.Active:
                Active();
                break;
            case PlayerState.Editing:
                Editing();
                break;  
        }
    }


    void Resting()
    {
        HandleLook();
        HandleCamera();
    }

    void Active()
    {
        HandleMovement();
        HandleLook();
        HandleCamera();
    }

    void Editing()
    {
        if(editInput.action.WasPressedThisFrame())
        {
            playerState = PlayerState.Active;
        }
    }



    #region Camera and Hands
    private void HandleCamera()
    {
        //detect input
        isAimingCamera = aimInput.action.IsPressed();
        //set right hand anim to isAiming
        rightHandAnim.SetBool("isAiming", isAimingCamera);
        //lerp field of view of fps Cam if player is aiming the camera
        fpsCam.Lens.FieldOfView = Mathf.Lerp(fpsCam.Lens.FieldOfView, isAimingCamera ? 45f : 60f, Time.deltaTime * 5f);

        if (isAimingCamera && captureInput.action.WasPressedThisFrame())
        {
            cameraController.TakePhoto();

        }

        if(isAimingCamera && editInput.action.WasPressedThisFrame())
        {
            playerState = PlayerState.Editing;
        }

    }
    #endregion


    #region Basic Movement and Looking
    private void HandleMovement()
    {
       
        playerMovement.Move(moveInput.action.ReadValue<Vector2>());
    }

    private void HandleLook()
    {
       
        playerLook.Look(lookInput.action.ReadValue<Vector2>());
    }

    #endregion
}
