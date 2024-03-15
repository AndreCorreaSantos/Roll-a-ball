using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;

    private PlayerController pc;
    public float speed;
    void Start()
    {
        pc = player.GetComponent<PlayerController>();   
    }

    // Update is called once per frame
    void Update()
    {
        float newSpeed = speed+(10f*pc.count/2048f);
        Quaternion lookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * newSpeed/10f);
        transform.Translate(Vector3.forward * newSpeed * Time.deltaTime);
    }
}
