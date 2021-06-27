using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStone : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void EnabledAnimation(bool enabled)
    {
        anim.SetBool("Enabled", enabled);
    }
}
