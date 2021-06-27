using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Instantiate all gameobject needed to playerScript
//////Traps randomly
//////Player, Chest

public class ObjectsManager : MonoBehaviour
{
    ///////////PUBLIC VARS/////////

    [Header("MapTerrain")]
    public GameObject[] terrain;


    [Header("MapGroups")]
    public GameObject[] mapGrups;

    [Header("Prefabs")]
    public GameObject chest;
    public GameObject kingChest;
    public GameObject[] characters;
    public GameObject magicStone;

    [Header("Traps")]
    public GameObject[] trapsMiddle; //Middle traps
    public GameObject[] trapsSide; //Side traps


    [Header("PROVISIONAL")]
    public GameObject terrainProv;
    public bool provisional = true;

	///////////PRIVATE VARS/////////
    Chest chestScript;
    PlayerController playerScript;
    GlobalVariables.CharactersTypes classTried;

    List <Traps> trapList;
    List<MagicStone> magicStonesList;
    List<GameObject[]> trapTypes;

    //MAP GENERATOR
    MapTerrain map;
    Transform rootKeys;
    Transform rootEnemies;
    Transform chestSpawn;
    Transform playerSpawn;
    Transform mapSpawns;
    Transform rootSpawnMagicStone; //Gollem config
    GameObject obstaclesRoot;

    NavMeshSurface surface;

    //vars
    int rand;
    float randFloat;
    GameObject obj;
	
	
	/////////UNITY FUNCTIONS//////////////
	private void Awake()
    {
        trapTypes = new List<GameObject[]>();
        trapList = new List<Traps>();
        magicStonesList = new List<MagicStone>();

        trapTypes.Add(trapsMiddle);
        trapTypes.Add(trapsSide);
    }
	/////////PUBLIC FUNCTIONS//////////////
	public void SpawnWorld() //Called by Manager(on configure game)
    {
        classTried = ManagerGame.Instance.GetClassTried();
        //Spawns
        SpawnTerrain();
        SpawnGroupMap();
        SpawnPlayer();
        SpawnChest();

        if (classTried == GlobalVariables.CharactersTypes.Gollem)
            SpawnMagicStones();
    }

	
	///////Gets called by Manager(on configure game)
    public Chest GetChestScript()
    {
        return chestScript;
    }
    public PlayerController GetPlayerController()
    {
        return playerScript;
    }
	
	public List<Traps> GetTrapList()
    {
        return trapList;
    }

    public List<MagicStone> GetMagicStonesList()
    {
        return magicStonesList;
    }
    public Transform GetKeyRoot()
    {
        return rootKeys;
    }
    public Transform GetEnemyRoot()
    {
        return rootEnemies;
    }
    public NavMeshSurface GetSurface()
    {
        return surface;
    }

    /////////PRIVATE FUNCTIONS//////////////

    void SpawnTerrain()
    {
        rand = Random.Range(0, terrain.Length);

        if (!provisional)
            obj = Instantiate(terrain[rand], Vector3.zero, Quaternion.identity);
        else
        {
            obj = terrainProv;
        }
        map = obj.GetComponent<MapTerrain>();

        rootKeys = map.getRootKey();
        rootEnemies = map.GetRootEnemy();
        rootSpawnMagicStone = map.getRootMagicStone();
        playerSpawn = map.GetPlayerSpawn();
        chestSpawn = map.GetChestSpawn();
        mapSpawns = map.GetRootGroups();
        obstaclesRoot = map.getObstacles();
        surface = map.getSurface();
    }

    void SpawnGroupMap()
    {
        Shuffle(mapGrups);
        for (int i=0;i<mapSpawns.childCount;i++)
        {
            randFloat = Random.Range(0, 360);
            obj =Instantiate(mapGrups[i], mapSpawns.GetChild(i).position, 
                    transform.rotation * Quaternion.Euler(0,randFloat,0), obstaclesRoot.transform);
            SpawnTraps(obj.GetComponent <MapGroup>().GetRootTraps());
        }
    }

    void Shuffle(GameObject[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Length);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void SpawnPlayer()
    {
        GameObject obj = Instantiate(characters[(int)classTried], playerSpawn.position, chestSpawn.rotation);
        playerScript = obj.GetComponent<PlayerController>();
    }
	
    void SpawnChest() 
    {
        GameObject obj;

        if (classTried == GlobalVariables.CharactersTypes.King)
			{ obj = Instantiate(kingChest, chestSpawn.position, chestSpawn.rotation); }
        else
			{ obj = Instantiate(chest, chestSpawn.position, chestSpawn.rotation); }

        chestScript = obj.GetComponent<Chest>();
    }

    void SpawnMagicStones()
    {
        for (int i = 0; i < rootSpawnMagicStone.childCount; i++)
        {
            obj = Instantiate(magicStone,rootSpawnMagicStone.GetChild(i).transform.position, rootSpawnMagicStone.GetChild(i).transform.rotation);
            magicStonesList.Add(obj.GetComponent<MagicStone>());
        }
    }

    ///Spawns traps depend on the type of trap. 
    ////If there is a group os spawns, spawn only in one of them choosed randomly.
    ////Otherwise, is just spawned 
    void SpawnTraps(Transform rootSpawnTrap)
    {
        Transform spawnType, spawnGroup;
        for (int type = 0; type < rootSpawnTrap.childCount; type++) //Types of traps(lateral, middle,..)
        {
            spawnType = rootSpawnTrap.GetChild(type);
            for (int group = 0; group < spawnType.childCount; group++) //Types of groups of spawns
            {
                spawnGroup = spawnType.GetChild(group);
				
                if(spawnGroup.childCount > 0) //Is a group (randomize spawn)
                {
                    rand = Random.Range(0, spawnGroup.childCount);
                    InstantiateTrap(spawnGroup.GetChild(rand), type);
                }
                else //Is not a group
                {
                    InstantiateTrap(spawnGroup, type);
                }
            }
        }
    }

    void InstantiateTrap(Transform spawn, int type)
    {

        rand = Random.Range(0, trapTypes[type].Length);
        obj = Instantiate(trapTypes[type][rand], spawn.position, spawn.rotation, spawn);
        trapList.Add(obj.GetComponent<Traps>());
    }
}
