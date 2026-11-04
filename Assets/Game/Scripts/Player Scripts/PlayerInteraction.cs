using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

public class PlayerInteraction : MonoBehaviour
{

    [SerializeField] private Camera fpsCam;
    private bool isInteracting = false;
    [SerializeField] private LayerMask doorLayer;
    [SerializeField] private float interactRange =5f;
    [SerializeField] private InputActionReference interactInput;

    [Header("Interaction State")]
    [SerializeField] private InteractionState currentInteractionState;

    [Header("Interaction UI")]
    [SerializeField] private Image interactCrosshair;

    [Header("Interacting With Door References")]
    [SerializeField] private Rigidbody playersHandRB;
    private GameObject doorObject;
    [SerializeField] private SpringJoint doorJoint;
    private Vector3 interactionPoint;

    public enum InteractionState
    {
        None,
        InteractingWithDoor
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnEnable()
    {
    
    }

    void OnDisable()
    {
        //on disable, make sure to disable the interact crosshair
        interactCrosshair.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInteractDetection();

        switch(currentInteractionState)
        {
            case InteractionState.None:
                // Handle logic for when the player is not interacting with anything
                None();
                break;
            case InteractionState.InteractingWithDoor:
                // Handle logic for when the player is interacting with a door
                InteractingWithDoor();
                break;
        }

    }

    private void InteractingWithDoor()
    {
        if (doorObject != null)
        {
            Rigidbody doorRB = doorObject.GetComponent<Rigidbody>();

            if (doorJoint.connectedBody == null)
            {
                doorJoint.connectedBody = doorRB;

                // Connect to the exact point we grabbed
                doorJoint.autoConfigureConnectedAnchor = false;

                doorJoint.connectedAnchor =
                    doorRB.transform.InverseTransformPoint(interactionPoint);

                doorJoint.anchor =
                    doorJoint.transform.InverseTransformPoint(interactionPoint);
            }
        }

        if (interactInput.action.WasReleasedThisFrame())
        {
            doorJoint.connectedBody = null;
            doorObject = null;

            currentInteractionState = InteractionState.None;
        }
    }

    private void None()
    {

    }

    void HandleInteractDetection()
    {
        RaycastHit hit;

        // Cast a ray from the camera position, straight forward
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, interactRange, doorLayer))
        {
            interactCrosshair.enabled = true;
            doorObject = hit.transform.gameObject;
            interactionPoint = hit.point;
            Debug.Log("Hit object: " + hit.transform.name);

            //Draw a temporary debug line in the Scene view
            Debug.DrawLine(fpsCam.transform.position, hit.point, Color.red, 1f);
            
            if(interactInput.action.WasPressedThisFrame())
            {
               currentInteractionState = InteractionState.InteractingWithDoor;    
            }   



        }
        else
        {
            doorObject = null;
            interactCrosshair.enabled = false;
        }
    }

    
}
