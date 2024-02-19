using UnityEngine;

public class AutoRotateToCamera : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        // MainCamera etiketine sahip kamerayı bul
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        if (mainCamera == null)
        {
            Debug.LogError("MainCamera etiketine sahip kamera bulunamadı!");
        }
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            // Objeyi kameraya döndür
            transform.LookAt(mainCamera.transform);
        }
    }
}
