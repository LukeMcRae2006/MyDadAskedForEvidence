using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PlayerLook))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        PlayerLook fow = (PlayerLook)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.directionFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.directionFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        
    }
}
