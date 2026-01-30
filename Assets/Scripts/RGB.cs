using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class RGBController : MonoBehaviour
{
    [Header("3D Objects")]
    [SerializeField] private List<Renderer> lights;

    [Header("Settings")]
    [SerializeField] private float maxIntensity = 5f;

    [Header("UI References - Sliders")]
    [SerializeField] private Slider hueSlider;
    
    [Header("UI References - Visuals")]
    [SerializeField] private Image hueHandleImage;      // Color Knob
    [SerializeField] private Image intensityFillImage;  // Intensity Bar
    [SerializeField] private Image speedFillImage;      // Speed Bar (Optional)
    
    [Header("UI References - Toggle")]
    [SerializeField] private Image toggleGraphicImage;  // The background or checkmark of the toggle

    // Control variables
    private float speed = 0.1f;
    private float currentHue = 0.4f;
    private float currentIntensity = 0.5f;
    private bool isAutoCycle = true;

    private void Update()
    {
        // 1. Auto Cycle Logic
        if (isAutoCycle)
        {
            currentHue += Time.deltaTime * speed;
            if (currentHue > 1f) currentHue -= 1f;

            if (hueSlider != null)
            {
                hueSlider.value = currentHue;
            }
        }

        // 2. Apply colors
        UpdateColorsAndUI();
    }

    private void UpdateColorsAndUI()
    {
        // Calculate Base and HDR Colors
        Color pureColor = Color.HSVToRGB(currentHue, 1f, 1f);
        Color emissionColor = pureColor * currentIntensity;

        // --- UPDATE 3D OBJECTS ---
        for (int i = 0; i < lights.Count; i++)
        {
            if (lights[i] != null)
            {
                lights[i].material.color = pureColor;
                lights[i].material.SetColor("_EmissionColor", emissionColor);
                DynamicGI.SetEmissive(lights[i], emissionColor);
            }
        }

        // --- UPDATE UI VISUALS ---
        
        // 1. Sliders
        if (hueHandleImage != null) hueHandleImage.color = pureColor;
        
        if (intensityFillImage != null)
        {
            Color uiColor = pureColor;
            float uiAlpha = Mathf.Clamp(currentIntensity / maxIntensity, 0.2f, 1f);
            uiColor.a = uiAlpha;
            intensityFillImage.color = uiColor;
        }

        if (speedFillImage != null) speedFillImage.color = pureColor;

        // 2. Toggle
        if (toggleGraphicImage != null) toggleGraphicImage.color = pureColor;
    }

    // --- PUBLIC METHODS ---

    public void SetIntensity(float val) => currentIntensity = val*maxIntensity;
    public void SetSpeed(float val) => speed = val;
    
    public void SetHue(float val)
    {
        if (!isAutoCycle) currentHue = val;
    }

    public void ToggleAutoCycle(bool isOn)
    {
        isAutoCycle = isOn;
    }
}