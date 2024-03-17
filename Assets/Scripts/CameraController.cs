using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivityX = 1.0f;
    public float sensitivityY = 1.0f;
    public Transform player; // Reference to the player's transform
    public float minYAngle = -90f; // Min lookdown angle
    public float maxYAngle = 90f; // Max lookup angle

    float xRotation = 0f;

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
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minYAngle, maxYAngle);


        Quaternion fullRotation = Quaternion.Euler(xRotation, player.eulerAngles.y, 0);

        player.transform.rotation = fullRotation;
    }
}
