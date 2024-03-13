using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivityX = 1.0f;
    public float sensitivityY = 1.0f;
    public Transform player; // Reference to the player's transform
    public Transform cameraPos; // Reference to the camera position and rotation relative to the player
    public float minYAngle = -90f; // Minimum angle for looking down
    public float maxYAngle = 90f; // Maximum angle for looking up

    float currentXRotation = 0f;
    Vector3 cameraOffset;
    Quaternion cameraRotationOffset;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Calculate the constant offset between player and cameraPos
        if (player != null && cameraPos != null)
        {
            cameraOffset = cameraPos.position - player.position;
            cameraRotationOffset = Quaternion.Inverse(player.rotation) * cameraPos.rotation;
        }
        else
        {
            Debug.LogError("Player or cameraPos transform is not assigned!");
        }
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

        // Calculate position based on rotation and constant offset
        Vector3 position = player.position + rotation * cameraOffset*player.localScale.x;

        // Set camera position and rotation
        transform.rotation = rotation * cameraRotationOffset;
        transform.position = position;

        // Rotate the player according to the camera's rotation
        player.transform.rotation = rotation;
    }
}
