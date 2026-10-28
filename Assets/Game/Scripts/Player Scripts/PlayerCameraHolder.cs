using UnityEngine;

public class PlayerCameraHolder : MonoBehaviour
{
    [SerializeField] private Transform camPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    // Update is called once per frame
    void Update()
    {
        transform.position = camPos.position;
    }
}
