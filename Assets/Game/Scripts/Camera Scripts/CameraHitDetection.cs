using UnityEngine;

public class CameraHitDetection : MonoBehaviour
{
    public bool isCreatureInCamera = false;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Creature"))
        {
            isCreatureInCamera = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Creature"))
        {
            isCreatureInCamera = false; 
        }
    }


}
