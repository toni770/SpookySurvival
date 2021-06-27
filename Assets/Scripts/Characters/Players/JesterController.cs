using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can't stop moving
public class JesterController : PlayerController
{
	///////////PUBLIC VARS/////////
    [Header("JesterConfig")]
    public float extraCameraSize = 2;
    [Header("PowerConfig")]
    public float timeBtwFire = 0.5f;
    public float fireDuration = 3;
    public float velIncrement = 3;
    public float powerDuration = 3;
    public float cameraSizeReduction = 2;
    

    public GameObject Fire;
    public Transform FirePosition;

    bool auto = false;
    bool powerActive = false;

    float fireCount = 0;
    float powerCount = 0;

    GameObject obj;
	///////////FUNCTIONS/////////
    protected override void GetDirection(ref float h, ref float v) //When no input detected, moves in the direction char is looking
    {
        base.GetDirection(ref h, ref v);
        if (auto)
        {
            if(h == 0 && v == 0)
            {
                Vector3 dir = transform.forward;

                h = Mathf.Round(dir.x);
                v = Mathf.Round(dir.z);       
            }
        }
        else
        {
            if (h != 0 || v != 0)
                auto = true;
        }
    }

    protected override void DoPower()
    {
        if(!powerActive)
        {
            ActiveFastMode();
        }
        if (!auto) auto = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(powerActive)
        {
            fireCount += Time.deltaTime;
            if(fireCount > timeBtwFire)
            {
                fireCount = 0;
                obj = Instantiate(Fire, FirePosition.position, Quaternion.identity);
                obj.GetComponent<Fire>().SetLifeTime(fireDuration);
            }

            powerCount += Time.deltaTime;
            if(powerCount > powerDuration)
            {
                DisableFastMode();
            }
        }
    }

    void ActiveFastMode()
    {
        powerActive = true;
        ChangeVel(velIncrement);
        anim.SetBool("Fast", true);
        ManagerGame.Instance.IncreaseCameraSize(-cameraSizeReduction);
    }
    void DisableFastMode()
    {
        powerCount = 0;
        fireCount = 0;
        powerActive = false;
        anim.SetBool("Fast", false);
        EnablePower();

        ManagerGame.Instance.IncreaseCameraSize(cameraSizeReduction);
        ChangeVel(-velIncrement);
    }
    protected override void EnablePower()
    {
        if(!powerActive && habilityCount == 0)
            base.EnablePower();
    }
}
