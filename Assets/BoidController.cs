using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    public int SwarmIndex { get; set; }
    public float NoClumpingRadius { get; set; }
    public float LocalAreaRadius { get; set; }
    public float Speed { get; set; }
    public float SteeringSpeed { get; set; }

    public void SimulateMovement(List<BoidController> other, float time)
    {
        // Default vars
        var steering = Vector3.zero;

        // Separation vars
        Vector3 separationDirection = Vector3.zero;
        Vector3 alignmentDirection = Vector3.zero;
        Vector3 cohesionDirection = Vector3.zero;
        int separationCount = 0;
        int alignmentCount = 0;
        int cohesionCount = 0;

        foreach (BoidController boid in other)
        {
            // Skip self
            if (boid == this)
                continue;

            var distance = Vector3.Distance(boid.transform.position, this.transform.position);

            // Identify local neighbours
            if (distance < NoClumpingRadius)
            {
                separationDirection += boid.transform.position - transform.position;
                separationCount++;
            }
            //identify local neighbour
            if (distance < LocalAreaRadius)
            {
                alignmentDirection += boid.transform.forward;
                alignmentCount++;
            }
            if (distance < LocalAreaRadius)
            {
                cohesionDirection += boid.transform.position - transform.position;
                cohesionCount++;
            }
        }

        // Calculate average separation direction
        if (separationCount > 0)
            separationDirection /= separationCount;

        // Flip and normalize
        separationDirection = -separationDirection.normalized;

        // Apply to steering
        steering += separationDirection.normalized * 0.5f;
        steering += alignmentDirection.normalized * 0.34f;
        steering += cohesionDirection.normalized * 0.16f;
        

        // Apply steering
        if (steering != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), SteeringSpeed * time);

        // Move 
        transform.position += transform.forward * Speed * time;
    }

}