using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoCharacters : MonoBehaviour
{

    [Header("Info")]
    public string[] titlesList;
    [TextArea(5, 5)]
    public string[] descriptionsList;

    [Header("Elements")]
    public Text title;
    public Text description;

    public void SetInfo(int character)
    {
        title.text = titlesList[character];
        description.text = descriptionsList[character];
    }
}
