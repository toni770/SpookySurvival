using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Manages the game (Singleton)
//Controls when the game starts and ends
//Connects with Spawn and Ui Manager
public class ManagerGame : MonoBehaviour
{
	///////////PUBLIC VARS/////////
	
	[Header("Config")]
    public GlobalVariables.CharactersTypes classTried;
    public float timeResetCombo = 1;
    public bl_Joystick joystick;
	
	[Header("JesterConfig")]
    public float extraCameraSize = 2;

    ///////////PRIVATE VARS/////////

    public bool proves;
    //Variables
    float gameCount = 0;
    int wave;
    int money = 0;
    bool isPlaying;

	//Combo
    int enemKillCombo = 0;
    int lastEnemKillCombo = 0;
    bool resetCombo = false;
    float countCombo = 0;

	//Managers
	SpawnManager spMan;
    ObjectsManager objMan;
    PowerUpManager puMan;
	
	//Scripts
    CameraFollow cameraScript;
    Chest chestScript;
    PlayerController playerScript;

    Camera cam;

    List<Traps> trapList;
    List<MagicStone> magicStoneList;
    List<Plant> plantList;

    Transform keyRootSpawn;
    Transform enemyRootSpawn;

    NavMeshBaking navAdmin;
    NavMeshSurface surface;

    //Singleton
    private static ManagerGame _instance;
    public static ManagerGame Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("ManagerGame is NULL");

            return _instance;
        }
    }

	//////////UNITY FUNCTIONS//////////
    void Awake()
    {
        SaveSystem.LoadData();

        trapList = new List<Traps>();
        plantList = new List<Plant>();

        _instance = this;
        spMan = GetComponent < SpawnManager >();
        objMan = GetComponent<ObjectsManager>();
        puMan = GetComponent<PowerUpManager>();
        navAdmin = GetComponent<NavMeshBaking>();

        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //To select character directly
        SaveSystem.SelectCharacter = true;
    }

    private void Start()
    {
        StartGame();
    }
	
	 void Update() ////Controls combo(enemies killed at a short time)
    {
        if (isPlaying)
        {
            gameCount += Time.deltaTime; //Time in game
            if(!resetCombo)
            {
                countCombo += Time.deltaTime;
                if(countCombo>= timeResetCombo)
                {
                    countCombo = 0;
                    lastEnemKillCombo = enemKillCombo;
                    enemKillCombo = 0;
					
                    UIManager.Instance.UpdateCombo(enemKillCombo.ToString());
                    resetCombo = true;
                }
            }
        }
    }
    //////////PUBLIC FUNCTIONS//////////

    //CharacterFunctions
    //Gollem
    public void ChangeEnergy(float value)
    {
        UIManager.Instance.UpdateEnergy(value);
    }

    public void EnableMagicStones(bool enabled)
    {
        for(int i=0;i<magicStoneList.Count;i++)
        {
            magicStoneList[i].EnabledAnimation(enabled);
        }
    }
    //Jester
    public void IncreaseCameraSize(float sizeIncrement)
    {
        cameraScript.ChangeSizeAnimation(sizeIncrement);
    }

    //Undertaker
    public void SetPlant(Plant p)
    {
        plantList.Add(p);
        UIManager.Instance.ChangeButtonText(plantList.Count.ToString());
    }

    public void DestroyPlant(Plant p)
    {
        plantList.Remove(p);
        ChangePlantText();
    }
    public void DestroyPlants()
    {
        for(int i=0;i<plantList.Count;i++)
        {
            plantList[i].Die();
        }
        plantList.Clear();
        ChangePlantText();
    }

    public int PlantCount()
    {
        return plantList.Count;
    }
    public void ChangePlantText()
    {
        UIManager.Instance.ChangeButtonText(plantList.Count.ToString());
    }

    public bool CanSpawnPlant()
    {
        UndertakerScript u = playerScript as UndertakerScript;
        return u.GetAssistantNum() <= 0 && PlantCount() < u.GetMaxAssistants();
    }
    //////////////
    

    public bool IsOnScreen(Vector3 pos) //Check if a position is on camera
    {
        Vector3 point = cam.WorldToViewportPoint(pos);

        return point.x > 0 && point.y >= 0 && point.x <= 1 && point.y <= 1;
    }

    public void SetWave(int wave) //Called by: SpawnManager(On next wave)
    {
        this.wave = wave;
        UIManager.Instance.UpdateWave(wave.ToString());
    }
	
	public void EnemyDeath() //Called by: EnemyController(on death)
    {
        if(resetCombo) resetCombo = false;
        enemKillCombo++;
        if (enemKillCombo > 1000) enemKillCombo = 1000;

        UIManager.Instance.UpdateCombo(enemKillCombo.ToString());

        if(classTried != GlobalVariables.CharactersTypes.King) chestScript.IncrementPunt(); 
    }

    public void SetEnemyDeath(EnemyController en) //Called by: EnemyController(on anounce death)
    {
        spMan.EnemyDeath(en);
    }

    public void NewEnemy(EnemyController en) //Called by: SlimeController(On Spawn mini Slimes)
    {
        spMan.NewEnemy(en);
    }

    public void GetKey() // Called by: King Controller(on collide with Key)
    {
        chestScript.IncrementPunt();
    }

    public void IncreaseMoney(int quantity) //Called by: PlayerController(On collide with coin), KeyChest(On open)
    {
        money += quantity;
        if (money > 10000) money = 10000;

        UIManager.Instance.UpdateMoney(((int)money).ToString());
    }
	
    public void EndOfGame() //Called by: PlayerController(On death)
    {

        isPlaying = false;
        StartCoroutine(CloseGame());
    }

    IEnumerator CloseGame()
    {
        yield return new WaitForSeconds(3);
        SetEndData();
        UIManager.Instance.EndOfGame();
    }
    
    public void ChangeButtonText(string text)
    {
        UIManager.Instance.ChangeButtonText(text);
    }

	//////Power ups////
	public void ChangeSpeedTraps(float vel) //Called by: PowerUpManager(On get powerUp)
    {
        for(int i=0;i<trapList.Count;i++)
        {
            trapList[i].changeVel(vel);
        }
    }
	
	public void KillAllEnemies() //Called by: PowerUpManager(On get powerUp)
    {
        spMan.KillAllEnemies();
    }
	
	public void StunAllEnemies(float stunDuration) //Called by: PowerUpManager(On get powerUp)
    {
        spMan.StunAllEnemies(stunDuration);
    }
	
	public void ChangePlayerVel(float velIncrement) //Called by: PowerUpManager(On get powerUp)
    {
        playerScript.ChangeVel(velIncrement);
    }

    public void ActivatePlayerShield() //Called by: PowerUpManager(On get powerUp)
    {
        playerScript.ActivateShield(true);
    }

    public void UsePower() //Called by: UIManager(on press button)
    {
        if(playerScript!=null)
        {
            playerScript.UsePower();
        }
    }

    public void EnableHability(bool enable) //Called by: Player(on use/Reload hability)
    {
        UIManager.Instance.EnableHability(enable);
    }


    //////Gets///
    public GlobalVariables.CharactersTypes GetClassTried()
    {
        return classTried;
    }
	
	public void GetPower(GlobalVariables.Powers pow, bool activate)//Called by: PlayerController(On get powerUp)
    {
        puMan.GetPower(pow, activate);
    }
	
	public int GetLastCombo() //Called by: EnemyLootController(On enemy death)
    {
        if (enemKillCombo == 0) return lastEnemKillCombo;
        else return enemKillCombo;
    }

	public bool Playing()
    {
        return isPlaying;
    }

    public int GetCoins()
    {
        return money;
    }
	
    public bl_Joystick GetJoystick()
    {
        return joystick;
    }
	//////////PRIVATE FUNCTIONS//////////
	
	void StartGame()
    {
        //GetPlayerSelected
        if(!proves)classTried = (GlobalVariables.CharactersTypes)PlayerPrefs.GetInt("PlayerSelected",0);

        //SpawnObjects
        objMan.SpawnWorld();
        enemyRootSpawn = objMan.GetEnemyRoot();
        keyRootSpawn = objMan.GetKeyRoot();

        chestScript = objMan.GetChestScript();
        playerScript = objMan.GetPlayerController();
        trapList = objMan.GetTrapList();
        magicStoneList = objMan.GetMagicStonesList();

        surface = objMan.GetSurface();
        navAdmin.setSurface(surface);
        navAdmin.Bake();


        //Config GameObjects
        cameraScript.ConfigureCamera(playerScript.transform);
        UIManager.Instance.Config(classTried);
 
        //Inicialize variables
        lastEnemKillCombo = 0;
        isPlaying = true;
        gameCount = 0;
        money = 0;
        enemKillCombo = 0;
        resetCombo = false;
		
		spMan.StartGame(classTried, enemyRootSpawn, keyRootSpawn);

        ConfigClass();
        StartUI();
    }
	
	//Changes config depends on Class tried if needed
	void ConfigClass()
    {
        switch(classTried)
        {
            case GlobalVariables.CharactersTypes.Jester:
                cameraScript.IncreaseSize(extraCameraSize);
                break;
        }
    }
	
	//Start all texts needed
    void StartUI()
    {
        UIManager.Instance.UpdateCombo(enemKillCombo.ToString());
    }
    void SetEndData()
    {
        if(wave > SaveSystem.maxWave)
        {
            SaveSystem.maxWave = wave;
        }
        SaveSystem.coins += money;
        if (SaveSystem.coins > 10000) SaveSystem.coins = 10000;
        SaveSystem.SaveData();

        //To select character directly
       // SaveSystem.SelectCharacter = true;

        //Change ui;
        UIManager.Instance.UpdateBestWave(SaveSystem.maxWave.ToString());
        UIManager.Instance.UpdateTotalMoney(SaveSystem.coins.ToString());
    }



}
