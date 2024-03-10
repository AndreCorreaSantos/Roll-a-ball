using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public BoidController boidPrefab;

    public int spawnBoids = 100;

    private List<BoidController> _boids;

    private BoidInfo[] _boidInfos;

    public ComputeShader computeShader;

    private ComputeBuffer inputBuffer;

    public Transform playerPos;

    public float separation = 0.5f;

    public float alignment = 0.5f;

    public float target = 0.5f;

    public float cohesion = 0.5f;


    struct BoidInfo
    {
        public Vector3 position;
        public Vector3 forward;
    };

    private void Start() {
        _boids = new List<BoidController>();
        _boidInfos = new BoidInfo[spawnBoids]; // Corrected initialization

        for (int i = 0; i < spawnBoids; i++) {
            SpawnBoid(boidPrefab.gameObject, i);
        }
    }

    private void Update() {
        // Ensure _boidInfos array is properly sized
        if (_boidInfos.Length != _boids.Count) {
            System.Array.Resize(ref _boidInfos, _boids.Count);
        }

        var boidInfoSize = 6 * sizeof(float);
        if (inputBuffer == null || inputBuffer.count != _boids.Count) {
            if (inputBuffer != null) {
                inputBuffer.Release();
            }
            Debug.Log("New input Buffer");
            inputBuffer = new ComputeBuffer(_boids.Count, boidInfoSize);
        }

        inputBuffer.SetData(_boidInfos);

        // Setting shader properties
        computeShader.SetInt("numBoids", _boids.Count);
        computeShader.SetFloat("deltaTime", Time.deltaTime/2f);
        computeShader.SetFloat("separationWeight", separation);
        computeShader.SetFloat("alignmentWeight", alignment);
        computeShader.SetFloat("targetWeight", target);
        computeShader.SetFloat("cohesionWeight",cohesion);
        float[] playerPosArr = new float[3] { playerPos.position.x, playerPos.position.y, playerPos.position.z };
        computeShader.SetFloats("targetPosition", playerPosArr );
        computeShader.SetFloat("moveSpeed", 20.0f);
        computeShader.SetBuffer(0, "inputBuffer", inputBuffer);

        // Adjust dispatch call to ensure all boids are processed
        int threadGroupsX = (_boids.Count + 7) / 8; // Ensure we have enough groups to cover all boids
        computeShader.Dispatch(0,threadGroupsX, 1, 1);

        inputBuffer.GetData(_boidInfos);

        // Update boids with new data from compute shader
        for (int i = 0; i < _boids.Count; i++) {
            var boid = _boids[i];
            var info = _boidInfos[i];
            boid.transform.position = info.position;
            boid.transform.forward = info.forward;
        }
    }

    private void SpawnBoid(GameObject prefab, int swarmIndex)
    {
        var boidInstance = Instantiate(prefab);
        Vector3 startPos = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        boidInstance.transform.position = startPos;
        BoidController boidController = boidInstance.GetComponent<BoidController>();
        _boids.Add(boidController);


        BoidInfo startingInfo;
        startingInfo.position = startPos;
        startingInfo.forward = boidInstance.transform.forward;
        _boidInfos[swarmIndex] = startingInfo;
    }

    private void OnDestroy() {
        if (inputBuffer != null) {
            inputBuffer.Release(); // Properly release the buffer to prevent leaks
        }
    }
}
