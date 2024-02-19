using UnityEngine;
using BNG;
using TMPro;

public class CameraZoom : MonoBehaviour
{
    public Camera mainCamera;
    public TextMeshProUGUI zoomText;

    private float minZoom = 1.0f;
    private float maxZoom = 80.0f;
    private float zoomStep = 30.0f;
    private float currentZoom = 10.0f;

    public PlayerRotation playerRotation; // 

    private void Start()
    {
        
        GameObject playerObject = GameObject.FindWithTag("Player"); 

       
        if (playerObject != null)
        {
            playerRotation = playerObject.GetComponent<PlayerRotation>();
        }
        else
        {
            Debug.LogError("PlayerRotation bileşeni bulunamadı. Bir Player Layer GameObject'i olduğundan emin olun.");
        }
    }

    private void Update()
    {
        if (playerRotation == null)
        {
            return; // PlayerRotation bileşeni yoksa işlem yapma
        }

        float yAxisInput = playerRotation.GetAxisInputY();

        if (yAxisInput > 0 && currentZoom > minZoom)
        {
            currentZoom -= zoomStep * Time.deltaTime; // Kademe kademe yakınlaştırma
        }
        else if (yAxisInput < 0 && currentZoom < maxZoom)
        {
            currentZoom += zoomStep * Time.deltaTime; // Kademe kademe uzaklaştırma
        }

        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        mainCamera.fieldOfView = currentZoom;

        int zoomFactor = (int)((maxZoom - currentZoom) / zoomStep) + 1;
        zoomText.text = zoomFactor + "x";
    }


}
