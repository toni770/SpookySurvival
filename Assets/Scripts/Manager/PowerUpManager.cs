using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
	///////////PUBLIC VARS/////////
	
    [Header("+Vel")]
    public float velIncrement = 10;
    public float velIncDuration = 5;

    [Header("+VelTraps")]
    public float velTrapIncDuration = 10;

    [Header("StunEnemies")]
    public float stunDuration = 5;

	///////////PRIVATE VARS/////////
    GlobalVariables.Powers actualPower;
    float count;
    float limit;
    bool powerActivate = false;

    ManagerGame gm;

	void Awake()
	{
		actualPower = GlobalVariables.Powers.None;
        gm = GetComponent<ManagerGame>();
	}

    void Update()
    {
        if(powerActivate)
        {
            count += Time.deltaTime;
            if(count >= limit)
            {
                GetPower(actualPower, false); ///Disable power
            }
        }
    }

    public void GetPower(GlobalVariables.Powers pow, bool activate)
    {
        if (activate && powerActivate) GetPower(actualPower, false); //Disable current Power before get new one.
		
        powerActivate = activate;
        count = 0;
		
        if (activate) actualPower = pow;
        else actualPower = GlobalVariables.Powers.None;
		
		//Give power up depends on type.
        switch(pow)
        {
            case GlobalVariables.Powers.Fast: ///Increase Player Speed for a while
			
                limit = velIncDuration;
                if (activate) gm.ChangePlayerVel(velIncrement);
                else gm.ChangePlayerVel(-velIncrement);
				
                break;
            case GlobalVariables.Powers.EnemyDeath: //Kill all enemies 
			
                limit = 0;
                if (activate) gm.KillAllEnemies();
				
                break;
            case GlobalVariables.Powers.EnemyStun: //Stun all enemies
			
                limit = 0;
                if (activate) gm.StunAllEnemies(stunDuration);
				
                break;
            case GlobalVariables.Powers.FastTraps: //Traps reload faster for a while
			
                limit = velTrapIncDuration;
                if (activate) gm.ChangeSpeedTraps(0.5f);
                else gm.ChangeSpeedTraps(2);
				
                break;
            case GlobalVariables.Powers.Shield: //Gives to player a shield.
			
                limit = 0;
                if (activate) gm.ActivatePlayerShield();
				
                break;
        } 
    }
}
