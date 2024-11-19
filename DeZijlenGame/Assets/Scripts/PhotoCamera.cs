using UnityEngine;
using UnityEngine.UI; // For working with UI elements
using System.Collections.Generic;

public class PhotoCamera : MonoBehaviour
{
    public Camera photoCamera;               // The camera used for taking photos
    public RenderTexture photoTexture;       // Render Texture to hold the photo
    public Renderer cameraScreen;            // Renderer for the camera's screen display
    public KeyCode takePhotoKey = KeyCode.Space; // Key to take a photo

    public List<Transform> targetObjects;    // List of objects to capture
    public List<string> targetDescriptions;  // Descriptions for each object
    public Text uiText;                      // UI Text to display what to capture
    public float visibleThreshold = 0.2f;    // Minimum fraction of an object that must be visible
    private int currentTargetIndex = 0;      // Index of the current target in the list

    private Texture2D capturedPhoto;         // Texture to save the captured image

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

        UpdateUIText();
    }

    void Update()
    {
        if (Input.GetKeyDown(takePhotoKey))
        {
            TakePhoto();
        }
    }

    void TakePhoto()
    {
        if (currentTargetIndex >= targetObjects.Count)
        {
            Debug.Log("All targets have been captured!");
            return;
        }

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
                uiText.text = "Je hebt overal een foto van gemaakt!";
                Invoke("ClearUIText", 3f); // Clear the text after 3 seconds
            }
        }
        else
        {
            Debug.Log("Target not in photo or obstructed. Try again!");
        }
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
                // Perform a raycast to ensure there's no obstruction
                if (!IsCornerObstructed(corner))
                {
                    visibleCorners++;
                }
            }
        }

        // Calculate the fraction of visible corners
        float visibility = (float)visibleCorners / corners.Length;

        // Check if the visibility meets the threshold
        return visibility >= visibleThreshold;
    }

    bool IsCornerObstructed(Vector3 corner)
    {
        Vector3 cameraPosition = photoCamera.transform.position;
        Vector3 direction = corner - cameraPosition;
        float distance = Vector3.Distance(cameraPosition, corner);

        // Perform a raycast from the camera to the corner
        if (Physics.Raycast(cameraPosition, direction, out RaycastHit hit, distance))
        {
            // Check if the object hit by the raycast is the target
            if (hit.transform != targetObjects[currentTargetIndex])
            {
                Debug.Log($"Obstruction detected: {hit.transform.name}");
                return true; // Corner is obstructed
            }
        }

        return false; // No obstruction
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
