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
    // Calculate movement vector based on input
    Vector3 movementVector = (transform.forward * movementY + transform.right * movementX) * speed * Time.deltaTime;

    // Move up with Spacebar
    if (Keyboard.current.spaceKey.isPressed)
    {
        movementVector += Vector3.up * speed * Time.deltaTime;
    }
    // Move down with Ctrl
    else if (Keyboard.current.ctrlKey.isPressed)
    {
        movementVector -= Vector3.up * speed * Time.deltaTime;
    }

    // Apply movement
    transform.position += movementVector;
}



    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
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
