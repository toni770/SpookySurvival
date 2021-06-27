using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIntro : MonoBehaviour
{
    public MenuController menu;

    
    public void EndOfAnimation()
    {
        menu.ShowSelectButtons();
    }
}
