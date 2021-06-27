using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapTerrain : MonoBehaviour
{
    public Transform rootGroups;
    public Transform playerSpawn;
    public Transform chestSpawn;
    public Transform rootEnemy;
    public Transform rootMagicStone;
    public Transform rootKey;
    public GameObject Obstacles;
    public NavMeshSurface surface;

    public Transform GetRootGroups()
    {
        return rootGroups;
    }
    public Transform GetPlayerSpawn()
    {
        return playerSpawn;
    }
    public Transform GetChestSpawn()
    {
        return chestSpawn;
    }
    public Transform GetRootEnemy()
    {
        return rootEnemy;
    }
    public Transform getRootMagicStone()
    {
        return rootMagicStone;
    }
    public Transform getRootKey()
    {
        return rootKey;
    }
    public GameObject getObstacles()
    {
        return Obstacles;
    }

    public NavMeshSurface getSurface()
    {
        return surface;
    }
}
