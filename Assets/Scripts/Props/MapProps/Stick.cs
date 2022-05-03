using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour,IMapProps
{
    [Range(0f, 100f)]
    public float[] spawnRate;

    public bool CheckSpawnChance(int pathIndex)
    {
        return true;
    }
}
