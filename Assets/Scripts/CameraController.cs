using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivityX = 1.0f;
    public float sensitivityY = 1.0f;
    public Transform player; // Reference to the player's transform
    public float distanceFromPlayer = 10.0f; // Distance between camera and player
    public float minYAngle = -90f; // Minimum angle for looking down
    public float maxYAngle = 90f; // Maximum angle for looking up

    float currentXRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityY;

        // Rotate the player horizontally
        player.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically
        currentXRotation -= mouseY;
        currentXRotation = Mathf.Clamp(currentXRotation, minYAngle, maxYAngle);

        // Calculate rotation based on currentXRotation and player's rotation
        Quaternion rotation = Quaternion.Euler(currentXRotation, player.eulerAngles.y, 0);

        // Calculate position based on rotation and distance from player
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distanceFromPlayer);
        Vector3 position = rotation * negDistance + player.position;

        // Set camera position and rotation
        transform.rotation = rotation;
        transform.position = position;


        //rotating the player according to the camera's rotation

        player.transform.rotation = rotation;


    }
}
