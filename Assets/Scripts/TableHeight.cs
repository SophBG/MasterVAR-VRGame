using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for handling UI Sliders

public class TableHeight : MonoBehaviour {

    [Header("Configuration")]
    [Tooltip("List of objects or parent objects to move")]
    [SerializeField] private List<Transform> objectsToMove;

    [Header("Height Limits")]
    [Tooltip("Height when slider is at 0 (Bottom)")]
    [SerializeField] private float minHeight = 0.5f;

    [Tooltip("Height when slider is at 1 (Top)")]
    [SerializeField] private float maxHeight = 2.5f;

    [Header("Optional: Auto-Setup")]
    [Tooltip("Drag your UI Slider here to auto-set its value on Start")]
    [SerializeField] private Slider uiSlider;

    void Start() {
        // Optional: If a slider is assigned, set its handle to the current object height
        // so the object doesn't "jump" when you first touch the slider.
        if (uiSlider != null && objectsToMove.Count > 0) {
            float currentH = objectsToMove[0].position.y;
            // Reverse math: Calculate slider value (0-1) based on current height
            float t = Mathf.InverseLerp(minHeight, maxHeight, currentH);
            uiSlider.value = t;
        }
    }

    // --- CONNECT THIS TO THE SLIDER "ON VALUE CHANGED" EVENT ---
    public void SetHeight(float sliderValue) {
        // sliderValue comes automatically from the UI Slider (0.0 to 1.0)
        
        // Calculate the exact Y position
        float newY = Mathf.Lerp(minHeight, maxHeight, sliderValue);

        foreach (Transform obj in objectsToMove) {
            if (obj != null) {
                // Keep X and Z the same, only change Y
                Vector3 pos = obj.position;
                pos.y = newY;
                obj.position = pos;
            }
        }
    }
}