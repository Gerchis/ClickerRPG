using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager:MonoBehaviour
{
    public static MapManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("Multiple instances of MapManager");
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public int mapWidth = 5;
    public int mapLenght = 10;
    public int mapMaxHeight = 5;

    public int[][] mapHeights;

    public float noiseScale = 10f;
    public float noiseOffsetX = 100f;
    public float noiseOffsetY = 100f;
    public float cellSize = 1f;

    public GameObject mapRoot;
    public GameObject mapCell;

    public void Start()
    {
        SpawnMap();        
    }

    public void SpawnMap()
    {
        InitializeEmptyMap();
        GenerateNoiseMap();
        GenerateTerrain();
        Debug.Log("mapa terminado");
    }

    public void InitializeEmptyMap()
    {
        mapHeights = new int[mapWidth][];

        for (int i = 0; i < mapWidth; i++)
        {
            mapHeights[i] = new int[mapLenght];
        }
    }

    public void GenerateNoiseMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLenght; y++)
            {
                mapHeights[x][y] = CalculateHeight(x, y);
            }
        }
    }

    public int CalculateHeight(int x, int y)
    {
        float xCoord = (float)x/ (float)mapWidth * noiseScale + noiseOffsetX;
        float yCoord = (float)y / (float)mapLenght * noiseScale + noiseOffsetY;

        int result = (int)Mathf.Ceil(Mathf.PerlinNoise(xCoord,yCoord) * mapMaxHeight);

        Debug.Log(Mathf.PerlinNoise(xCoord, yCoord) * mapMaxHeight);
        return result;
    }

    public void GenerateTerrain()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLenght; y++)
            {
                Instantiate(mapCell, new Vector3(x * cellSize, mapHeights[x][y] * cellSize, y * cellSize), mapRoot.transform.rotation, mapRoot.transform);
            }
        }
    }

}
