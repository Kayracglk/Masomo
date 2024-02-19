using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoldierManager))]
public class FieldOfViewEditor : Editor
{
   private void OnSceneGUI()
    {
        SoldierManager fov = (SoldierManager)target;
        if (fov == null || fov.fieldOfView == null)
            return;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.config.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.config.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.config.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.config.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.config.radius);

        if (fov.fieldOfView.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.playerTransform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}