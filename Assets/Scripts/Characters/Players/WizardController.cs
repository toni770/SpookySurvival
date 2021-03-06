using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can Dash 
public class WizardController : PlayerController
{
    [Header("Wizard Config")]
    public float dashSpeed;
    public float blackHoleLife = 3;
    public GameObject blackHole;

    float dashCount = 0;
    bool dashing = false;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (dashing)
        {
            dashCount += Time.deltaTime;
            if(dashCount >= inmunityDuration)
            {
                dashCount = 0;
                dashing = false;
                ChangeVel(-dashSpeed);
            }
        }

    }
    protected override void DoPower()
    {
        if (ManagerGame.Instance.charLvl > 1)
        {
            GameObject obj = Instantiate(blackHole, transform.position, Quaternion.identity);
            obj.GetComponent<BlackHole>().lifeTime = blackHoleLife;
        }

        GetInmunity();
        ChangeVel(dashSpeed);
        dashing = true;
    }

    protected override void GetDirection(ref float h, ref float v) //When no input detected, moves in the direction char is looking
    {
        if(!dashing)
            base.GetDirection(ref h, ref v);
        else
        {
            if (h == 0 && v == 0)
            {
                Vector3 dir = transform.forward;

                h = Mathf.Round(dir.x);
                v = Mathf.Round(dir.z);
            }
        }

    }
}
