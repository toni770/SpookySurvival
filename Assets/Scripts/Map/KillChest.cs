using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Increment lvl by killing mobs. Reward: Power Ups
public class KillChest : Chest
{
	///////////PUBLIC VARS/////////
    [Header("Power Ups")]
    public GameObject[] lvl1;
    public GameObject[] lvl2;
    public GameObject[] lvl3;
    public GameObject[] lvl4;

    ///////////PRIVATE VARS/////////
    List<GameObject[]> lvlList;
	
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        lvlList = new List<GameObject[]>();
        lvlList.Add(lvl1);
        lvlList.Add(lvl2);
        lvlList.Add(lvl3);
        lvlList.Add(lvl4);
    }

    protected override void GiveReward()
    {
        base.GiveReward();

        if (lvl > 0)
        {
            SpawnPower(lvlList[lvl - 1]);
        }
    }

    void SpawnPower(GameObject[] items)
    {
        if (items.Length > 0)
        {
            int rand = Random.Range(0, items.Length);
            Instantiate(items[rand], transform.position, Quaternion.identity);
        }
    }
}
