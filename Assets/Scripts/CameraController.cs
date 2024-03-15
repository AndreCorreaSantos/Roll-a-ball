using UnityEngine;
// using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public float sensitivityX = 1.0f;
    public float sensitivityY = 1.0f;
    public Transform player; // Reference to the player's transform
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
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityY;

        player.Rotate(Vector3.up * mouseX);


        currentXRotation -= mouseY;
        currentXRotation = Mathf.Clamp(currentXRotation, minYAngle, maxYAngle);


        Quaternion rotation = Quaternion.Euler(currentXRotation, player.eulerAngles.y, 0);

        player.transform.rotation = rotation;
    }
}
