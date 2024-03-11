using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float movementX;
    private float movementY;
    public float speed;
    public TextMeshProUGUI countText;
    private int count;
    public AudioSource hitSource;
    public AudioClip eating;

    public Slider HealthBar;
    private float timeSinceLastEat = 0f; 
    public float eatEffectCooldown = 0.4f; 

    public float health = 10f;

    private float eatTimer = 0;

    void Start()
    {
        count = 0;
        SetCountText();
        hitSource.playOnAwake = false; 
    }

   void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void Update()
    {
        if (eatTimer > 0)
        {
            eatTimer -= Time.deltaTime;
            speed = 40f;
        } else
        {
            speed = 20f;
        }
        if (health > 0)
        {
            health -= Time.deltaTime;
            HealthBar.value = health/10f;
        }
        else
        {
            Debug.Log("You died!");
            // go to menu
             
        }


        Vector3 movementVector = (transform.forward * movementY + transform.right * movementX) * speed * Time.deltaTime;

        if (Keyboard.current.spaceKey.isPressed)
        {
            movementVector += Vector3.up * speed * Time.deltaTime;
        }
        else if (Keyboard.current.ctrlKey.isPressed)
        {
            movementVector -= Vector3.up * speed * Time.deltaTime;
        }

        transform.position += movementVector;

        // Update the timer
        if (timeSinceLastEat < eatEffectCooldown)
        {
            timeSinceLastEat += Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
            PlayEatEffect();
            eatTimer = 1.5f;
            if (health < 10f)
            {
                health += 0.5f;
            }
        }
    }

    void PlayEatEffect()
    {
        // Check if the cooldown has elapsed
        if (timeSinceLastEat >= eatEffectCooldown)
        {
            hitSource.PlayOneShot(eating, 0.5f);
            timeSinceLastEat = 0f; // Reset the timer
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }
}
