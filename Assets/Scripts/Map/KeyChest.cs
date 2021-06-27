using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Increment lvl by getting keys. Reward: Coins
public class KeyChest : Chest
{
    public int[] coinValue;
    public GameObject coins;


    protected override void GiveReward()
    {
        base.GiveReward();
        ManagerGame.Instance.IncreaseMoney(coinValue[lvl]);
        coins.SetActive(lvl > 0);
    }
    
}
