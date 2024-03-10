using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{

    public void SimulateMovement(List<BoidController> other, float time, Vector3 steering, float Speed)
    {
        transform.position += transform.forward * Speed * time;
    }

}