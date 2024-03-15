using UnityEngine;

public class BoidController : MonoBehaviour
{
    public int edible;
    private Material material;



    void Start()
    {
        GameObject child = transform.GetChild(0).gameObject;
        material =  child.GetComponent<Renderer>().material;

        if(edible == 0) // poisonous
        {
            Debug.Log("unedible");
            material.SetVector("_BaseColor", new Vector3(0.933f, 0f, 1f));
        }
        if (edible == 1) // speed buff
        {
            material.SetVector("_BaseColor", new Vector3(0, 1, 0)); 
        }
    }
}
