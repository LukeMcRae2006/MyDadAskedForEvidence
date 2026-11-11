using UnityEngine;

public class RotateObjectScript : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
  
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation);
    }
}
