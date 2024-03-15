using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    private float movementX;
    private float movementY;
    public float speed;
    public TextMeshProUGUI countText;
    public int count;
    public AudioSource hitSource;
    public AudioClip eating;

    public Slider HealthBar;

    public Slider SpeedBar;
    private float timeSinceLastEat = 0f; 
    public float eatEffectCooldown = 0.4f; 

    public float health = 10f;

    public float speedBar = 0f;

    private float speedBoostTimer = 0;

    public Transform cameraPos;

    private Vector3 cameraStartPos;

    void Start()
    {
        count = 0;
        SetCountText();
        hitSource.playOnAwake = false; 
        cameraStartPos = cameraPos.localPosition;
    }

   void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void Update()
    {
        HealthBar.value = health/10f;
        SpeedBar.value = speedBar/10f;
        speed = 20f;
        if (speedBoostTimer > 0)
        {
            speedBoostTimer -= Time.deltaTime;
            speed = 40f;
        }
        if (health > 0)
        {
            health -= Time.deltaTime;
            
        }else
        {
            SceneManager.LoadScene("MainMenu");
             
        }
        Debug.Log(speedBar);
        if (speedBar > 0 && Keyboard.current.leftShiftKey.isPressed) //check if there is speed bar and it was pressed, shift
        {
            Debug.Log("SPEED");
            speedBar -= Time.deltaTime;
            SpeedBar.value = speedBar/10f;
            speed = 50f;
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

            if (health < 10f && edible == 2)
            {
                health += 0.5f;
            }
            else if (edible == 0)
            {
                health -= 3f;
            }
            else if (edible == 1 && speedBar < 10f)
            {
                speedBar += 1.5f;//
            }
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
