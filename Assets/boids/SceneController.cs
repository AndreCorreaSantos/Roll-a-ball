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

    public float noClumpingRadius = 0.5f;

    public float cohesionRadius = 5.0f;
    public float targetRadius = 5.0f;

    public int spawnRadius = 100;

    public float boxSizeY = 25f;

    public float boxSizeX = 128f;

    public float boxSizeZ = 128f;

    struct BoidInfo
    {
        public Vector3 position;
        public Vector3 forward;
    };

    private void Start() {
        _boids = new List<BoidController>();
        _boidInfos = new BoidInfo[spawnBoids]; 

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
        float timeConstant = 2f;
        float speed = 2.5f*timeConstant;
        float time = Time.deltaTime*timeConstant;
        computeShader.SetInt("numBoids", _boids.Count);
        computeShader.SetFloat("deltaTime", time);
        computeShader.SetFloat("separationWeight", separation);
        computeShader.SetFloat("alignmentWeight", alignment);
        computeShader.SetFloat("targetWeight", target);
        computeShader.SetFloat("cohesionWeight",cohesion);
        computeShader.SetFloat("noClumpingRadius",noClumpingRadius);
        computeShader.SetFloat("cohesionRadius",cohesionRadius);
        computeShader.SetFloat("targetRadius",targetRadius);
        computeShader.SetFloat("boxSizeX",boxSizeX);
        computeShader.SetFloat("boxSizeY",boxSizeY);
        computeShader.SetFloat("boxSizeZ",boxSizeZ);

        float[] playerPosArr = new float[3] { playerPos.position.x, playerPos.position.y, playerPos.position.z };
        computeShader.SetFloats("targetPosition", playerPosArr );
        computeShader.SetFloat("moveSpeed", speed);
        computeShader.SetBuffer(0, "inputBuffer", inputBuffer);

        computeShader.Dispatch(0,_boids.Count/32, 1, 1);

        inputBuffer.GetData(_boidInfos);

        // Update boids with new data from compute shader
        for (int i = 0; i < _boids.Count; i++) {
            var boid = _boids[i];
            var info = _boidInfos[i];

            // Update boid position
            boid.transform.position = info.position;
            boid.transform.forward = info.forward;
        }
    }

    private void SpawnBoid(GameObject prefab, int swarmIndex)
    {
        var boidInstance = Instantiate(prefab);
        Vector3 startPos = new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));
        boidInstance.transform.position = startPos;
        BoidController boidController = boidInstance.GetComponent<BoidController>();
        boidController.edible = 1;
        
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
