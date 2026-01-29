using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Hammer : MonoBehaviour {
    
    // Internal variables to store the "Home Point" of the hammer
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        
        // Save the exact position and rotation when the scene loads
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // --- Function to be called by a UI Button or Event ---
    public void ResetToStart() {
        // 1. Teleport back to the starting point
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // 2. IMPORTANT: Reset physics inertia
        // If we skip this, the hammer will return but keep its previous momentum/velocity
        if (rb != null) {
            rb.linearVelocity = Vector3.zero;  // Stops linear movement
            rb.angularVelocity = Vector3.zero; // Stops spinning
        }
    }
}