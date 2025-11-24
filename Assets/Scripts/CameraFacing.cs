using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    private Camera camera;
    void Start()
    {
        camera = Camera.main;
    }
    void LateUpdate()
    {
        //To make enemy health bar canvas face main camera
        if (camera != null) // Check if camera exists
        {
            // Calculate the position the object we want to face
            // We take our position and add the camera's forward direction
            Vector3 targetPosition = transform.position + camera.transform.rotation * Vector3.forward;
            
            // Get the camera's up vector
            Vector3 cameraUp = camera.transform.rotation * Vector3.up;
            
            // Make the object look at the target position
            transform.LookAt(targetPosition, cameraUp);
        }
    }
}