using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{

    public void SimulateMovement(List<BoidController> other, float time, Vector3 steering, float Speed) //maybe do raycast stuff here,
    {
        transform.position += transform.forward * Speed * time;
    }

}