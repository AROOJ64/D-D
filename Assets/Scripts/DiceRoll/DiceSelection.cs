using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceSelection : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown diceDropdown;    
    [SerializeField] private List<GameObject> dicePrefabs;  

    private GameObject selectedDicePrefab;          
    public GameObject GetSelectedDicePrefab()
    {
        return selectedDicePrefab;
    }

    private void Start()
    {
        UpdateSelectedDice(diceDropdown.value);
        diceDropdown.onValueChanged.AddListener(UpdateSelectedDice);
    }
    private void UpdateSelectedDice(int index)
    {
        if (index >= 0 && index < dicePrefabs.Count)
        {
            selectedDicePrefab = dicePrefabs[index];
        }
        else
        {
            Debug.LogError("Invalid dice selection from dropdown.");
        }
    }
}
