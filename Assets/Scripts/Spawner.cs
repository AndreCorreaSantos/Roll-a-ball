using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random=UnityEngine.Random;

public class Spawner : MonoBehaviour
{

    // walls and spawn prefab
    public GameObject prefab;
    public GameObject upper;
    public GameObject lower;
    public GameObject right;
    public GameObject left;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        while(true)
        {
            float offsetX = Random.Range(left.transform.position.x, right.transform.position.x);
            float offsetZ = Random.Range(lower.transform.position.z,upper.transform.position.z);

        
            GameObject spawned = Instantiate(prefab,transform.position,quaternion.identity);
            spawned.transform.position = new Vector3(offsetX,0.5f,offsetZ);
            yield return new WaitForSeconds(2.0f);
        }
    }
}
