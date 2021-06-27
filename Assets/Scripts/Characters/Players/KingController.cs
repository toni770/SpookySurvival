using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can get keys
public class KingController : PlayerController
{
    [Header("KingConfig")]
    public int minCoin = 50;
    public int incCoin = 10;

    int actualminCoin;

    private void Start()
    {
        actualminCoin = minCoin;
        ManagerGame.Instance.ChangeButtonText(actualminCoin.ToString());
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.tag == "Key")
        {
            ManagerGame.Instance.GetKey();
            Destroy(other.gameObject);
        }
    }

    protected override void DoPower()
    {
        if(ManagerGame.Instance.GetCoins() >= actualminCoin )
        {
            ManagerGame.Instance.GetPower(GlobalVariables.Powers.EnemyDeath, true);
            ManagerGame.Instance.IncreaseMoney(-actualminCoin);

            actualminCoin += incCoin;
            ManagerGame.Instance.ChangeButtonText(actualminCoin.ToString());
        }
    }
        
    

    protected override void DisablePower()
    {
        if(ManagerGame.Instance.GetCoins() >= actualminCoin)
         base.DisablePower();
    }
}
