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

    public void SpawnMap()
    {

    }

}
