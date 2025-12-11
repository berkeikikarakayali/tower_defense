using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Stats")]
    public float moveSpeed = 30f; //how fast the camera moves wasd
    public float zoomSpeed = 5f; //how fast we zoom

    [Header("Limits")]
    public float minY = 10f;
    public float maxY = 80f;
    public float minX = 0f;
    public float maxX = 70f;
    public float minZ = 0f;
    public float maxZ = 70f;

    public bool canMove = true; //we can turn off while interacting UI

    void Update()
    {
        if(!canMove) return;
        Vector3 camPosition = transform.position; //current position of the camera

        if (Input.GetKey("w"))
        {
            camPosition.z += moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s"))
        {
            camPosition.z -= moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d"))
        {
            camPosition.x += moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            camPosition.x -= moveSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        camPosition.y -= scroll * 1000 * zoomSpeed * Time.deltaTime; //scroll up is positive so go down, decrease - scroll down the other way


        camPosition.x = Mathf.Clamp(camPosition.x, minX, maxX);
        camPosition.y = Mathf.Clamp(camPosition.y, minY, maxY); // Limit height
        camPosition.z = Mathf.Clamp(camPosition.z, minZ, maxZ);
        transform.position = camPosition;
    }
}
