using UnityEngine;

public class Door : MonoBehaviour
{
    // Call this function via a UI Button
    public void QuitGame() {
        // Logs a message to the console to confirm it's working
        Debug.Log("Quitting Game...");

        // Preprocessor directive: check if we are running in the Unity Editor
        #if UNITY_EDITOR
            // Stop play mode in the Editor
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Close the application if running as a standalone build
            Application.Quit();
        #endif
    }
}
