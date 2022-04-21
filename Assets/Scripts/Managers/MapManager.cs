using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
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

    public int mapWidth = 50;
    public int mapLenght = 100;
    public int mapMaxHeight = 5;

    public int[][] mapHeights;

    public float noiseScale = 3f;
    public float noiseOffsetX = 100f;
    public float noiseOffsetY = 100f;
    public float cellSize = 1f;

    public GameObject mapRoot;
    public GameObject mapCell;

    public List<GameObject> mapElements;

    public void Start()
    {

    }

    public void SpawnMap()
    {
        ClearMap();
        RandomizeNoise();

        InitializeEmptyMap();
        GenerateNoiseMap();
        GenerateTerrain();
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
        float xCoord = (float)x / (float)mapWidth * noiseScale + noiseOffsetX;
        float yCoord = (float)y / (float)mapLenght * noiseScale + noiseOffsetY;

        int result = (int)Mathf.Ceil(Mathf.PerlinNoise(xCoord, yCoord) * mapMaxHeight);

        return result;
    }

    public void GenerateTerrain()
    {
        mapElements = new List<GameObject>();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLenght; y++)
            {
                mapElements.Add(Instantiate(mapCell, new Vector3(x * cellSize, mapHeights[x][y] * cellSize, y * cellSize), mapRoot.transform.rotation, mapRoot.transform));
            }
        }
    }

    public void ClearMap()
    {
        if (mapElements.Count <= 0) return;

        foreach (GameObject element in mapElements)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(element);
            }
            else if (Application.isPlaying)
            {
                Destroy(element);
            }            
        }

        mapElements.Clear();
    }

    public void RandomizeNoise()
    {
        noiseOffsetX = Random.Range(0, 100000);
        noiseOffsetY = noiseOffsetX;
    }
}
