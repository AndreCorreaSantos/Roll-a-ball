using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float movementX;
    private float movementY;
    public float speed;
    public TextMeshProUGUI countText;
    public int count;
    public AudioSource hitSource;
    public AudioClip eating;
    public AudioClip damage;
    public Slider HealthBar;
    public Slider SpeedBar;
    private float timeSinceLastEat = 0f; 
    public float eatEffectCooldown = 0.4f; 

    public float health = 10f;
    public float speedBar = 0f;
    public float maxHealth = 50f;

    private float speedBoostTimer = 0;

    private Transform playerTransform;


    private Rigidbody rb; // Reference to the Rigidbody component

    void Start()
    {
        //get first child
        playerTransform = transform.GetChild(0).GetComponent<Transform>();
        count = 0;
        SetCountText();
        hitSource.playOnAwake = false; 
        health = maxHealth;
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate() 
    {
        Vector3 forward = movementY * transform.forward;
        Vector3 right = movementX * transform.right;
        Vector3 movementVector = (forward+right).normalized * speed;
        rb.velocity = movementVector;
    }

    void Update()
    {

        HealthBar.value = health / maxHealth;
        SpeedBar.value = speedBar / 10f;
        speed = 20f + (speedBoostTimer > 0 ? 20f : 0) + (speedBar > 0 && Keyboard.current.leftShiftKey.isPressed ? 30f : 0); // Calculate speed

        if (speedBoostTimer > 0)
        {
            speedBoostTimer -= Time.deltaTime;
        }

        if (health > 0)
        {
            health -= Time.deltaTime/1.5f;
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }

        if (speedBar > 0 && Keyboard.current.leftShiftKey.isPressed)
        {
            speedBar -= Time.deltaTime;
            SpeedBar.value = speedBar / 10f;
        }


        if (timeSinceLastEat < eatEffectCooldown)
        {
            timeSinceLastEat += Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;
        if (otherObject.CompareTag("PickUp"))
        {
            int edible = otherObject.GetComponentInParent<BoidController>().edible;
            otherObject.SetActive(false);
            count += 1;
            if (count >= 2048)
            {
                SceneManager.LoadScene("WinScene");
            }
            SetCountText();
            PlayEatEffect();
            speedBoostTimer = 1.5f;

            if (health < 50f && edible == 2)
            {
                health += 5f;
            }
            else if (edible == 0)
            {
                hitSource.PlayOneShot(damage, 0.5f);
                health -= 20f;
            }
            else if (edible == 1 && speedBar < 10f)
            {
                speedBar += 1.0f;//
            }
        }

        if (otherObject.CompareTag("Enemy"))
        {
            hitSource.PlayOneShot(damage, 0.5f);
            health -= 20f;
        }
    }

    void PlayEatEffect()
    {
        // Check if the cooldown has elapsed
        if (timeSinceLastEat >= eatEffectCooldown)
        {
            hitSource.PlayOneShot(eating, 0.1f);
            timeSinceLastEat = 0f; // Reset the timer
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }
}
