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

    [Header("Map Settings")]
    public int mapWidth = 10;
    public int mapLenght = 50;
    public int mapMaxHeight = 5;

    private int[][] mapHeights;

    private GameObject mapRoot;

    [Header("Path Settings")]
    public int pathWidth = 1;

    [Header("Random Settings")]
    [Range(0f, 100f)]
    public float[] cohesionRate;
    [Range(0f, 100f)]
    public float[] elevationRate;
    [Range(0f, 100f)]
    public float[] sinkingRate;

    [Header("UnitSettings")]
    public GameObject mapUnit;
    public float unitSize = 1f;
    public float unitHeight = 0.5f;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        GenerateHeightMap();
        GenerateTerrain();
    }

    public void GenerateHeightMap()
    {
        InitializeEmptyMap();
        GeneratePathHeightMap();        
        GenerateLeftHeightMap();
        GenerateRightHeightMap();
    }

    public void InitializeEmptyMap()
    {
        mapRoot = new GameObject();
        mapRoot.name = "MapRoot";

        mapHeights = new int[mapWidth][];

        for (int i = 0; i < mapWidth; i++)
        {
            mapHeights[i] = new int[mapLenght];
        }
    }

    public void GeneratePathHeightMap()
    {
        for (int y = 0; y < mapLenght; y++)
        {
            for (int x = Mathf.FloorToInt(((float)mapWidth/2)-Mathf.FloorToInt((float)pathWidth /2)); x < Mathf.CeilToInt(((float)mapWidth /2)+ Mathf.CeilToInt((float)pathWidth /2)) ; x++)
            {
                mapHeights[x][y] = 1;
            }
        }
    }

    public void GenerateLeftHeightMap()
    {
        for (int y = 0; y < mapLenght; y++)
        {
            for (int x = Mathf.FloorToInt((float)mapWidth /2) - (Mathf.FloorToInt((float)pathWidth/2)+1); x >= 0; x--)
            {
                int heightOffset = CalculateHeightOffset(x, y);

                mapHeights[x][y] = mapHeights[x + 1][y] + heightOffset;
            }
        }
    }

    public void GenerateRightHeightMap()
    {
        for (int y = 0; y < mapLenght; y++)
        {
            for (int x = Mathf.CeilToInt((float)mapWidth /2) + Mathf.CeilToInt((float)pathWidth / 2) ; x < mapWidth; x++)
            {
                int heightOffset = CalculateHeightOffset(x, y);

                mapHeights[x][y] = mapHeights[x - 1][y] + heightOffset;
            }
        }
    }

    public int CalculateHeightOffset(int x, int y)
    {
        float slopeWidth = (mapWidth - pathWidth) / 2;
        float actualSlope = 0f;
        int result = 0;

        if (x < mapWidth/2)
        {
            actualSlope = slopeWidth - x;
        }
        else
        {
            actualSlope = (x + 1) - (mapWidth - slopeWidth);
        }

        int slopeIndex = Mathf.RoundToInt(actualSlope) > cohesionRate.Length ? cohesionRate.Length - 1 : Mathf.RoundToInt(actualSlope) - 1;

        if (y != 0 && Random.Range(0f, 100f) < cohesionRate[slopeIndex])
        {
            if (x < mapWidth/2)
            {
                result = mapHeights[x][y - 1] - mapHeights[x + 1][y];
            }
            else
            {
                result = mapHeights[x][y - 1] - mapHeights[x - 1][y];
            }
        }
        else
        {
            slopeIndex = Mathf.RoundToInt(actualSlope) > elevationRate.Length ? elevationRate.Length - 1 : Mathf.RoundToInt(actualSlope) - 1;

            if (Random.Range(0f, 100f) < elevationRate[slopeIndex]) result = 1;
            else
            {
                slopeIndex = Mathf.RoundToInt(actualSlope) > sinkingRate.Length ? sinkingRate.Length - 1 : Mathf.RoundToInt(actualSlope) - 1;

                if (Random.Range(0f, 100f) < sinkingRate[slopeIndex]) result = -1;
            }
        }        

        return result;
    }

    public void GenerateTerrain()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapLenght; y++)
            {
                GameObject actualUnit = Instantiate(mapUnit, CalculateUnitPosition(x,y), mapRoot.transform.rotation, mapRoot.transform);
                actualUnit.transform.localScale = new Vector3(unitSize, unitSize, unitSize);
            }
        }
    }

    public Vector3 CalculateUnitPosition(int x, int y)
    {
        float xPos = x * unitSize - (unitSize / 2);
        float yPos = unitHeight * mapHeights[x][y];
        float zPos = y * unitSize - (unitSize / 2);

        return new Vector3(xPos, yPos, zPos);
    }
}
