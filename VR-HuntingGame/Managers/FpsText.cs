using System;
using UnityEngine;
using TMPro;

public class FpsText : MonoBehaviour {
    public TextMeshProUGUI text; // TextMeshPro metni için referans

    private float deltaTime = 0.0f;

    void Update() {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        float fps = 1.0f / deltaTime;
        text.text = "FPS: " + Math.Round(fps).ToString();
    }
}
