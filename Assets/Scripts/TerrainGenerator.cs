using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{
    public int depth = 20;
    public int width = 256;
    public int height = 256;

    public float scale = 20f;

    public float steepness = 10f;

    void Start()
    {
        Terrain terrain = GetComponent<Terrain> ();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width+1;
        terrainData.size = new Vector3(width,depth,height);
        terrainData.SetHeights(0,0,GenerateHeights());
        return terrainData;
    }
    private float[,] GenerateHeights()
    {
        float [,] heights = new float[width,height];
        for (int x = 0; x < width; x++)
        {
            for( int y = 0; y < height; y++)
            {
                heights[x,y] = CalculateHeight(x,y);
            }
        }
        return heights;
    }
    private float CalculateHeight(int x, int y)
    {

        
        //perlin logic

        float xCoord = (float) x/width ;
        float yCoord = (float) y/height ;

        return  Mathf.PerlinNoise(xCoord*scale,yCoord*scale);

    }

}
