using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform bone;
    
    // Update is called once per frame
    void Update()
    {
        bone.LookAt(target);
    }
}
