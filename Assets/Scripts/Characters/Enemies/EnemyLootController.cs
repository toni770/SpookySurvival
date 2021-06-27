using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Give a reward depends on the combo(num Of Enemy Killed In a Short Time)
public class EnemyLootController : MonoBehaviour
{
	///////////PUBLIC VARS/////////
	[Header("Config")]
	public GameObject[] coins;
	public int[] comboMinimum;
    public float[] coinProb;


	[Header("King Config")]
	public GameObject Key;
    public float keyProb = 15;

    [Header("Undertaker Config")]
    public GameObject plant;
	
	///////////PRIVATE VARS/////////
    int combo;
    int lvlOfLoot;
    GameObject[] coinsToSpawn;
    GlobalVariables.CharactersTypes classTried;
    Vector3 spawnPos;

    Traps trapOnDied;
    GameObject obj;
	
    ///////////UNITY FUNCTIONS////////
    void Start()
    {
        lvlOfLoot = 0;
        combo = 0;
        coinsToSpawn = new GameObject[coins.Length];
        classTried = ManagerGame.Instance.GetClassTried();
    }
	
	///////////PUBLIC FUNCTIONS////////
	//Spawn coin depends on the probability of each type of coin.
    public void Loot()
    {
        GetCombo();
        ConfigLevel();

        spawnPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        int rand = Random.Range(1, 101);
        if (rand > 0 && rand <= coinProb[0])
                { Instantiate(coinsToSpawn[1], spawnPos, Quaternion.identity); }
        else if (rand > coinProb[0] && rand <= coinProb[0] + coinProb[1] + 1)
                { Instantiate(coinsToSpawn[2], spawnPos, Quaternion.identity); }
        else if (rand > coinProb[0] + coinProb[1] + 1 && rand <= coinProb[0] + coinProb[1] + coinProb[2] + 2)
                { Instantiate(coinsToSpawn[3], spawnPos, Quaternion.identity); }
        else
        {
            if (coinsToSpawn[0] != null)
            {
                Instantiate(coinsToSpawn[0], spawnPos, Quaternion.identity);
            }
        }
		
		//Extra: King
        if (classTried == GlobalVariables.CharactersTypes.King) { SpawnKey(); }
        if (classTried == GlobalVariables.CharactersTypes.Undertaker) { SpawnPlant(); }
    }
	
    public void SetTrapActived(Traps trap)
    {
        trapOnDied = trap;
    }
	///////////PRIVATE FUNCTIONS////////
    void SpawnKey() //Only with King character
    {
        int rand = Random.Range(1, 101);
        if (rand <= keyProb)
        {
            Instantiate(Key, transform.position, Quaternion.identity);
        }
    }
    void SpawnPlant() //Only with Undertaker character
    {
        if(trapOnDied != null && !trapOnDied.IsFrozen() && ManagerGame.Instance.CanSpawnPlant())
        {
            obj = Instantiate(plant, transform.position, Quaternion.identity);
            obj.GetComponent<Plant>().AssignTrap(trapOnDied);
            ManagerGame.Instance.SetPlant(obj.GetComponent<Plant>());
        }
    }
    void ConfigLevel() //Change lvl if combo it's bigger than the minimum 
								//and increase the reward probability depends on the lvl.
    {
        for(int j=0;j<comboMinimum.Length;j++)
        {
            if(combo >= comboMinimum[j])
            {
                lvlOfLoot++;
            }
        }
        for(int i=0;i<coinsToSpawn.Length;i++)
        {
            int level = i + lvlOfLoot;
            if (level >= coinsToSpawn.Length) level = coinsToSpawn.Length - 1;
            coinsToSpawn[i] = coins[level];
        }
    }

    void GetCombo()
    {
        combo = ManagerGame.Instance.GetLastCombo();
    }
}
