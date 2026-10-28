using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]    
    public float moveSpeed;
    
    [Header("References")] 
        [SerializeField] private Rigidbody rb;
    private Vector2 moveInput;
    private PlayerInput playerInput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = new PlayerInput();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MovePlayer()
    {
        
    }
    
    
    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
