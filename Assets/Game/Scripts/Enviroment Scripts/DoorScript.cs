using UnityEngine;

public class DoorScript : MonoBehaviour, IInteractable, IApproachable
{
    [SerializeField] private HingeJoint hinge;
    [SerializeField] private float snapAngle = 10f;
    [SerializeField] private float snapForce = 10f;
    private bool isDoorOpen = false;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource audioSource;
    public float curVel = 0;

    private void FixedUpdate()
    {
        float angle = Mathf.Abs(hinge.angle);

        if (angle < snapAngle)
        {
            JointMotor motor = hinge.motor;
            motor.targetVelocity = -hinge.angle * 5f;
            motor.force = snapForce;

            hinge.motor = motor;
            hinge.useMotor = true;
            isDoorOpen = false;
            
        }
        else
        {
            hinge.useMotor = false;
            isDoorOpen=true;
            
        }
        RoomManager.Instance.isDoorOpen = isDoorOpen;

       
    }

    private void Update()
    {
        //handle audio
        curVel = rb.linearVelocity.magnitude;

        audioSource.pitch = Mathf.Clamp(audioSource.pitch = curVel *0.5f, 0.75f, 1.25f); 
        if (rb.linearVelocity.magnitude > 0.2f)
        {
            audioSource.enabled = true;
        }
        else
        {
            audioSource.enabled = false;
        }
    }

    public void Interact()
    {
       
        rb.AddForce(-Vector3.right * 6f, ForceMode.Impulse);
        isDoorOpen = true;
        RoomManager.Instance.isDoorOpen = isDoorOpen;
    }

    public void SetApproachDetection(bool isDetected)
    {
        RoomManager.Instance.isDoorDetected = isDetected;
    }
}