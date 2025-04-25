using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CH_SheetManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField display;

    [SerializeField] private TMP_InputField[] inputField;

    void Start()
    {
        for (int i = 0; i < inputField.Length; i++)
        {
            inputField[i].text = PlayerPrefs.GetString(i.ToString());
        }
    }
    public void savedata()
    {
        for(int i = 0; i < inputField.Length; i++)
        {
            PlayerPrefs.SetString(i.ToString(), inputField[i].text);
            PlayerPrefs.Save();
        }
    }
}
