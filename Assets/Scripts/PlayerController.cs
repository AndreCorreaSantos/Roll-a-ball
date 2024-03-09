using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private float movementX;
    private float movementY;

    public float speed;

    public TextMeshProUGUI countText;

    private int count;

    void Start()
    {
        count = 0;
        SetCountText();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void Update()
    {
        Vector3 movementVector = (transform.forward * movementY + transform.right * movementX).normalized * speed * Time.deltaTime;
        transform.position += movementVector;

        // Move up with Spacebar
        if (Keyboard.current.spaceKey.isPressed)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        // Move down with Ctrl
        else if (Keyboard.current.ctrlKey.isPressed)
        {
            transform.position -= Vector3.up * speed * Time.deltaTime;
        }

        // Limit the character's absolute speed
        float characterSpeed = Mathf.Clamp(transform.position.magnitude, 0f, speed);
        transform.position = transform.position.normalized * characterSpeed;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }
}
