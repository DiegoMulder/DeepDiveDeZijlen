using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using System.Collections.Generic;

public class PhotoCamera : MonoBehaviour
{
    public Camera photoCamera;               // The camera used for taking photos
    public RenderTexture photoTexture;       // Render Texture to hold the photo
    public Renderer cameraScreen;            // Renderer for the camera's screen display

    public List<Transform> targetObjects;    // List of objects to capture
    public List<string> targetDescriptions;  // Descriptions for each object
    public Text uiText;                      // UI Text to display what to capture
    public float visibleThreshold = 0.2f;    // Minimum fraction of an object that must be visible
    private int currentTargetIndex = 0;      // Index of the current target in the list

    private Texture2D capturedPhoto;         // Texture to save the captured image
    private InputDevice rightController;     // Reference to the right-hand controller
    private bool canTakePhoto = true;        // Cooldown flag to control photo-taking

    void Start()
    {
        if (photoCamera == null)
        {
            Debug.LogError("Photo camera not assigned!");
        }
        if (cameraScreen == null)
        {
            Debug.LogError("Camera screen renderer not assigned!");
        }
        if (targetObjects == null || targetObjects.Count == 0)
        {
            Debug.LogError("No target objects assigned!");
        }
        if (uiText == null)
        {
            Debug.LogError("UI Text not assigned!");
        }

        // Find the right-hand controller
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, devices);

        if (devices.Count > 0)
        {
            rightController = devices[0];
            Debug.Log("Right controller found.");
        }
        else
        {
            Debug.LogError("No right controller found!");
        }

        UpdateUIText();
    }

    void Update()
    {
        if (rightController.isValid)
        {
            // Check if the trigger button is pressed
            if (rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed) && isPressed && canTakePhoto)
            {
                TakePhoto();
            }
        }
        else
        {
            // Try to reinitialize the controller if it's lost
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, devices);

            if (devices.Count > 0)
            {
                rightController = devices[0];
            }
        }
    }

    void TakePhoto()
    {
        if (currentTargetIndex >= targetObjects.Count)
        {
            Debug.Log("All targets have been captured!");
            return;
        }

        // Start cooldown
        canTakePhoto = false;
        Invoke(nameof(ResetPhotoCooldown), 1f);

        // Save the current RenderTexture to a Texture2D
        RenderTexture.active = photoTexture;
        capturedPhoto = new Texture2D(photoTexture.width, photoTexture.height, TextureFormat.RGB24, false);
        capturedPhoto.ReadPixels(new Rect(0, 0, photoTexture.width, photoTexture.height), 0, 0);
        capturedPhoto.Apply();
        RenderTexture.active = null;

        // Display the captured photo on the camera's screen
        cameraScreen.material.mainTexture = capturedPhoto;

        Debug.Log("Photo taken!");

        // Check if the current target object is in the frame
        if (IsTargetPartiallyInPhoto(targetObjects[currentTargetIndex]))
        {
            Debug.Log($"Target {currentTargetIndex + 1} captured!");
            currentTargetIndex++; // Move to the next target

            if (currentTargetIndex < targetObjects.Count)
            {
                UpdateUIText();
            }
            else
            {
                // All targets captured
                uiText.text = "Je hebt van alles een foto gemaakt!";
                Invoke("ClearUIText", 3f); // Clear the text after 3 seconds
            }
        }
        else
        {
            Debug.Log("Target not in photo or obstructed. Try again!");
        }
    }

    void ResetPhotoCooldown()
    {
        canTakePhoto = true;
    }

    bool IsTargetPartiallyInPhoto(Transform targetObject)
    {
        Renderer targetRenderer = targetObject.GetComponent<Renderer>();
        if (targetRenderer == null)
        {
            Debug.LogWarning($"Target object {targetObject.name} does not have a Renderer component!");
            return false;
        }

        Bounds bounds = targetRenderer.bounds;

        // Get all 8 corners of the bounding box
        Vector3[] corners = new Vector3[8];
        corners[0] = bounds.min;
        corners[1] = bounds.max;
        corners[2] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        corners[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        corners[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        corners[5] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        corners[6] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        corners[7] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);

        int visibleCorners = 0;

        // Check each corner to see if it's visible in the photo camera's viewport
        foreach (Vector3 corner in corners)
        {
            Vector3 viewportPosition = photoCamera.WorldToViewportPoint(corner);

            // A corner is visible if it is in front of the camera and within the viewport bounds
            if (viewportPosition.z > 0 && // In front of the camera
                viewportPosition.x > 0 && viewportPosition.x < 1 && // Horizontal bounds
                viewportPosition.y > 0 && viewportPosition.y < 1)   // Vertical bounds
            {
                visibleCorners++;
            }
        }

        // Calculate the fraction of visible corners
        float visibility = (float)visibleCorners / corners.Length;

        // Check if the visibility meets the threshold
        return visibility >= visibleThreshold;
    }

    void UpdateUIText()
    {
        if (currentTargetIndex < targetDescriptions.Count)
        {
            uiText.text = $"Maak een foto van: {targetDescriptions[currentTargetIndex]}";
        }
        else
        {
            uiText.text = "";
        }
    }

    void ClearUIText()
    {
        uiText.text = "";
    }
}
