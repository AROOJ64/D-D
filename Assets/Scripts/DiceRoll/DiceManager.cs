using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public List<Dice> diceList;

    [SerializeField] private TextMeshProUGUI sum;

    private void Start()
    {
        diceList = new List<Dice>();
    }

    public void FaceSummition()
    {
        int totalSum = 0;
        foreach (var dice in diceList)
        {
            totalSum += dice.topFaceValue;
        }
        sum.text = totalSum.ToString();
        Debug.Log($"Total Dice Sum: {totalSum}");
    }
    public void ClearDiceList()
    {
        if (diceList.Count > 0)
        {
            // Loop all dice in the list
            foreach (var dice in diceList)
            {
                Destroy(dice.gameObject);
            }
            // Clear list
            diceList.Clear();
            sum.text = "0";

            Debug.Log("All elements in diceList have been removed and destroyed.");
        }
    }
}
    

