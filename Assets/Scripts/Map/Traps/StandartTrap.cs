using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartTrap : Traps
{
    [Header("StandartTrap Config")]
    public ParticleSystem effects;
    

    protected override void Activating()
    {
        base.Activating();
        
    }

    protected override void Reloading()
    {
        base.Reloading();

    }

    protected override void ChangeAnim()
    {
        base.ChangeAnim();
        if(effects != null)
        {
            switch (actualState)
            {
                case States.Waiting:
                    effects.Stop();
                    break;
                case States.Activating:
                    effects.Play();
                    break;
                case States.Reloading:
                    effects.Stop();
                    break;
            }
        }
    }
}
