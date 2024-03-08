using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    public float speed;

    public TextMeshProUGUI countText;

    private int count;
    void Start()
    {
        rb = GetComponent <Rigidbody>();
        count = 0;    
        SetCountText();
    }

   void OnMove (InputValue movementValue)
   {
    
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x; 
        movementY = movementVector.y; 
   }

   private void FixedUpdate()
   {
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);
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
       countText.text =  "Count: " + count.ToString();
   }

}
