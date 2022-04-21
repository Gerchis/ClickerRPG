using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapSpawner : MonoBehaviour
{
    public bool newMap = false;
    public bool clearMap = false;

    void Update()
    {
        if (newMap)
        {
            MapManager.instance.SpawnMap();
            newMap = false;
        }

        if (clearMap)
        {
            MapManager.instance.ClearMap();
            clearMap = false;
        }
    }
}
