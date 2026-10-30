using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Inputs")]
    public PlayerInputActions inputActions;
    public InputActionReference moveInput, lookInput;

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

        //Intialize input actions
        inputActions = new PlayerInputActions();


    }

    private void OnEnable()
    {
        //enable input actions
        inputActions.Player.Enable();
        
    }

    private void OnDisable()
    {
        //disable input actions
        inputActions.Player.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
