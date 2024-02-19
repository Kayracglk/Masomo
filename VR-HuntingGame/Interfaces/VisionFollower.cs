using UnityEngine;

public class VisionFollower : MonoBehaviour
{
    private Transform mainCameraTransform;

    private float distance = 1.0f;
    private float height = -0.5f; // New variable for height adjustment
    private bool isCentered = false;

    private void Start()
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        if (mainCamera != null)
        {
            mainCameraTransform = mainCamera.transform;
        }
        else
        {
            Debug.LogError("Kamera bulunamadı.");
        }
    }

    private void OnBecameInvisible()
    {
        isCentered = false;
    }

    private void Update()
    {
        if (!isCentered && mainCameraTransform != null)
        {
            Vector3 targetPosition = FindTargetPosition();

            LookAtCamera();

            MoveTowards(targetPosition);

            if (ReachedPosition(targetPosition))
                isCentered = true;
        }
    }

    private Vector3 FindTargetPosition()
    {
        // Adjust the height in the Y-axis based on the object's distance from the camera
        Vector3 heightAdjustment = mainCameraTransform.up * height;
        return mainCameraTransform.position + (mainCameraTransform.forward * distance) + heightAdjustment;
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        transform.position += (targetPosition - transform.position) * 0.05f;
    }

    private bool ReachedPosition(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, transform.position) < 0.1f;
    }

    private void LookAtCamera()
    {
        Vector3 lookPos = transform.position - mainCameraTransform.position;
        Quaternion rotation = Quaternion.LookRotation(lookPos);

        Vector3 euler = rotation.eulerAngles;
        euler.x = 0f;
        euler.z = 0f;
        rotation = Quaternion.Euler(euler);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5.0f);
    }
}
