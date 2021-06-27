using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages the spawns of enemies
    //Controls how many enemies could be at the same time
    //Instantiate in specific points
    //Check If spawnPoint is in screen to avoid spawn near player

public class SpawnManager : MonoBehaviour
{
	///////////PUBLIC VARS/////////
    [Header("InitialConfig", order = 1)]
	[Tooltip("Initial Enemies Spawned.")]
    public int InitWavEnem = 5;
	[Tooltip("Initial time between each spawn.")]
    public float InitTimeBtwnSpawn = 3f; //Seconds between each spawn
	[Tooltip("Max enemies in game.")]
    public int maxEnemies = 50; //Max Enemies in game
	[Tooltip("Initial time between each wave.")]
    public float InitTimeBtwnWaves = 10; //Seconds between each wave
	[Tooltip("Num of waves on each group of waves.")]
    public int NumDif = 10; //Num of diferents difficulties in a wave

    [Header("Incrementals Wave", order = 2)]
	[Tooltip("Increment of enemies' velocity on each wave (NEVER RESET).")]
    public float IncEnemVelWave = 0.1f; //Velocity incremental per wave
	[Tooltip("Increment of num of enemies spawned on each wave.")]
    public int IncEnemNumWave = 1;
	[Tooltip("Increment of time between each spawn.")]
    public float IncTimeBtwnSpawn = -1;
	[Tooltip("Increment of time between each wave.")]
    public float IncTimeBtwnWaves = 3;

    [Header("Incrementals Group of Waves", order = 3)]
	[Tooltip("Increment of num of enemies spawned. Increase on change group of wave.")]
    public int IncEnemNumGroup = 1;
	[Tooltip("Increment of time between each spawn. Increase on change group of wave.")]
    public float IncTimeBtwnSpawnGroup = -1;
	[Tooltip("Increment of time between each wave. Increase on change group of wave.")]
    public float IncTimeBtwnWavesGroup = 3;

    [Header("Enemies", order = 4)]
    public GameObject[]enemies1;
    public GameObject[]enemies2;
    public GameObject[]enemies3;
    public GameObject[] enemies4;

    [Tooltip("Num of wave to start to appear a group of enemies. Pos 0 = Enemies1.")]
    public int[] waveToappear; //Num of wave which group of monsters starts to appear
	[Tooltip("Percent of total spawned of each group. Pos 0 = Enemies1.")]
    public int[] percentAppear; //% of enemies that will appear in each wave



    [Header("KeyConfig")]
    public float keySpawnCD = 10;
    public GameObject kingKey;
	///////////PRIVATE VARS/////////
	enum SpawnStates { Waiting, Spawning }
    SpawnStates actualState;
	
	//Lists
	List<GameObject[]> enemyGroups;
    List<GameObject> enemToSpawn;
    List<EnemyController> enemiesSpawned;

    //Variables
    Transform[] spawns;
    EnemyController enemScript;
	ManagerGame gm;
    GlobalVariables.CharactersTypes character;

    Transform enemySpawnRoot;  //GameObject parent of all Spawn Points.
    Transform keySpawnersRoot;

    //Counts
    float keySpawnCount = 0;

    float spawnCount = 0; //Count time between spawns
    float waveCount = 0; //Count time between wave
    int monsterWaveCount = 0; //Count monsters in a wave
    int wave = 0;
    int difficulty = 0;
    int enemiesInGame;
    float enemyIncrement = 0; //Velocity Increment

    int WavEnem;
    int num;
    GameObject enem;
    float timeBtwnSpawn, timeBtwnWaves;

	///////UNITY FUNCTIONS/////////////
	private void Awake()
    {
        gm = GetComponent<ManagerGame>();
    }
	
    private void Update()
    {
        if (gm.Playing())
        {
            switch (actualState)
            {
                case SpawnStates.Spawning:
				
                    spawnCount += Time.deltaTime;
                    if (spawnCount >= timeBtwnSpawn)
                    {
                        spawnCount = 0;
                        Spawn();
                    }
                    if (monsterWaveCount >= WavEnem)
                    {
                        FinishWave();
                    }

                    break;
					
                case SpawnStates.Waiting:
                    waveCount += Time.deltaTime;
                    if (waveCount >= timeBtwnWaves)
                    {
                        NextWave();
                    }
                    break;
            }

            if(character == GlobalVariables.CharactersTypes.King) SpawningKeys();
        }
    }
	///////PUBLIC FUNCTIONS/////////////
	
	public void StartGame(GlobalVariables.CharactersTypes car, Transform rootEnemy, Transform rootKeys) //Called by Manager(On start game)
    {
        character = car;
        keySpawnersRoot = rootKeys;
        enemySpawnRoot = rootEnemy;

        enemToSpawn = new List<GameObject>();
        enemiesSpawned = new List<EnemyController>();

        InicializeEnemyGroups();
        InicializeSpawn();

        difficulty = 1;
        wave = 1;
        gm.SetWave(wave);
   
        enemiesInGame = 0;
        ResetIncrementals();
        InitWave();
    }
	
	public void EnemyDeath(EnemyController en) //Called by Manager(on enemy death)
    {
        enemiesInGame--;
        enemiesSpawned.Remove(en);
        if(actualState == SpawnStates.Waiting && enemiesInGame<= 0)
        {
            NextWave();
        }
    }
	
    public void KillAllEnemies() //Called by Manager(on get Power Up)
    {
        for(int i=0;i<enemiesSpawned.Count;i++)
        {
            enemiesSpawned[i].Death();
        }
    }
    public void StunAllEnemies(float stunDuration) //Called by Manager(on get Power Up)
    {
        for (int i = 0; i < enemiesSpawned.Count; i++)
        {
            enemiesSpawned[i].stunnedTime = stunDuration;
            enemiesSpawned[i].Stun(true);
        }
    }

    public void NewEnemy(EnemyController en) //Called by Manager(on enemy added outside this script. EX: MiniSlime)
    {
        enemiesSpawned.Add(en);
    }

	///////PRIVATE FUNCTIONS/////////////
	
    void SpawningKeys()
    {
        keySpawnCount += Time.deltaTime;
        if(keySpawnCount > keySpawnCD)
        {
            keySpawnCount = 0;
            SpawnKey();
        }
    }
    void SpawnKey()
    {
        int rand = Random.Range(0, keySpawnersRoot.childCount);
        float x = Random.Range(-2,2);
        float z = Random.Range(-2, 2);
        Vector3 pos = keySpawnersRoot.GetChild(rand).position;

        Instantiate(kingKey, new Vector3(pos.x+x, pos.y, pos.z + z), Quaternion.identity); 
    }
	void InicializeEnemyGroups()
    {
        enemyGroups = new List<GameObject[]>();
        enemyGroups.Add(enemies1);
        enemyGroups.Add(enemies2);
        enemyGroups.Add(enemies3);
        enemyGroups.Add(enemies4);
    }
	
    void InicializeSpawn()
    {
        GetSpawns();
    }
	
	void GetSpawns()
    {
        spawns = new Transform[enemySpawnRoot.childCount];

        for (int i = 0; i < enemySpawnRoot.childCount; i++)
        {
            spawns[i] = enemySpawnRoot.GetChild(i);
        }
    }
	
	 void Spawn()
    {
        if (enemiesInGame < maxEnemies)
        {
            List<Transform> dispSpawns = AvaliableSpawns();
			
            if (dispSpawns.Count > 0)
            {
                num = Random.Range(0, dispSpawns.Count);
                enem = Instantiate(enemToSpawn[monsterWaveCount], dispSpawns[num].position, Quaternion.identity);
                
				//Increment speed
				enemScript = enem.GetComponent<EnemyController>();
                enemScript.VelIncrement(enemyIncrement);
                enemiesSpawned.Add(enemScript);

                monsterWaveCount++;
                enemiesInGame++;
            }
            else print("NO SPAWNS");
        }
    }

    void InitWave() //Config which enem have to be spawned this wave
    {
        int i, j, rand, perc, rest;
		
        actualState = SpawnStates.Spawning;
        monsterWaveCount = 0;
        enemToSpawn.Clear();
        
		//Spawn enemies depends on his percentage of the total spawneds
        for(i= enemyGroups.Count-1; i> 0;i--)
        {
            if (wave >= waveToappear[i])
            {
                perc = (int)(WavEnem * percentAppear[i] / 100);
                for (j = 0; j < perc; j++)
                {
                    rand = Random.Range(0, enemyGroups[i].Length);
                    enemToSpawn.Add(enemyGroups[i][rand]);
                }
            }
        }
		//Zombies to fill the rest of the wave
        rest = WavEnem - enemToSpawn.Count;
        for (i = 0; i < rest; i++)
        {
            rand = Random.Range(0, enemyGroups[0].Length);
            enemToSpawn.Add(enemyGroups[0][rand]);
        }
        Shuffle(enemToSpawn); //Shuffle all enemies to spawn randomly 
        
    }
	
	void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
	
	void NextWave()
    {
        wave++;
        if (wave > 10000) wave = 10000;

        gm.SetWave(wave);

        waveCount = 0;

        spawnCount = timeBtwnSpawn;

        IncreaseDifficult();
        InitWave();
    }

    void IncreaseDifficult() //If wave is compelted, increase waveInc, otherwise, increase groupInc
    {
        difficulty++;
        enemyIncrement += IncEnemVelWave;
        if (difficulty > NumDif)
        {
            difficulty = 1;
            IncreaseGroupIncrementals();
            ResetIncrementals();
        }
        else
        {
            IncreaseIncrementals();
        }
    }
	    
    void IncreaseIncrementals() //When wave is completed
    {
        WavEnem += IncEnemNumWave;
        timeBtwnSpawn += IncTimeBtwnSpawn;
        timeBtwnWaves += IncTimeBtwnWaves;
    }

	void ResetIncrementals() //When wave group is completed
    {
        WavEnem = InitWavEnem;
        timeBtwnSpawn = InitTimeBtwnSpawn;
        timeBtwnWaves = InitTimeBtwnWaves;
    }

    void IncreaseGroupIncrementals() //When wave group is completed
    {
        InitWavEnem += IncEnemNumGroup;
        InitTimeBtwnSpawn += IncTimeBtwnSpawnGroup;
        InitTimeBtwnWaves += IncTimeBtwnWavesGroup;
    }
	
    void FinishWave()
    {
        actualState = SpawnStates.Waiting;
    }

    
    List<Transform> AvaliableSpawns() //Spawns that are not on camera
    {
        List<Transform> avaliables = new List<Transform>();

        foreach (Transform f in spawns)
        {
            if (!gm.IsOnScreen(f.position)) {
                avaliables.Add(f); }
        }

        return avaliables;
    }
}
