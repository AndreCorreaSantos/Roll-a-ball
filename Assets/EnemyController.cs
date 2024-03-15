using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform player;
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Calculate the rotation needed to look at the player
        Quaternion lookRotation = Quaternion.LookRotation(player.position - transform.position);

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed/10f);

        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
