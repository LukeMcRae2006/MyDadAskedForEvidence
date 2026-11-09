using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private HingeJoint hinge;
    [SerializeField] private float snapAngle = 10f;
    [SerializeField] private float snapForce = 10f;
    private bool isDoorOpen = false;

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
}