using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dice : MonoBehaviour
{
    public Transform[] diceFaces;
    private Rigidbody rb;

    public int diceIndex = -1;
    public int topFaceValue;
   
    public static UnityAction<int, int> OnDiceResult;

    private DiceManager diceManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        diceManager=FindObjectOfType<DiceManager>(); 
    }

    private void Update()
    {
        if (rb.velocity.sqrMagnitude == 0f && rb.angularVelocity.sqrMagnitude == 0f)
        {
            GetNumberOnTopFace();
        } 
    }

    [ContextMenu(itemName:"Get Top Face")]
    public void GetNumberOnTopFace()
    {
        if (diceFaces == null) return;

        var topFace = 0;
        var lastYPosition = diceFaces[0].position.y;

        for(int i = 0; i < diceFaces.Length; i++)
        {
            if (diceFaces[i].position.y > lastYPosition)
            {
                lastYPosition = diceFaces[i].position.y;
                topFace = i;
            }
        }
        topFaceValue = topFace + 1;
        Debug.Log($"Dice result {topFaceValue}");

        diceManager.FaceSummition();
        OnDiceResult?.Invoke(diceIndex, topFaceValue);
    }

    public void DicePhysics(GameObject dice)
    {
        rb = dice.GetComponent<Rigidbody>();
        rb.AddTorque(
            Random.Range(0, 700),
            Random.Range(0, 700),
            Random.Range(0, 700)
        );
    }
}
