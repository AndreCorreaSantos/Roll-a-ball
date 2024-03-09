using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public BoidController boidPrefab;

    public int spawnBoids = 100;

    private List<BoidController> _boids;

    private void Start()
    {
        _boids = new List<BoidController>();

        for (int i = 0; i < spawnBoids; i++)
        {
            SpawnBoid(boidPrefab.gameObject, 0);
        }
    }

    private void Update()
    {
        foreach (BoidController boid in _boids)
        {
            boid.SimulateMovement(_boids, Time.deltaTime);
        }
    }

    private void SpawnBoid(GameObject prefab, int swarmIndex)
    {
        var boidInstance = Instantiate(prefab);
        boidInstance.transform.localPosition += new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        BoidController boidController = boidInstance.GetComponent<BoidController>();
        _boids.Add(boidController);
        if (boidController != null)
        {
            boidController.Speed = 5.0f; // Set Speed to a random value within a specified range
            boidController.SteeringSpeed = 100f;
            boidController.NoClumpingRadius = 5f;
            boidController.LocalAreaRadius = 10f;
        }
    }
}