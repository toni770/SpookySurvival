using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HabilityInfo : MonoBehaviour
{
    [Header("GameObjects")]
    public Text titleText;
    public Text descText;
    public GameObject info;

    GameObject openedHab;

    public void ObrirInfo(GameObject hab)
    {
        if (openedHab != null) tancarInfo();
        else
        {
            openedHab = hab;

            titleText.text = hab.GetComponent<HabilityButton>().tittle;
            descText.text = hab.GetComponent<HabilityButton>().desc;
            info.SetActive(true);
            openedHab.transform.parent = info.transform;
        }
    }

    public void tancarInfo()
    {
        if (openedHab != null)
        {
            openedHab.transform.parent = transform;
            openedHab.transform.SetSiblingIndex(0);
        }
        info.SetActive(false);
        openedHab = null;
    }

}
